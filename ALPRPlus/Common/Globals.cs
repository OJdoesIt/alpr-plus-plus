using LSPD_First_Response.Mod.API;
using Rage;
using Stealth.Plugins.ALPRPlus.API;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.Common
{
    internal static class Globals
    {
        internal const string cDLLPath = @"Plugins\LSPDFR\ALPRPlus.dll";

        private static FileVersionInfo mVersionInfo = null;
        internal static FileVersionInfo VersionInfo
        {
            get
            {
                if (mVersionInfo == null)
                    mVersionInfo = FileVersionInfo.GetVersionInfo(cDLLPath);

                return mVersionInfo;
            }
        }

        internal static Version Version
        {
            get
            {
                return Version.Parse(VersionInfo.FileVersion);
            }
        }

        internal static bool IsBeta
        {
            get
            {
#if BETA
                return true;
#else
                return false;
#endif
            }
        }

        internal static bool IsPlayerOnDuty { get; set; } = false;
        internal static Random Random { get; } = new Random();
        internal static Stopwatch AlertTimer { get; } = new Stopwatch();
        internal static DateTime ALPRLastReadyOrActivation { get; set; } = DateTime.Now;

        internal static Dictionary<Vehicle, bool> ALPRVehicles { get; set; } = new Dictionary<Vehicle, bool>();
        internal static Dictionary<Vehicle, ALPRScanResult> ScanResults { get; set; } = new Dictionary<Vehicle, ALPRScanResult>();

        internal static List<LHandle> TrafficStops { get; set; } = new List<LHandle>();
        internal static LHandle ActiveTrafficStop { get; set; } = null;
        internal static LHandle ActivePursuit { get; set; } = null;
        internal static bool ALPRDisabledForCallout { get; set; } = false;

        internal static Ped PlayerPed {  get { return Game.LocalPlayer.Character; } }
        internal static Vehicle PlayerVehicle {  get { return Globals.PlayerPed.CurrentVehicle; } }

        internal static bool CurrentlyCollectingWorldVehicles { get; set; } = false;
        internal static bool CurrentlyCollectingClosestWorldVehicles { get; set; } = false;
        internal static bool CurrentlyProcessingClosestWorldVehicles { get; set; } = false;
        internal static List<Vehicle> WorldVehicles { get; set; } = new List<Vehicle>();
        internal static List<Vehicle> ClosestWorldVehicles { get; set; } = new List<Vehicle>();
    }
}
