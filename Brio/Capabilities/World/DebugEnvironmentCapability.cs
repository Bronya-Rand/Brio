using Brio.Capabilities.Core;
using Brio.Config;
using Brio.Entities.Core;
using Brio.Game.GPose;
using Brio.Game.World;
using Brio.IPC;
using Brio.UI.Widgets.World;

namespace Brio.Capabilities.World;

public class DebugEnvironmentCapability : Capability
{
    public bool IsDebug => _configService.IsDebug;

    public DynamisService DynamisIPC => _dynamisIPC;
    public EnvironmentService Environment => _environmentService;

    public GPoseService GPoseService => _gPoseService;

    public readonly EnvironmentService _environmentService;
    private readonly ConfigurationService _configService;
    private readonly DynamisService _dynamisIPC;
    private readonly GPoseService _gPoseService;

    public DebugEnvironmentCapability(Entity parent, GPoseService gPoseService, EnvironmentService environmentService, DynamisService dynamisIPC, ConfigurationService configService) : base(parent)
    {
        _environmentService = environmentService;

        _configService = configService;
        _dynamisIPC = dynamisIPC;
        _gPoseService = gPoseService;

        Widget = new DebugEnvironmentWidget(this);
    }
}
