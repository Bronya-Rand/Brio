﻿using Brio.Config;
using Brio.Entities.Camera;
using Brio.Game.Camera;
using Brio.Game.GPose;
using Brio.UI.Widgets.Camera;
using Brio.UI.Windows.Specialized;

namespace Brio.Capabilities.Camera;

public class BrioCameraCapability : CameraCapability
{
    private readonly CameraWindow _cameraWindow;
    private readonly VirtualCameraManager _virtualCameraService;
    public readonly ConfigurationService configurationService;

    public BrioCameraCapability(CameraEntity parent, VirtualCameraManager virtualCameraService, GPoseService gPoseService, CameraWindow cameraWindow, ConfigurationService configService) : base(parent, gPoseService)
    {
        _virtualCameraService = virtualCameraService;
        _cameraWindow = cameraWindow;

        configurationService = configService;

        Widget = new BrioCameraWidget(this);
    }

    public override void OnEntitySelected()
    {
        _virtualCameraService.SelectCamera(VirtualCamera);
    }

    public void ShowCameraWindow()
    {
        _cameraWindow.IsOpen = true;
    }
}
