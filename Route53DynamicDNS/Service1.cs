using System.ServiceProcess;
using System.Threading;

namespace Route53DynamicDNS
{
    public partial class Service1 : ServiceBase
    {
        private Thread thread;
        private ManualResetEvent shutdownEvent; //Used to signal background thread to shut down

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceConfig.Load(); //Loads in the config.json file

            shutdownEvent = new ManualResetEvent(false);

            thread = new Thread(WorkerThreadFunc)
            {
                Name = "AWSDynDNSWorker",
                IsBackground = true
            };
            thread.Start();
        }

        protected override void OnStop()
        {
            shutdownEvent.Set();
            if (!thread.Join(120 * 1000)) //Allowing time for an INSYNC to complete, if needed
            {
                thread.Abort();
            }
        }

        private void WorkerThreadFunc()
        {
            while (!shutdownEvent.WaitOne(0))
            {
                Route53RecordUpdater.UpdateRecord();
                Thread.Sleep(ServiceConfig.DnyDNSPollTime);
            }
        }
    }
}
