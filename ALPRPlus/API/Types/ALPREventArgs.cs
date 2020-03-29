using Stealth.Plugins.ALPRPlus.API.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.API.Types
{
    public struct ALPREventArgs
    {
        internal ALPREventArgs(ALPRScanResult res, ECamera cam)
        {
            mResult = res;
            mCamera = cam;
        }

        private ALPRScanResult mResult;
        public ALPRScanResult Result { get { return mResult; } }

        private ECamera mCamera;
        public ECamera Camera { get { return mCamera; } }
    }
}
