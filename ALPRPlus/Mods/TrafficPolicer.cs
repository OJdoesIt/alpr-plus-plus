using Rage;
using Stealth.Plugins.ALPRPlus.Common;
using Stealth.Plugins.ALPRPlus.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic_Policer;
using Traffic_Policer.API;

namespace Stealth.Plugins.ALPRPlus.Mods
{
    internal static class TrafficPolicer
    {
        internal static EVehicleStatus GetVehicleRegistrationStatus(Vehicle veh)
        {
            if (veh.Exists())
            {
                return FromTrafficPolicer(Functions.GetVehicleRegistrationStatus(veh));
            }
            else
            {
                return EVehicleStatus.Valid;
            }
        }

        internal static EVehicleStatus GetVehicleInsuranceStatus(Vehicle veh)
        {
            if (veh.Exists())
            {
                return FromTrafficPolicer(Functions.GetVehicleInsuranceStatus(veh));
            }
            else
            {
                return EVehicleStatus.Valid;
            }
        }

        internal static void SetVehicleRegistrationStatus(Vehicle veh, EVehicleStatus RegistrationStatus)
        {
            if (veh.Exists())
            {
                Logger.LogVerbose("Setting vehicle registration status...");
                Functions.SetVehicleRegistrationStatus(veh, ToTrafficPolicer(RegistrationStatus));
            }
        }

        internal static void SetVehicleInsuranceStatus(Vehicle veh, EVehicleStatus InsuranceStatus)
        {
            if (veh.Exists())
            {
                Logger.LogVerbose("Setting vehicle insurance status...");
                Functions.SetVehicleInsuranceStatus(veh, ToTrafficPolicer(InsuranceStatus));
            }
        }

        private static EVehicleStatus FromTrafficPolicer(EVehicleDetailsStatus status)
        {
            if (status == EVehicleDetailsStatus.Expired)
                return EVehicleStatus.Expired;
            else if (status == EVehicleDetailsStatus.None)
                return EVehicleStatus.None;
            else
                return EVehicleStatus.Valid;
        }

        private static EVehicleDetailsStatus ToTrafficPolicer(EVehicleStatus status)
        {
            if (status == EVehicleStatus.Expired)
                return EVehicleDetailsStatus.Expired;
            else if (status == EVehicleStatus.None)
                return EVehicleDetailsStatus.None;
            else
                return EVehicleDetailsStatus.Valid;
        }
    }
}
