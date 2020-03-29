using Rage;
using Stealth.Plugins.ALPRPlus.API.Types;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using Stealth.Plugins.ALPRPlus.Common;
using Stealth.Plugins.ALPRPlus.Common.Enums;
using Stealth.Plugins.ALPRPlus.Extensions;
using Stealth.Plugins.ALPRPlus.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.API
{
    /// <summary>
    /// Static class for calling ALPR+ functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Sets the ALPR flag/result for a specific vehicle. This will go in the Subtitle field of the notification, so make sure the string is not too long.
        /// You are able to use RPH color codes, such as ~b~ and ~r~.
        /// NOTE: This function will NOT set corresponding Persona or Vehicle details to match the flag you set; that part is YOUR responsibility!!
        /// </summary>
        /// <param name="veh">The Vehicle to set the flag for.</param>
        /// <param name="flag">The custom flag to be set.</param>
        /// <returns>True if the operation succeeded, or false if it failed. Errors will be logged.</returns>
        public static bool SetVehicleCustomALPRFlag(Vehicle veh, string flag)
        {
            try
            {
                if (veh.Exists())
                {
                    if (Globals.ScanResults.ContainsKey(veh))
                    {
                        ALPRScanResult r = new ALPRScanResult(veh, flag);
                        r.IsCustomFlag = true;
                        Globals.ScanResults[veh] = r;
                    }
                    else
                    {
                        ALPRScanResult r = new ALPRScanResult(veh, flag);
                        r.IsCustomFlag = true;
                        Globals.ScanResults.Add(veh, r);
                    }

                    return true;
                }
                else
                {
                    Logger.LogVerbose("Cannot set custom vehicle ALPR flag -- Vehicle is null.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(String.Format("Exception occurred in API.SetVehicleCustomALPRFlag(Vehicle veh, string flag) -- {0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Sets the ALPR flag/result for a specific vehicle. This function uses the pre-defined alert types used by ALPR+.
        /// NOTE: This function will NOT set corresponding Persona or Vehicle details to match the flag you set; that part is YOUR responsibility!!
        /// </summary>
        /// <param name="veh">The Vehicle to set the flag for.</param>
        /// <param name="flag">The pre-defined flag to be set. If this parameter is EAlertType.Null, ALPR+ will not trigger any events for the vehicle (i.e. the vehicle has a 'clear record').
        /// However, again, YOU are responsible for setting the vehicle's owner and the driver's Persona accordingly!</param>
        /// <returns>True if the operation succeeded, or false if it failed. Errors will be logged.</returns>
        public static bool SetVehiclePredefinedALPRFlag(Vehicle veh, EAlertType flag)
        {
            try
            {
                if (veh.Exists())
                {
                    if (Globals.ScanResults.ContainsKey(veh))
                    {
                        Globals.ScanResults[veh].AlertType = flag;
                    }
                    else
                    {
                        ALPRScanResult r = new ALPRScanResult(veh, flag);
                        r.IsCustomFlag = false;
                        Globals.ScanResults.Add(veh, r);
                    }

                    if (Globals.ScanResults[veh] != null)
                    {
                        if (flag == EAlertType.Null)
                        {
                            Globals.ScanResults[veh].IsCustomFlag = false;
                            Globals.ScanResults[veh].SetResult("");
                            Globals.ScanResults[veh].Persona = null;
                            Globals.ScanResults[veh].RegisteredOwner = "";
                        }
                    }

                    if (Funcs.IsTrafficPolicerRunning())
                    {
                        TrafficPolicer.SetVehicleRegistrationStatus(veh, EVehicleStatus.Valid);
                        TrafficPolicer.SetVehicleInsuranceStatus(veh, EVehicleStatus.Valid);
                    }

                    if (flag == EAlertType.Registration_Expired)
                    {
                        if (Funcs.IsTrafficPolicerRunning())
                        {
                            TrafficPolicer.SetVehicleRegistrationStatus(veh, EVehicleStatus.Expired);
                        }
                    }

                    if (flag == EAlertType.Unregistered_Vehicle)
                    {
                        if (Funcs.IsTrafficPolicerRunning())
                        {
                            TrafficPolicer.SetVehicleRegistrationStatus(veh, EVehicleStatus.None);
                        }
                    }

                    if (flag == EAlertType.No_Insurance)
                    {
                        if (Funcs.IsTrafficPolicerRunning())
                        {
                            TrafficPolicer.SetVehicleInsuranceStatus(veh, EVehicleStatus.None);
                        }
                    }

                    if (flag == EAlertType.Insurance_Expired)
                    {
                        if (Funcs.IsTrafficPolicerRunning())
                        {
                            TrafficPolicer.SetVehicleInsuranceStatus(veh, EVehicleStatus.Expired);
                        }
                    }

                    return true;
                }
                else
                {
                    Logger.LogVerbose("Cannot set vehicle ALPR flag -- Vehicle is null.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(String.Format("Exception occurred in API.SetVehiclePredefinedALPRFlag(Vehicle veh, string flag) -- {0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Clears the (pre-defined or custom) ALPR flag for a specific vehicle. This will revert ALPR+ to it's normal behavior when it detects this vehicle.
        /// NOTE: This function will NOT revert corresponding Persona or Vehicle details; that part is YOUR responsibility!!
        /// </summary>
        /// <param name="veh">The Vehicle to clear the flag for.</param>
        /// <returns>True if the operation succeeded, or false if it failed. Errors will be logged.</returns>
        public static bool ClearVehicleALPRFlag(Vehicle veh)
        {
            try
            {
                if (veh.Exists())
                {
                    if (Globals.ScanResults.ContainsKey(veh))
                        Globals.ScanResults.Remove(veh);

                    if (Funcs.IsTrafficPolicerRunning())
                    {
                        TrafficPolicer.SetVehicleRegistrationStatus(veh, EVehicleStatus.Valid);
                        TrafficPolicer.SetVehicleInsuranceStatus(veh, EVehicleStatus.Valid);
                    }

                    return true;
                }
                else
                {
                    Logger.LogVerbose("Cannot clear ALPR flag -- Vehicle is null.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(String.Format("Exception occurred in API.ClearVehicleALPRFlag(Vehicle veh) -- {0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Gets the (pre-defined or custom) ALPR flag for a specific vehicle. Pre-defined EAlertType flags will be returned as their string representation.
        /// </summary>
        /// <param name="veh">The Vehicle to get the flag for.</param>
        /// <returns>The ALPR result, or an empty string if none was found, or if the vehicle is null. Errors will be logged.</returns>
        public static string GetALPRScanResultForVehicle(Vehicle veh)
        {
            try
            {
                if (veh.Exists())
                {
                    string mScanResult = (from x in Globals.ScanResults where x.Key.Equals(veh) select x.Value.Result).FirstOrDefault();

                    if (!String.IsNullOrWhiteSpace(mScanResult))
                    {
                        return mScanResult;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    Logger.LogVerbose("Cannot get ALPR flag -- Vehicle is null.");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(String.Format("Exception occurred in API.GetALPRScanResultForVehicle(Vehicle veh) -- {0}", ex.ToString()));
                return "";
            }
        }

        /// <summary>
        /// Gets a list of all ALPR scan results, as well as API-defined results.
        /// </summary>
        /// <returns>A Generic List of ALPRScanResult objects. ALPRScanResult contains the Vehicle, and the result, represented as a string. 
        /// The data is sorted by the time the alert was last displayed (newest first).
        /// The list will be empty if no results are found. Errors will be logged.</returns>
        public static List<ALPRScanResult> GetAllALPRScanResults()
        {
            try
            {
                List<ALPRScanResult> mResults = new List<ALPRScanResult>();

                foreach (var x in Globals.ScanResults.Keys)
                {
                    mResults.Add(Globals.ScanResults[x]);
                }

                mResults = mResults.OrderByDescending(x => x.LastDisplayed).ToList();

                return mResults;
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(String.Format("Exception occurred in API.GetAllALPRScanResults() -- {0}", ex.ToString()));
                return new List<ALPRScanResult>();
            }
        }

        internal static void RaiseALPRFlagGenerated(Vehicle veh, ALPREventArgs e)
        {
            ALPREventHandler ev = ALPRFlagGenerated;
            if (ev != null)
                ev(veh, e);
        }

        internal static void RaiseALPRResultDisplayed(Vehicle veh, ALPREventArgs e)
        {
            ALPREventHandler ev = ALPRResultDisplayed;
            if (ev != null)
                ev(veh, e);
        }

        /// <summary>
        /// Raised when ALPR+ creates/generates a flag for a vehicle. Keep in mind that there is only a certain chance (15% by default) of an alert being triggered.
        /// This event is only thrown once, when the alert is actually "created" for that particular vehicle.
        /// This event is ONLY triggered when ALPR+ generates a flag, not when one is created via the API.
        /// </summary>
        public static event ALPREventHandler ALPRFlagGenerated;

        /// <summary>
        /// Raised any time ALPR+ displays an alert for a vehicle. This could be an alert generated by ALPR+, or created by another plugin via the API.
        /// This event can be triggered multiple times for a vehicle.
        /// However, once an alert is displayed for a vehicle, it is not displayed again for a certain period of time, to minimize spamming the user.
        /// By default, this "buffer" period is 30 seconds.
        /// </summary>
        public static event ALPREventHandler ALPRResultDisplayed;

        public delegate void ALPREventHandler(Vehicle veh, ALPREventArgs e);
    }
}
