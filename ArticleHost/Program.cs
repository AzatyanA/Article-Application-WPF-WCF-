using ArticleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ArticleHost
{
    class Program
    {
      

        static void Main(string[] args)
        {
            string address_TCP = "net.tcp://localhost:5002/ArticleLibrary/ArticleService";

            Uri[] address_base = { new Uri(address_TCP) };
            ServiceHost service_host = new ServiceHost(typeof(ArticleLibrary.ArticleService), address_base);

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            service_host.Description.Behaviors.Add(behavior);

            ServiceDebugBehavior debug = service_host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debug == null)
            {
                service_host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                if (!debug.IncludeExceptionDetailInFaults) debug.IncludeExceptionDetailInFaults = true;
            }

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxArrayLength = int.MaxValue;
            readerQuotas.MaxBytesPerRead = int.MaxValue;
            readerQuotas.MaxNameTableCharCount = int.MaxValue;
            readerQuotas.MaxDepth = int.MaxValue;
            readerQuotas.MaxStringContentLength = int.MaxValue;

            NetTcpBinding binding_tcp = new NetTcpBinding();
            binding_tcp.MaxReceivedMessageSize = int.MaxValue;
            binding_tcp.OpenTimeout = new TimeSpan(0, 5, 0);
            binding_tcp.CloseTimeout = new TimeSpan(0, 5, 0);
            binding_tcp.SendTimeout = new TimeSpan(0, 5, 0);
            binding_tcp.ReceiveTimeout = new TimeSpan(24, 0, 0);
            binding_tcp.ReaderQuotas = readerQuotas;
            binding_tcp.Security.Mode = SecurityMode.Transport;
            binding_tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding_tcp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            binding_tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            service_host.AddServiceEndpoint(typeof(ArticleLibrary.IArticleService), binding_tcp, address_TCP);
            service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            service_host.Open();

            Console.WriteLine("Service works");
            Console.ReadLine();
            service_host.Close();
        }

    }
}
