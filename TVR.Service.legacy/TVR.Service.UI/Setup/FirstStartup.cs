using System;
using TVR.Service.Core.IO;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Config;

namespace TVR.Service.UI.Setup
{
    public class FirstStartup
    {

        public static void CheckFirstStart()
        {
            if (FileManager.Instance.IsFirstStart)
            {
                var welcomeWindow = new WelcomeWindow();
                var newCameraDialog = new NewCameraDialog();
                if (welcomeWindow.ShowDialog() == true && newCameraDialog.ShowDialog() == true)
                {
                    var defaultUserConfig = new UserConfig
                    {
                        CameraInfo = new CameraInfo() { Index = 0, ProfileName = newCameraDialog.CameraProfile.Identifier },
                        Offset = Vector3.Zero,
                        InputConfig = new InputConfig() { Latency = 2, PoseResetDelay = 0.2, UpdateRate = 120 },
                        HardwareConfig = new HardwareConfig() { SphereDistance = 0.055, SphereSize = 0.04 }
                    };
                    ConfigIO.WriteUserConfig(defaultUserConfig);
                    FileManager.Instance.ProfilesFolder.Create();
                    CameraProfileIO.WriteCameraProfile(newCameraDialog.CameraProfile);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

    }
}
