using SuperMarketBasket.ScanAccess;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace Scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Scan();
        }

        private static void Scan()
        {

            Process pipeClient = new Process();
            pipeClient.StartInfo.FileName = "SuperMarketBasket.exe";

            // Pass the client process a handle to the server.

            Console.WriteLine("1. Apple");
            Console.WriteLine("2. Biscuits");
            Console.WriteLine("3. Coffee");
            Console.WriteLine("4. Tissues");
            Console.WriteLine("5. Pear");

            using (AnonymousPipeServerStream pipeServer =
            new AnonymousPipeServerStream(PipeDirection.Out,
            HandleInheritability.Inheritable))
            {
                pipeClient.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                pipeClient.StartInfo.UseShellExecute = false;
                pipeClient.Start();

                pipeServer.DisposeLocalCopyOfClientHandle();

                try
                {
                    // Read user input and send that to the client process.
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        //bool breakLoop = false;
                        while(true)
                        {
                            Console.WriteLine("type number and enter:");

                            string line = Console.ReadLine();
                            string sendingString = String.Empty;

                            int i = 0;
                            if (int.TryParse(line, out i))
                            {
                                switch (i)
                                {
                                    case 1:
                                        sendToBasketApp(pipeServer, sw, "A99");
                                        break;
                                    case 2:
                                        sendToBasketApp(pipeServer, sw, "B15");
                                        break;
                                    case 3:
                                        sendToBasketApp(pipeServer, sw, "C40");
                                        break;
                                    case 4:
                                        sendToBasketApp(pipeServer, sw, "T23");
                                        break;
                                    case 5:
                                        sendToBasketApp(pipeServer, sw, "P43");
                                        break;
                                    default:
                                        Console.WriteLine("not a valid number");
                                        break;
                                }

                            }
                            else
                            {
                                Console.WriteLine("not a valid number");
                            }
                        }
                        
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("[SERVER] Error: {0}", e.Message);
                }

            }
           
        }

        private static void sendToBasketApp(AnonymousPipeServerStream pipeServer, StreamWriter sw, string sendingString)
        {
            // Send a 'sync message' and wait for client to receive it.
            sw.WriteLine("SYNC");
            pipeServer.WaitForPipeDrain();
            // Send the console input to the client process.
            sw.WriteLine(sendingString);
        }
    }
}