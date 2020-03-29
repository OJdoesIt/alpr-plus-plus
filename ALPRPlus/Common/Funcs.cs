using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.Common
{
    internal static class Funcs
    {
        internal static bool PreloadChecks()
        {
            return (IsRPHVersionRecentEnough() && IsLSPDFRVersionRecentEnough() /*&& IsCommonDLLValid()*/);
        }

        public static void CheckForUpdates()
        {
            Stealth.Common.Functions.UpdateFuncs.CheckForUpdates(Constants.LCPDFRDownloadID, Globals.Version, Globals.VersionInfo.ProductName, true);
        }

        public static bool IsCommonDLLValid()
        {
            return CheckAssemblyVersion("Stealth.Common.dll", "Stealth.Common DLL", Constants.ReqCommonVersion);
        }

        public static bool IsRPHVersionRecentEnough()
        {
            return CheckAssemblyVersion("RAGEPluginHook.exe", "RAGE Plugin Hook", Constants.ReqRPHVersion);
        }

        public static bool IsLSPDFRVersionRecentEnough()
        {
            return CheckAssemblyVersion(@"Plugins\LSPD First Response.dll", "LSPDFR", Constants.ReqLSPDFRVersion);
        }

        private static bool CheckAssemblyVersion(string pFilePath, string pFileAlias, string pRequiredVersion)
        {
            bool isValid = true;

            try
            {
                if (System.IO.File.Exists(pFilePath))
                {
                    Version mRequiredVersion = Version.Parse(pRequiredVersion);
                    Version mInstalledVersion = Version.Parse(FileVersionInfo.GetVersionInfo(pFilePath).FileVersion);

                    if (mRequiredVersion.CompareTo(mInstalledVersion) > 0)
                    {
                        DisplayNotification("Dependency Check", string.Format("~r~ERROR: ~w~v{0} of ~b~{1} ~w~required; v{2} found.", pRequiredVersion, pFileAlias, mInstalledVersion));
                        Logger.LogTrivial(string.Format("ERROR: {0} requires at least v{1} of {2}. Older version ({3}) found; {0} cannot run.", Globals.VersionInfo.ProductName, pRequiredVersion, pFileAlias, mInstalledVersion));
                        isValid = false;
                    }
                }
                else {
                    DisplayNotification("Dependency Check", string.Format("~r~ERROR: ~b~{0} ~w~is missing! ~n~Initialization ~r~aborted!", pFileAlias));
                    Logger.LogTrivial(string.Format("ERROR: {0} requires at least v{1} of {2}. {2} not found; {0} cannot run.", Globals.VersionInfo.ProductName, pRequiredVersion, pFileAlias));
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerboseDebug(string.Format("Error while checking for {0}: {1}", pFileAlias, ex.ToString()));
            }

            return isValid;
        }

        internal static uint DisplayNotification(string subtitle, string text)
        {
            return DisplayNotification(Globals.VersionInfo.ProductName, subtitle, text);
        }

        internal static uint DisplayNotification(string title, string subtitle, string text)
        {
            return Stealth.Common.Functions.GameFuncs.DisplayNotification(title, subtitle, text);
        }

        internal static bool IsTrafficPolicerRunning()
        {
            return IsLSPDFRPluginRunning("Traffic Policer", new Version(6, 14, 4, 2));
        }

        internal static bool IsLSPDFRPluginRunning(string pName, Version pMinVersion = null)
        {
            try
            {
                if (DoesLSPDFRPluginExist(pName, pMinVersion))
                {
                    Logger.LogTrivialDebug("Plugin exists");

                    foreach (Assembly a in Functions.GetAllUserPlugins())
                    {
                        AssemblyName an = a.GetName();
                        if (an.Name.ToLower() == pName.ToLower())
                        {
                            if (pMinVersion == null || an.Version.CompareTo(pMinVersion) >= 0)
                            {
                                Logger.LogTrivialDebug("Plugin is running");
                                return true;
                            }
                        }
                    }

                    Logger.LogTrivialDebug("Plugin is not running");
                    return false;
                }
                else
                {
                    Logger.LogTrivialDebug("Plugin does not exist");
                    return false;
                }
            }
            catch
            {
                Logger.LogTrivialDebug("Error getting plugin -- returning false");
                return false;
            }
        }

        private static bool DoesLSPDFRPluginExist(string pName, Version pMinVersion = null)
        {
            string mFilePath = string.Format(@"Plugins\LSPDFR\{0}.dll", pName);

            if (System.IO.File.Exists(mFilePath))
            {
                if (pMinVersion == null)
                {
                    return true;
                }
                else
                {
                    Version mInstalledVersion = null;

                    if (Version.TryParse(FileVersionInfo.GetVersionInfo(mFilePath).FileVersion, out mInstalledVersion) == true)
                    {
                        if (pMinVersion.CompareTo(mInstalledVersion) > 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
