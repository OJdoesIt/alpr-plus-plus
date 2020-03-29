using LSPD_First_Response.Engine.Scripting.Entities;
using Rage;
using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using Stealth.Plugins.ALPRPlus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.API
{
    /// <summary>
    /// Contains data for an ALPR+ "hit"
    /// </summary>
    public sealed class ALPRScanResult
    {
        private Vehicle mVehicle;
        /// <summary>
        /// The vehicle for which the hit occurred, or should occur for
        /// </summary>
        public Vehicle Vehicle { get { return mVehicle; } }

        private string mResult;
        /// <summary>
        /// The 'flag' or alert text; This is simply the String representation of the EAlertType enum for pre-defined flags, or the custom flag passed in by another plugin
        /// </summary>
        public string Result {
            get
            {
                if (IsCustomFlag)
                    return mResult;
                else
                    return AlertType.ToFriendlyString();
            }
        }

        internal EAlertType AlertType { get; set; } = EAlertType.Null;
        internal Persona Persona { get; set; } = null;
        internal string RegisteredOwner { get; set; } = "";
        internal DateTime Created { get; set; } = DateTime.Now;
        internal DateTime LastDisplayed { get; set; } = DateTime.Now;
        internal bool IsCustomFlag { get; set; } = false;
        internal bool PersonaReapplied { get; set; } = false;

        internal ALPRScanResult()
        {
            Init();
        }

        internal ALPRScanResult(Vehicle pVehicle, string pResult)
        {
            mVehicle = pVehicle;
            mResult = pResult;
            Created = DateTime.Now;
        }

        internal ALPRScanResult(Vehicle pVehicle, EAlertType pResult)
        {
            mVehicle = pVehicle;
            AlertType = pResult;
            mResult = pResult.ToString();
            Created = DateTime.Now;
        }

        private void Init()
        {
            mVehicle = null;
            mResult = "";
            Created = DateTime.Now;
        }

        internal void SetResult(string pResult)
        {
            mResult = pResult;
        }
    }
}
