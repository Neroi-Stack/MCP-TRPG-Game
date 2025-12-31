using ModelContextProtocol.Server;

namespace ToolBox.Tools.GamePrompt;
[McpServerPromptType]
public static partial class TrpgPrompt
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}