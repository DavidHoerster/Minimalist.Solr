using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Model;
using SolrNet;

namespace Agile.Minimalist.NancyRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up Minimalist Service");
            Console.WriteLine("Initializing Solr Search Service");
            Startup.Init<Quote>("http://localhost:8983/solr/historicalQuotes");

            Console.WriteLine("Search service initialized.  Start host configuration");
            var nancyConfig = new Nancy.Hosting.Self.HostConfiguration()
            {
                UrlReservations = new Nancy.Hosting.Self.UrlReservations()
                {
                    CreateAutomatically = true,
                    User = "Everyone"
                }
            };
            Console.WriteLine("Register host URL of 50000");
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new StaticBootstrapper(), nancyConfig, new Uri("http://localhost:50000"));

            Console.WriteLine("Start Minimalist Service");
            nancyHost.Start();

            Console.WriteLine("Host is running.  Hit <ENTER> to shutdown");
            Console.ReadLine();

            Console.WriteLine("Service is shutting down");
            nancyHost.Stop();

            Console.WriteLine("Service is shut down.  Bye!");
        }
    }
}
