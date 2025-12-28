using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MCPTRPGGame.Data;

namespace MCPTRPGGame.Services;

/// <summary>
/// 自動載入 seed 資料夾所有 CSV 檔案並插入資料庫
/// </summary>
public class SeedDataLoader
{
    private readonly TrpgDbContext _context;
    private readonly string _seedFolder = "seed";

    public SeedDataLoader(TrpgDbContext context)
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
                Order = GetLeadingNumber(Path.GetFileNameWithoutExtension(f)),
                IsNPrefix = IsNPrefix(Path.GetFileNameWithoutExtension(f))
            })
            // numeric-ordered files first, then other non-n files, then n_ files last
            .OrderBy(x => x.Order ?? int.MaxValue)
            .ThenBy(x => x.IsNPrefix ? 1 : 0)
            .ThenBy(x => x.Name)
            .ToList();

        foreach (var fileInfo in files)
        {
            var file = fileInfo.Path;
            var fileName = fileInfo.Name;
            // If filename has a leading numeric prefix like "0_Scenario" or a join prefix like
            // "n_CharacterAttribute", strip it for model matching
            var modelFileName = StripLeadingNumberPrefix(fileName);
            Type? modelType = null;
            var candidates = new List<string> { modelFileName };
            if (modelFileName.EndsWith("es", StringComparison.OrdinalIgnoreCase))
                candidates.Add(modelFileName[..^2]);
            if (modelFileName.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                candidates.Add(modelFileName[..^1]);

            foreach (var cand in candidates)
            {
                modelType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(cand, StringComparison.OrdinalIgnoreCase) && t.Namespace == modelNamespace);
                if (modelType != null) break;
            }

            if (modelType == null)
            {
                Console.WriteLine($"找不到對應的 model 類別給檔案: {fileName} (嘗試: {string.Join(',', candidates)})");
                continue;
            }
            // If we're about to seed CharacterAttribute, print its FK constraints for diagnosis
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
                            // Allow setting `Id` from CSV so seeded relationships keep consistent keys

                            if (string.IsNullOrWhiteSpace(value))
                            {
                                // For string properties, set empty string instead of null
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
                    // 發生錯誤時也要清除變更追蹤
                    _context.ChangeTracker.Clear();
                }
            }
        }
    }

    // Helper: try parse leading number (e.g. "0_Scene" -> 0)
    private static int? GetLeadingNumber(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var parts = name.Split(new[] { '_', '-' }, 2);
        if (int.TryParse(parts[0], out var n)) return n;
        return null;
    }

    // Helper: strip leading numeric prefix if present (e.g. "0_Scene" -> "Scene")
    private static string StripLeadingNumberPrefix(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return name;
        var parts = name.Split(new[] { '_', '-' }, 2);
        if (parts.Length == 2)
        {
            // numeric prefix (e.g. 0_, 1-)
            if (int.TryParse(parts[0], out _)) return parts[1];
            // n_ or n- prefix for join tables (e.g. n_CharacterAttribute)
            if (parts[0].Equals("n", StringComparison.OrdinalIgnoreCase)) return parts[1];
        }
        return name;
    }

    // Helper: detect n_ or n- prefix (for join/relationship CSVs)
    private static bool IsNPrefix(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return name.StartsWith("n_", StringComparison.OrdinalIgnoreCase) || name.StartsWith("n-", StringComparison.OrdinalIgnoreCase);
    }
}
