using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using IOData;

namespace AnalyzerService
{
    static class Program
    {
        //using DMAData;
        // Dma obj = new Dma();
        // obj.ReplaceMasterFileNames();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
           AnalyzerService objService = new AnalyzerService();
            objService.Stk();

               ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AnalyzerService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
