using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace QWService
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
            QWService objService = new QWService();
            objService.ProcessData();
 
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new QWService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
