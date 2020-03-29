using LSPD_First_Response.Mod.API;
using Rage;
using Stealth.Plugins.ALPRPlus.Common;
using System;
using System.Linq;
using System.Windows.Forms;
using JM.LinqFaster;

namespace Stealth.Plugins.ALPRPlus.Core
{
    internal class Driver
    {
        private ALPR mALPR = new ALPR();
        private bool mDisabledForTStop = false;
        private bool mDisabledForPursuit = false;

        internal void Launch()
        {
            while (Globals.IsPlayerOnDuty)
            {
                if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                {
                    if (Globals.PlayerVehicle.IsPoliceVehicle)
                    {
                        if (!mALPR.IsVehicleAlreadyVerified(Globals.PlayerVehicle))
                        {
                            ResetTimeAndDisplayWelcome();
                            GameFiber.Wait(1000);
                        }

                        /*if (mALPR.IsActive(Globals.PlayerVehicle) && !Functions.IsPoliceComputerActive())
                        {
                            mALPR.ScanPlates();
                        }
                        else*/ if (!mALPR.IsActive(Globals.PlayerVehicle) && !Functions.IsPoliceComputerActive())
                        {
                            TimeSpan ts = DateTime.Now - Globals.ALPRLastReadyOrActivation;

                            if (ts.TotalMinutes >= 30)
                            {
                                ResetTimeAndDisplayWelcome();
                                GameFiber.Wait(1000);
                            }
                        }
                    }
                }

                GameFiber.Yield();
            }
        }

        internal void ListenForToggleKey()
        {
            while (Globals.IsPlayerOnDuty)
            {
                if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                {
                    if (Globals.PlayerVehicle.IsPoliceVehicle)
                    {
                        if (Game.IsKeyDown(Config.ToggleKey) && !Functions.IsPoliceComputerActive())
                        {
                            if (Config.ToggleKeyModifier == Keys.None || Game.IsKeyDownRightNow(Config.ToggleKeyModifier))
                            {
                                mALPR.ToggleActive(Globals.PlayerVehicle);
                                GameFiber.Wait(500);
                            }
                        }
                    }
                }

                GameFiber.Yield();
            }
        }

