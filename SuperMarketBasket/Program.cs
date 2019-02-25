using SuperMarketBasket.Client;
using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataStore;
using SuperMarketBasket.DataStore.Interfaces;
using SuperMarketBasket.ScanAccess;
using SuperMarketBasket.ScanAccess.Interfaces;
using SuperMarketBasket.Services;
using SuperMarketBasket.Services.Interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketBasket
{
    public static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arg)
        {                         
            IFakeDataStore dataStore = new FakeDataStore();
            IBasketService basketService = new BasketService(dataStore);
            IBasketClient basketClient = new BasketClient(basketService);
            IScanManager sm = new ScanManager(basketClient);

            if (arg.Length > 0)
                sm.SetupScanListener(arg[0]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(basketClient));
        }
    }
}
