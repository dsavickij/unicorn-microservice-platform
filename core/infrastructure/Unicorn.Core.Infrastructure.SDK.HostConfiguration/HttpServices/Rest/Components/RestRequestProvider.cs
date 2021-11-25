using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;
using RestSharp;
using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest.Components;

public interface IRestRequestProvider
{
    public IRestRequest GetRestRequest(MethodInfo httpServiceMethod, IList<object> methodArguments);
}

internal class RestRequestProvider : IRestRequestProvider
{
    public IRestRequest GetRestRequest(MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        var request = GetBaseRequest(httpServiceMethod);

        AddUrlSegmentParametersToRequestIfNeeded(request, httpServiceMethod, methodArguments);
        AddQueryParametersToRequestIfNeeded(request, httpServiceMethod, methodArguments);
        AddJsonBodyToRequestIfNeeded(request, httpServiceMethod, methodArguments);
        //TODO: need to add ability to send files in request

        return request;
    }

    private void AddJsonBodyToRequestIfNeeded(IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        if (GetHttpMethodType(httpServiceMethod) is Method.POST or Method.PUT)
        {
            //TODO: check if only one body argument can be passed to endpoint and remove foreach if needed
            foreach (var p in GetNonUrlSegmentParameters(httpServiceMethod))
            {
                request.AddJsonBody(methodArguments[p.Position]);
            }
        }
    }

    private void AddQueryParametersToRequestIfNeeded(IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        if (GetHttpMethodType(httpServiceMethod) is not Method.POST or Method.PUT)
        {
            foreach (var p in GetNonUrlSegmentParameters(httpServiceMethod))
            {
                if (methodArguments[p.Position].ToString() is string s)
                {
                    request.AddQueryParameter(p.Name!, s);
                }
                else throw new Exception($"Argument of type '{methodArguments[p.Position].GetType().FullName}'" +
                    $"is not a string");
            }
        }
    }

    private IEnumerable<ParameterInfo> GetNonUrlSegmentParameters(MethodInfo httpServiceMethod)
    {
        var urlSegmentParams = GetUrlSegmentMethodParameters(httpServiceMethod);
        return httpServiceMethod.GetParameters().Except(urlSegmentParams);
    }

    private void AddUrlSegmentParametersToRequestIfNeeded(
        IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        var urlSegmentParameters = GetUrlSegmentParameters(httpServiceMethod, methodArguments);

        foreach (var p in urlSegmentParameters)
            request.AddParameter(p);
    }

    private IRestRequest GetBaseRequest(MethodInfo httpServiceMethod)
    {
        var methodType = GetHttpMethodType(httpServiceMethod);
        var pathTemplate = GetHttpMethodPathTemplate(httpServiceMethod);

        return new RestRequest(pathTemplate, methodType, DataFormat.Json);
    }

    private IEnumerable<Parameter> GetUrlSegmentParameters(MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        var urlSegmentParams = new List<Parameter>();

        foreach (var p in GetUrlSegmentMethodParameters(httpServiceMethod))
        {
            var parameter = new Parameter(p.Name!, methodArguments[p.Position], ParameterType.UrlSegment);
            urlSegmentParams.Add(parameter);
        }

        return urlSegmentParams;
    }

    private IEnumerable<ParameterInfo> GetUrlSegmentMethodParameters(MethodInfo httpServiceMethod)
    {
        var pathTemplate = GetHttpMethodPathTemplate(httpServiceMethod);
        var urlSegmentParams = new List<ParameterInfo>();

        foreach (var p in httpServiceMethod.GetParameters())
        {
            if (pathTemplate.Contains($"{{{p.Name!}}}"))
                urlSegmentParams.Add(p);
        }

        return urlSegmentParams;
    }

    private string GetHttpMethodPathTemplate(MethodInfo method)
    {
        var type = method.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType.IsAssignableTo(typeof(PlaygroundHttpAttribute)))?.AttributeType;

        if (method.GetCustomAttribute(type!) is PlaygroundHttpAttribute attribute)
            return attribute.PathTemplate;

        throw new Exception();
    }

    private Method GetHttpMethodType(MethodInfo method)
    {
        var httpAttributeType = method.CustomAttributes
             .SingleOrDefault(ca => ca.AttributeType.IsAssignableTo(typeof(PlaygroundHttpAttribute)))?.AttributeType;

        if (httpAttributeType is not null)
        {
            return httpAttributeType switch
            {
                _ when httpAttributeType == typeof(PlaygroundHttpGet) => Method.GET,
                _ when httpAttributeType == typeof(PlaygroundHttpPost) => Method.POST,
                _ when httpAttributeType == typeof(PlaygroundHttpPut) => Method.PUT,
                _ when httpAttributeType == typeof(PlaygroundHttpDelete) => Method.DELETE,
                _ => throw new NotSupportedException()
            };
        }

        throw new Exception();
    }
}