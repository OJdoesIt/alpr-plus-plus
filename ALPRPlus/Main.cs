using LSPD_First_Response.Mod.API;
using Rage;
using Stealth.Plugins.ALPRPlus.Common;
using Stealth.Plugins.ALPRPlus.Core;
using System;
using System.Reflection;


namespace Stealth.Plugins.ALPRPlus
{
    internal sealed class Main : Plugin
    {
        public override void Initialize()
        {
            Config.Init();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        internal static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (!onDuty)
            {
                Globals.IsPlayerOnDuty = false;
                return;
            }

            if (!Funcs.PreloadChecks())
            {
                return;
            }

            StartPlugin(onDuty);
        }

        private static void StartPlugin(bool onDuty)
        {
            Globals.IsPlayerOnDuty = onDuty;

            if (onDuty)
            {
                Logger.LogTrivial(String.Format("{0} v{1} has been loaded!", Globals.VersionInfo.ProductName, Globals.VersionInfo.FileVersion));

                GameFiber.StartNew(delegate
                {
                    GameFiber.Wait(3000);
                    Funcs.DisplayNotification("~b~Developed By Stealth22", String.Format("~b~{0} v{1} ~w~has been ~g~loaded!~n~~b~Updated by OJdoesIt.", Globals.VersionInfo.ProductName, Globals.VersionInfo.FileVersion));
                    Funcs.CheckForUpdates();
                });

                Driver mDriver = new Driver();
                GameFiber.StartNew(mDriver.Launch);
                GameFiber.StartNew(mDriver.ListenForToggleKey);
                GameFiber.StartNew(mDriver.MonitorActiveTrafficStop);
                GameFiber.StartNew(mDriver.MonitorActivePursuit);
                GameFiber.StartNew(mDriver.ProcessWorldVehicles);
            }
        }

        public override void Finally()
        {
            Globals.IsPlayerOnDuty = false;
        }

        internal static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }
    }
}
