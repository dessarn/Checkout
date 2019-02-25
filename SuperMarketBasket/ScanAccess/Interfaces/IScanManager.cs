using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.ScanAccess.Interfaces
{
    interface IScanManager
    {
        void SetupScanListener(string pipeHandle);
    }
}
