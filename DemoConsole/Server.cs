using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dodo.Bus;
using durable_functions.DataAccess;
using durable_functions.Framework.BusAbstractions;

namespace DemoConsole
{
    public class MyApp
    {
        public async Task Run()
        {
            var busRepo = new EfBusRepository();
            var runSince = new DateTime(2019, 01, 01);
            while (true)
            {
                try
                {
                    await busRepo.ReceiveProcessAsync(runSince, CancellationToken.None);

                    await Task.Delay(10);
                }
                catch (DataException exception)
                {
                    Console.WriteLine($"Exception occured {exception}");
                }
            }
        }
    }
    
    /// <summary>
    /// Server is responsible for running client processes
    ///
    /// on app start
    ///     load all existing processes
    ///     run existing processes pipelines
    ///     listen to process trigger events
    /// 
    /// on app shutdown
    ///     explicitly tell that worker is terminated
    ///
    /// on process trigger event
    ///     create new process
    ///     run new process pipeline
    ///
    /// running pipeline
    ///     load checkpoint version
    ///     while current version less than checkpoint version
    ///         load all events and play, discarding output
    ///     while ! server terminated and ! process terminated
    ///         load all events and play, producing output
    /// 
    /// </summary>
    public class Server
    {
        public Server(IBusRepository busRepository, ITimeService timeService)
        {
            
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var bus = new EfBusRepository();
        }
    }
}
