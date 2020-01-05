using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(MessageService.MessageService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Press <Enter> to terminate the service.");
                    Console.WriteLine("\n");
                    Console.WriteLine(" Configuration Name: " + host.Description.ConfigurationName);
                    foreach (var endpoint in host.Description.Endpoints)
                    {
                        Console.WriteLine(" End point address: " + endpoint.Address);
                        Console.WriteLine(" End point binding: " + endpoint.Binding?.Name);
                        Console.WriteLine(" End point contract: " + endpoint.Contract.ConfigurationName);
                        Console.WriteLine(" End point name: " + endpoint.Name);
                        Console.WriteLine(" End point lisent uri: " + endpoint.ListenUri);
                    }
                    Console.WriteLine(" \nName:" + host.Description.Name);
                    Console.WriteLine(" Namespace: " + host.Description.Namespace);
                    Console.WriteLine(" Service Type: " + host.Description.ServiceType);
                    Console.WriteLine("\n");
                    Console.ReadLine();
                    host.Close();
                }
                catch (CommunicationException e)
                {
                    Console.WriteLine(" :( An exception occurred: {0}", e.Message);
                    host.Abort();
                }
            }
        }
    }
}
