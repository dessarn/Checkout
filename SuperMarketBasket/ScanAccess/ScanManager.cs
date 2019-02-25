using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.ScanAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketBasket.ScanAccess
{
    public class ScanManager : IScanManager
    {
        IBasketClient client;

        public ScanManager(IBasketClient client)
        {
            this.client = client;
        }

        public void SetupScanListener(string pipeHandle)
        {
            Task t = Task.Run(() =>
            {
                using (PipeStream pipeClient = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle))
                {
                    using (StreamReader sr = new StreamReader(pipeClient))
                    {
                        while(true)
                        {
                            string temp;

                            do
                            {
                                Console.WriteLine("[CLIENT] Wait for sync...");
                                temp = sr.ReadLine();
                            }
                            while (!temp.StartsWith("SYNC"));

                            while ((temp = sr.ReadLine()) != null)
                            {
                                Scan(temp);
                            }
                        }

                    }
                }
            });
        }

        private void Scan(string sku)
        {
            client.Scan(sku);
        }
    }
}
