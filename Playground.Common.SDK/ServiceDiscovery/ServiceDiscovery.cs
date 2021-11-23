//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Playground.Common.SDK.ServiceDiscovery;

//public class ServiceDiscovery
//{

//    public ServiceConfiguration ServiceDiscoveryConfiguration { get; set; }
//}

//public class ServiceConfiguration
//{
//    public IEnumerable<HttpServiceConfiguration> HttpServices { get; set; } = Enumerable.Empty<HttpServiceConfiguration>() ;
//    public IEnumerable<GrpcServiceConfiguration> GrpcServices { get; set; } = Enumerable.Empty<GrpcServiceConfiguration>();
//}

//public class GrpcServiceConfiguration
//{
//    public string Name { get; set; } = "";
//    public string BaseUrl { get; set; } = "";
//    public int Port { get; set; }
//}

//public class HttpServiceConfiguration
//{
//    public string Name { get; set; } = "";
//    public string BaseUrl { get; set; } = "";
//}
