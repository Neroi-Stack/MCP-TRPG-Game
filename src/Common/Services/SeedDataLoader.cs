using System.Globalization;
using Common.Interface;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

/// <summary>
/// 自動載入 seed 資料夾所有 CSV 檔案並插入資料庫
/// </summary>
public class SeedDataLoader : ISeedDataLoader
{
	private readonly DbContext _context;
	private readonly string _seedFolder = Path.Combine(AppContext.BaseDirectory, "seed");

	public SeedDataLoader(DbContext context)
	{
		_context = context;
	}

	public void LoadAllSeedData()
	{
		if (!Directory.Exists(_seedFolder)) return;
		var dbContextType = _context.GetType();
		var modelNamespace = typeof(SeedDataLoader).Namespace?.Replace("Services", "Data.Models");
		// Collect files and sort by leading numeric prefix if present (e.g. 0_, 1_).
		// Files with an "n_" or "n-" prefix are intended as join/relationship data
		// and should be executed last.
		var files = Directory.GetFiles(_seedFolder, "*.csv")
			.Select(f => new
			{
				Path = f,
				Name = Path.GetFileNameWithoutExtension(f),
				Order = GetLeadingNumber(Path.GetFileNameWithoutExtension(f))
			})
			// Only numeric ordering then filename; n_ / n- files are not special anymore
			.OrderBy(x => x.Order ?? int.MaxValue)
			.ThenBy(x => x.Name)
			.ToList();

		foreach (var fileInfo in files)
		{
			var file = fileInfo.Path;
			var fileName = fileInfo.Name;
			var modelFileName = StripLeadingNumberPrefix(fileName);
			var modelName = ToPascalCase(modelFileName);
			Type? modelType = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => SafeGetTypes(a))
				.FirstOrDefault(t => string.Equals(t.Name, modelName, StringComparison.OrdinalIgnoreCase)
					&& t.Namespace != null && t.Namespace.EndsWith(".Data.Models", StringComparison.OrdinalIgnoreCase));

			if (modelType == null)
			{
				Console.WriteLine($"找不到對應的 model 類別給檔案: {fileName} (嘗試: {modelName})");
				continue;
			}
			if (modelType.Name.Equals("CharacterAttribute", StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					var conn = _context.Database.GetDbConnection();
					conn.Open();
					using var cmd = conn.CreateCommand();
					cmd.CommandText = "PRAGMA foreign_key_list('CharacterAttribute');";
					using var fkReader = cmd.ExecuteReader();
					Console.WriteLine("CharacterAttribute foreign keys:");
					while (fkReader.Read())
					{
						var table = fkReader.IsDBNull(2) ? "" : fkReader.GetString(2);
						var from = fkReader.IsDBNull(3) ? "" : fkReader.GetString(3);
						var to = fkReader.IsDBNull(4) ? "" : fkReader.GetString(4);
						Console.WriteLine($"  -> references table={table}, from={from}, to={to}");
					}
					conn.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"無法查詢 foreign_key_list: {ex.Message}");
				}
			}
			var dbSetProp = dbContextType.GetProperties()
				.FirstOrDefault(p => p.PropertyType.IsGenericType &&
					p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
					p.PropertyType.GetGenericArguments()[0] == modelType);
			if (dbSetProp == null) continue;
			var dbSet = dbSetProp.GetValue(_context);
			if (dbSet == null) continue;
			var queryable = dbSet as IQueryable;
			if (queryable != null && queryable.Cast<object>().Any())
			{
				Console.WriteLine($"跳過已存在資料表的種子: {fileName}");
				continue;
			}

			using var reader = new StreamReader(file);
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			csv.Read();
			csv.ReadHeader();
			var headerRecord = csv.HeaderRecord?.Select(h => h?.Trim()).ToArray();

			var records = new List<object>();
			while (csv.Read())
			{
				var record = Activator.CreateInstance(modelType);
				if (record == null) continue;

				if (headerRecord != null)
				{
					foreach (var header in headerRecord)
					{
						if (string.IsNullOrWhiteSpace(header)) continue;
						var prop = modelType.GetProperties()
							.FirstOrDefault(p => string.Equals(p.Name, header, StringComparison.OrdinalIgnoreCase));
						if (prop != null && csv.TryGetField(header, out string? value))
						{
							if (string.IsNullOrWhiteSpace(value))
							{
								if (prop.PropertyType == typeof(string))
								{
									prop.SetValue(record, string.Empty);
								}
								else if (Nullable.GetUnderlyingType(prop.PropertyType) != null || !prop.PropertyType.IsValueType)
								{
									prop.SetValue(record, null);
								}
								continue;
							}

							try
							{
								var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

								if (targetType == typeof(bool))
								{
									var convertedValue = value.Equals("True", StringComparison.OrdinalIgnoreCase) || value == "1";
									prop.SetValue(record, convertedValue);
								}
								else if (targetType == typeof(DateTime))
								{
									if (DateTime.TryParse(value, out var dateValue))
										prop.SetValue(record, dateValue);
								}
								else
								{
									var convertedValue = Convert.ChangeType(value, targetType);
									prop.SetValue(record, convertedValue);
								}
							}
							catch
							{
								Console.WriteLine($"無法轉換欄位 {header} 的值: {value}");
							}
						}
					}
				}
				var isActiveProp = modelType.GetProperty("IsActive");
				isActiveProp?.SetValue(record, true);
				var createdAtProp = modelType.GetProperty("CreatedAt");
				createdAtProp?.SetValue(record, DateTime.UtcNow);
				var updatedAtProp = modelType.GetProperty("UpdatedAt");
				updatedAtProp?.SetValue(record, DateTime.UtcNow);

				records.Add(record);
			}
			if (records.Count > 0)
			{
				try
				{
					var typedArray = Array.CreateInstance(modelType, records.Count);
					for (int i = 0; i < records.Count; i++)
					{
						typedArray.SetValue(records[i], i);
					}
					var addRangeMethod = dbSet.GetType().GetMethod("AddRange", new[] { modelType.MakeArrayType() });
					if (addRangeMethod != null)
					{
						addRangeMethod.Invoke(dbSet, new object[] { typedArray });
						_context.SaveChanges();
						_context.ChangeTracker.Clear();
						Console.WriteLine($"成功載入 {records.Count} 筆 {fileName} 資料");
					}
					else
					{
						Console.WriteLine($"找不到適合的 AddRange 方法給 {fileName}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"載入 {fileName} 資料時發生錯誤: {ex.Message}");
					_context.ChangeTracker.Clear();
				}
			}
		}
	}

	private static string ToPascalCase(string input)
	{
		if (string.IsNullOrWhiteSpace(input)) return input;
		var parts = input.Split(new[] { '_', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
		return string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1) : string.Empty)));
	}

	// Safely get types from assembly (some assemblies may throw on GetTypes)
	private static IEnumerable<Type> SafeGetTypes(System.Reflection.Assembly a)
	{
		try
		{
			return a.GetTypes();
		}
		catch
		{
			return Enumerable.Empty<Type>();
		}
	}

	private static int? GetLeadingNumber(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) return null;
		var parts = name.Split(new[] { '_', '-' }, 2);
		if (int.TryParse(parts[0], out var n)) return n;
		return null;
	}

	private static string StripLeadingNumberPrefix(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) return name;
		var parts = name.Split(new[] { '_', '-' }, 2);
		if (parts.Length == 2)
		{
			// numeric prefix (e.g. 0_, 1-)
			if (int.TryParse(parts[0], out _)) return parts[1];
		}
		return name;
	}

}
