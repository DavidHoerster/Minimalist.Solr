using Agile.Minimalist.Commanding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Agile.Common;

namespace CommandHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueName = ConfigurationManager.AppSettings["queueName"];

            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(queueName))
                MessageQueue.Create(queueName, true);

            SearchFactory.Init();

            using (var host = new ServiceHost(typeof(CommandHandler)))
            {
                host.Open();

                Console.WriteLine("Service has started");
                Console.WriteLine("Press <ENTER> to exit the service");
                Console.WriteLine("");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