        internal void MonitorActiveTrafficStop()
        {
            while (Globals.IsPlayerOnDuty)
            {
                if (Functions.IsPlayerPerformingPullover())
                {
                    //if (!mALPR.IsActive(Globals.PlayerVehicle))
                    //{
                    //    return;
                    //}

                    LHandle TStop = Functions.GetCurrentPullover();

                    if (Config.AutoDisableOnTrafficStops)
                    {
                        if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                        {
                            if (Globals.PlayerVehicle.IsPoliceVehicle && mALPR.IsActive(Globals.PlayerVehicle))
                            {
                                mALPR.SetActive(Globals.PlayerVehicle, false);
                                mDisabledForTStop = true;
                            }
                        }
                    }

                    Globals.ActiveTrafficStop = TStop;

                    if (!Globals.TrafficStops.Contains(TStop))
                    {
                        Globals.TrafficStops.Add(TStop);

                        Ped ped = Functions.GetPulloverSuspect(TStop);

                        if (ped && ped.IsInAnyVehicle(false))
                        {
                            Vehicle veh = ped.CurrentVehicle;

                            if (veh)
                            {
                                if (Globals.ScanResults.ContainsKey(veh) == true && Globals.ScanResults[veh].PersonaReapplied == false)
                                {
                                    if (Globals.ScanResults[veh].Persona != null)
                                    {
                                        Logger.LogTrivialDebug(String.Format("Traffic stop -- Setting Persona for driver (lic: {0}), (name: {1})", veh.LicensePlate, Globals.ScanResults[veh].Persona.FullName));
                                        Functions.SetPersonaForPed(ped, Globals.ScanResults[veh].Persona);
                                    }

                                    if (Globals.ScanResults[veh].RegisteredOwner != "")
                                    {
                                        Functions.SetVehicleOwnerName(veh, Globals.ScanResults[veh].RegisteredOwner);
                                    }

                                    Globals.ScanResults[veh].PersonaReapplied = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Globals.ActiveTrafficStop != null)
                    {
                        if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                        {
                            if (Globals.PlayerVehicle.IsPoliceVehicle)
                            {
                                if (Config.AutoDisableOnTrafficStops && Functions.GetCurrentPullover() == null)
                                {
                                    Globals.ActiveTrafficStop = null;

                                    if (!mALPR.IsActive(Globals.PlayerVehicle) && mDisabledForTStop)
                                    {
                                        mALPR.SetActive(Globals.PlayerVehicle, true);
                                        mDisabledForTStop = false;
                                    }
                                }
                            }
                        }
                    }
                }

                GameFiber.Yield();
            }
        }

        internal void MonitorActivePursuit()
        {
            while (Globals.IsPlayerOnDuty)
            {
                LHandle mActivePursuit = Functions.GetActivePursuit();

                if (mActivePursuit != null)
                {
                    // Active pursuit in progress

                    if (Globals.ActivePursuit != mActivePursuit)
                    {
                        // Active pursuit that we don't know about (just started?)
                        Globals.ActivePursuit = mActivePursuit;

                        //if (!mALPR.IsActive(Globals.PlayerVehicle))
                        //{
                        //    return;
                        //}

                        if (Config.AutoDisableOnPursuits)
                        {
                            if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                            {
                                if (Globals.PlayerVehicle.IsPoliceVehicle && mALPR.IsActive(Globals.PlayerVehicle))
                                {
                                    mALPR.SetActive(Globals.PlayerVehicle, false);
                                    mDisabledForPursuit = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // No active pursuit, or it ended

                    if (Globals.ActivePursuit != null)
                    {
                        if (!Globals.PlayerPed.IsOnFoot && Globals.PlayerVehicle)
                        {
                            if (Globals.PlayerVehicle.IsPoliceVehicle)
                            {
                                Globals.ActivePursuit = null;

                                if (Config.AutoDisableOnPursuits)
                                {
                                    if (!mALPR.IsActive(Globals.PlayerVehicle) && mDisabledForPursuit)
                                    {
                                        mALPR.SetActive(Globals.PlayerVehicle, true);
                                        mDisabledForPursuit = false;
                                    }
                                }
                            }
                        }
                    }
                }

                GameFiber.Yield();
            }
        }

        private void ResetTimeAndDisplayWelcome()
        {
            Globals.ALPRLastReadyOrActivation = DateTime.Now;
            Funcs.DisplayNotification("~b~System Ready", String.Format("Press ~b~{0} ~w~to ~g~activate", Config.GetToggleKeyString()));
        }

        internal void ProcessWorldVehicles()
        {
            uint dwCollectWorldVehiclesTime = 0u;
            uint dwCollectClosestWorldVehiclesTime = 0u;

            while (Globals.IsPlayerOnDuty)
            {
                GameFiber.Yield();

                if (!Globals.CurrentlyCollectingClosestWorldVehicles && dwCollectWorldVehiclesTime <= Game.GameTime)
                {
                    Globals.CurrentlyCollectingWorldVehicles = true;
                    Globals.WorldVehicles.Clear();
                    Globals.WorldVehicles = World.GetAllVehicles().ToList();
                    dwCollectWorldVehiclesTime = Game.GameTime + Constants.CollectWorldVehiclesInterval;
                    Globals.CurrentlyCollectingWorldVehicles = false;
                }
                if (!Globals.CurrentlyProcessingClosestWorldVehicles && !Globals.CurrentlyCollectingWorldVehicles && dwCollectClosestWorldVehiclesTime <= Game.GameTime)
                {
                    Globals.CurrentlyCollectingClosestWorldVehicles = true;
                    Globals.ClosestWorldVehicles.Clear();
                    Globals.ClosestWorldVehicles = World.GetAllVehicles().WhereF(x => x.DistanceTo(Globals.PlayerPed) <= 100f).OrderByF(x => x.DistanceTo(Globals.PlayerPed)).ToList();
                    dwCollectClosestWorldVehiclesTime = Game.GameTime + Constants.CollectClosestWorldVehiclesInterval;
                    Globals.CurrentlyCollectingClosestWorldVehicles = false;
                }
            }
            Globals.WorldVehicles.Clear();
            Globals.ClosestWorldVehicles.Clear();
        }
    }
}
