using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MCPTRPGGame.Data;
using MCPTRPGGame.Models;

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
        var modelNamespace = typeof(SeedDataLoader).Namespace?.Replace("Services", "Models");
        // Collect files and sort by leading numeric prefix if present (e.g. 0_, 1_)
        var files = Directory.GetFiles(_seedFolder, "*.csv")
            .Select(f => new
            {
                Path = f,
                Name = Path.GetFileNameWithoutExtension(f),
                Order = GetLeadingNumber(Path.GetFileNameWithoutExtension(f))
            })
            .OrderBy(x => x.Order ?? int.MaxValue)
            .ThenBy(x => x.Name)
            .ToList();

        foreach (var fileInfo in files)
        {
            var file = fileInfo.Path;
            var fileName = fileInfo.Name;
            // If filename has a leading numeric prefix like "0_Scenario", strip it for model matching
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
            var dbSetProp = dbContextType.GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GetGenericArguments()[0] == modelType);
            if (dbSetProp == null) continue;
            var dbSet = dbSetProp.GetValue(_context);
            if (dbSet == null) continue;
            var queryable = dbSet as IQueryable;
            if (queryable != null && queryable.Cast<object>().Any()) continue;

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
                            if (string.Equals(header, "Id", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (string.IsNullOrWhiteSpace(value))
                            {
                                if (Nullable.GetUnderlyingType(prop.PropertyType) != null || !prop.PropertyType.IsValueType)
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
        if (parts.Length == 2 && int.TryParse(parts[0], out _)) return parts[1];
        return name;
    }
}
