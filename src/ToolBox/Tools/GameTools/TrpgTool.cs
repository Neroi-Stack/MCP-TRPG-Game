using ModelContextProtocol.Server;

namespace ToolBox.Tools.GameTools;
[McpServerToolType]
public static partial class TrpgTools
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}