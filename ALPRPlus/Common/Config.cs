using Rage;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stealth.Plugins.ALPRPlus.Common
{
    internal static class Config
    {
        private const string mFileName = "ALPRPlus.ini";
        private const string mFilePath = @"Plugins\LSPDFR\" + mFileName;
        private static InitializationFile mCfgFile = new InitializationFile(mFilePath);

        //internal static string BetaKey { get; set; }

        internal static Keys ToggleKey { get; set; }
        internal static Keys ToggleKeyModifier { get; set; }
        internal static bool PlayAlertSound { get; set; } = true;
        internal static bool PlayScanSound { get; set; } = true;
        internal static bool AutoDisableOnTrafficStops { get; set; } = true;
        internal static bool AutoDisableOnPursuits { get; set; } = true;

        private const int cDriverFrontAngle = 30;
        private const int cDriverRearAngle = 150;
        private const int cPassengerRearAngle = 210;
        private const int cPassengerFrontAngle = 330;

        private const int cCameraDegreesFOV = 50;
        private const float cCameraRange = 20f;
        private const float cCameraMinRange = 2f;

        internal static int CameraDegreesFOV { get; set; } = cCameraDegreesFOV;
        internal static float CameraRange { get; set; } = cCameraRange;
        internal static float CameraMinimum { get; set; } = cCameraMinRange;
        internal static int DriverFrontAngle { get; set; } = cDriverFrontAngle;
        internal static int DriverRearAngle { get; set; } = cDriverRearAngle;
        internal static int PassengerRearAngle { get; set; } = cPassengerRearAngle;
        internal static int PassengerFrontAngle { get; set; } = cPassengerFrontAngle;
        internal static int VehicleRescanBuffer { get; set; } = cVehicleRescanBuffer;
        internal static int VehicleRescanBufferInMilliseconds { get { return VehicleRescanBuffer * 1000; } }

        private const int cSecondsBetweenAlerts = 120;
        private const int cProbabilityOfAlerts = 8;
        private const int cDefaultStolenVehicleWeight = 10;
        private const int cDefaultOwnerWantedWeight = 10;
        private const int cDefaultOwnerLicenseSuspendedWeight = 15;
        private const int cDefaultOwnerLicenseExpiredWeight = 15;
        private const int cDefaultUnregisteredVehicleWeight = 10;
        private const int cDefaultRegisrationExpiredWeight = 15;
        private const int cDefaultNoInsuranceWeight = 10;
        private const int cDefaultInsuranceExpiredWeight = 15;
        private const int cVehicleRescanBuffer = 30;

        internal static int SecondsBetweenAlerts { get; set; } = cSecondsBetweenAlerts;
        internal static int ProbabilityOfAlerts { get; set; } = cProbabilityOfAlerts;
        internal static int StolenVehicleWeight { get; set; } = cDefaultStolenVehicleWeight;
        internal static int OwnerWantedWeight { get; set; } = cDefaultOwnerWantedWeight;
        internal static int OwnerLicenseSuspendedWeight { get; set; } = cDefaultOwnerLicenseSuspendedWeight;
        internal static int OwnerLicenseExpiredWeight { get; set; } = cDefaultOwnerLicenseExpiredWeight;
        internal static int UnregisteredVehicleWeight { get; set; } = cDefaultUnregisteredVehicleWeight;
        internal static int RegisrationExpiredWeight { get; set; } = cDefaultRegisrationExpiredWeight;
        internal static int NoInsuranceWeight { get; set; } = cDefaultNoInsuranceWeight;
        internal static int InsuranceExpiredWeight { get; set; } = cDefaultInsuranceExpiredWeight;

        internal static void Init()
        {
            if (!mCfgFile.Exists())
            {
                Logger.LogTrivial("Config file does not exist; creating...");
                CreateCfg();
            }

            ReadCfg();
        }

        private static void CreateCfg()
        {
            mCfgFile.Create();

            Logger.LogTrivial("Filling config file with default settings...");
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKey.ToString(), Keys.F8.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKeyModifier.ToString(), Keys.None.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.PlayAlertSound.ToString(), true.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.PlayScanSound.ToString(), true.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.AutoDisableOnTrafficStops.ToString(), true.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.AutoDisableOnPursuits.ToString(), true.ToString());
            //mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.BetaKey.ToString(), "YourBetaKeyHere");

            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.CameraDegreesFOV.ToString(), cCameraDegreesFOV);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.CameraRange.ToString(), cCameraRange);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.CameraMinimum.ToString(), cCameraMinRange);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.DriverFrontAngle.ToString(), cDriverFrontAngle);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.DriverRearAngle.ToString(), cDriverRearAngle);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.PassengerRearAngle.ToString(), cPassengerRearAngle);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.PassengerFrontAngle.ToString(), cPassengerFrontAngle);
            mCfgFile.Write(ECfgSections.CAMERAS.ToString(), ECameras.VehicleRescanBuffer.ToString(), cVehicleRescanBuffer);

            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.SecondsBetweenAlerts.ToString(), cSecondsBetweenAlerts);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.ProbabilityOfAlerts.ToString(), cProbabilityOfAlerts);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.StolenVehicleWeight.ToString(), cDefaultStolenVehicleWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.OwnerWantedWeight.ToString(), cDefaultOwnerWantedWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.OwnerLicenseSuspendedWeight.ToString(), cDefaultOwnerLicenseSuspendedWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.OwnerLicenseExpiredWeight.ToString(), cDefaultOwnerLicenseExpiredWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.UnregisteredVehicleWeight.ToString(), cDefaultUnregisteredVehicleWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.RegisrationExpiredWeight.ToString(), cDefaultRegisrationExpiredWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.NoInsuranceWeight.ToString(), cDefaultNoInsuranceWeight);
            mCfgFile.Write(ECfgSections.ALERTS.ToString(), EAlerts.InsuranceExpiredWeight.ToString(), cDefaultInsuranceExpiredWeight);
        }

        private static void ReadCfg()
        {
            Logger.LogTrivial("Reading settings from config file...");

            ToggleKey = mCfgFile.ReadEnum<Keys>(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKey.ToString(), Keys.F8);
            ToggleKeyModifier = mCfgFile.ReadEnum<Keys>(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKeyModifier.ToString(), Keys.None);
            PlayAlertSound = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.PlayAlertSound.ToString(), true);
            PlayScanSound = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.PlayScanSound.ToString(), true);
            AutoDisableOnTrafficStops = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.AutoDisableOnTrafficStops.ToString(), true);
            AutoDisableOnPursuits = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.AutoDisableOnPursuits.ToString(), true);
            //BetaKey = mCfgFile.ReadString(ECfgSections.SETTINGS.ToString(), ESettings.BetaKey.ToString(), "YourBetaKeyHere");

            Logger.LogTrivial("ToggleKey = " + ToggleKey.ToString());

            CameraDegreesFOV = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.CameraDegreesFOV.ToString(), cCameraDegreesFOV);
            CameraRange = (float)mCfgFile.ReadDouble(ECfgSections.CAMERAS.ToString(), ECameras.CameraRange.ToString(), cCameraRange);
            CameraMinimum = (float)mCfgFile.ReadDouble(ECfgSections.CAMERAS.ToString(), ECameras.CameraMinimum.ToString(), (float)cCameraMinRange);
            DriverFrontAngle = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.DriverFrontAngle.ToString(), cDriverFrontAngle);
            DriverRearAngle = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.DriverRearAngle.ToString(), cDriverRearAngle);
            PassengerRearAngle = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.PassengerRearAngle.ToString(), cPassengerRearAngle);
            PassengerFrontAngle = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.PassengerFrontAngle.ToString(), cPassengerFrontAngle);
            VehicleRescanBuffer = mCfgFile.ReadInt32(ECfgSections.CAMERAS.ToString(), ECameras.VehicleRescanBuffer.ToString(), cVehicleRescanBuffer);

            SecondsBetweenAlerts = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.SecondsBetweenAlerts.ToString(), cSecondsBetweenAlerts);
            ProbabilityOfAlerts = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.ProbabilityOfAlerts.ToString(), cProbabilityOfAlerts);
            StolenVehicleWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.StolenVehicleWeight.ToString(), cDefaultStolenVehicleWeight);
            OwnerWantedWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.OwnerWantedWeight.ToString(), cDefaultOwnerWantedWeight);
            OwnerLicenseSuspendedWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.OwnerLicenseSuspendedWeight.ToString(), cDefaultOwnerLicenseSuspendedWeight);
            OwnerLicenseExpiredWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.OwnerLicenseExpiredWeight.ToString(), cDefaultOwnerLicenseExpiredWeight);
            UnregisteredVehicleWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.UnregisteredVehicleWeight.ToString(), cDefaultUnregisteredVehicleWeight);
            RegisrationExpiredWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.RegisrationExpiredWeight.ToString(), cDefaultRegisrationExpiredWeight);
            NoInsuranceWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.NoInsuranceWeight.ToString(), cDefaultNoInsuranceWeight);
            InsuranceExpiredWeight = mCfgFile.ReadInt32(ECfgSections.ALERTS.ToString(), EAlerts.InsuranceExpiredWeight.ToString(), cDefaultInsuranceExpiredWeight);

            AdjustAlertWeights();
        }

        private static void AdjustAlertWeights()
        {
            Dictionary<EAlertType, int> mAlertWeights = new Dictionary<EAlertType, int>() {
                {EAlertType.Stolen_Vehicle, Config.StolenVehicleWeight },
                {EAlertType.Owner_Wanted, Config.OwnerWantedWeight},
                {EAlertType.Owner_License_Suspended, Config.OwnerLicenseSuspendedWeight},
                {EAlertType.Owner_License_Expired, Config.OwnerLicenseExpiredWeight},
                {EAlertType.Unregistered_Vehicle, Config.UnregisteredVehicleWeight},
                {EAlertType.Registration_Expired, Config.RegisrationExpiredWeight},
                {EAlertType.No_Insurance, Config.NoInsuranceWeight},
                {EAlertType.Insurance_Expired, Config.InsuranceExpiredWeight}
            };
                        
            int mTotal = (from x in mAlertWeights select x.Value).Sum();

            if (mTotal != 100)
            {
                List<EAlertType> keys = (from x in mAlertWeights select x.Key).ToList();

                foreach (var x in keys)
                {
                    double mAdjustedWeight = (mAlertWeights[x] / 100);
                    int mActualWeight = Convert.ToInt32(Math.Floor(mAdjustedWeight));
                    mAlertWeights[x] = mActualWeight;
                }

                Config.StolenVehicleWeight = mAlertWeights[EAlertType.Stolen_Vehicle];
                Config.OwnerWantedWeight = mAlertWeights[EAlertType.Owner_Wanted];
                Config.OwnerLicenseSuspendedWeight = mAlertWeights[EAlertType.Owner_License_Suspended];
                Config.OwnerLicenseExpiredWeight = mAlertWeights[EAlertType.Owner_License_Expired];
                Config.UnregisteredVehicleWeight = mAlertWeights[EAlertType.Unregistered_Vehicle];
                Config.RegisrationExpiredWeight = mAlertWeights[EAlertType.Registration_Expired];
                Config.NoInsuranceWeight = mAlertWeights[EAlertType.No_Insurance];
                Config.InsuranceExpiredWeight = mAlertWeights[EAlertType.Insurance_Expired];
            }
        }

        internal static string GetToggleKeyString()
        {
            return GetKeyString(ToggleKey, ToggleKeyModifier);
        }

        private static string GetKeyString(Keys key, Keys modKey)
        {
            if (modKey == Keys.None)
            {
                return key.ToString();
            }
            else
            {
                string strmodKey = modKey.ToString();

                if (strmodKey.EndsWith("ControlKey") | strmodKey.EndsWith("ShiftKey"))
                {
                    strmodKey.Replace("Key", "");
                }

                if (strmodKey.Contains("ControlKey"))
                {
                    strmodKey = "CTRL";
                }
                else if (strmodKey.Contains("ShiftKey"))
                {
                    strmodKey = "Shift";
                }
                else if (strmodKey.Contains("Menu"))
                {
                    strmodKey = "ALT";
                }

                return string.Format("{0} + {1}", strmodKey, key.ToString());
            }
        }

        private enum ECfgSections
        {
            SETTINGS, CAMERAS, ALERTS
        }

        private enum ESettings
        {
            ToggleKey, ToggleKeyModifier, PlayAlertSound, PlayScanSound, AutoDisableOnTrafficStops, AutoDisableOnPursuits, BetaKey
        }

        private enum ECameras
        {
            CameraDegreesFOV,
            CameraRange,
            CameraMinimum,
            DriverFrontAngle,
            DriverRearAngle,
            PassengerRearAngle,
            PassengerFrontAngle,
            VehicleRescanBuffer
        }

        private enum EAlerts
        {
            SecondsBetweenAlerts,
            ProbabilityOfAlerts,
            StolenVehicleWeight,
            OwnerWantedWeight,
            OwnerLicenseSuspendedWeight,
            OwnerLicenseExpiredWeight,
            UnregisteredVehicleWeight,
            RegisrationExpiredWeight,
            NoInsuranceWeight,
            InsuranceExpiredWeight
        }
    }
}
