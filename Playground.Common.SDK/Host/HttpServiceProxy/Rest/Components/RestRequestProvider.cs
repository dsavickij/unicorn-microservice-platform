using Playground.Common.SDK.Abstractions.Http.MethodAttributes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.Host.HttpServiceProxy.Rest.Components;

public interface IRestRequestProvider
{
    public IRestRequest GetRestRequest(MethodInfo httpServiceMethod, IList<object> methodArguments);
}

internal class RestRequestProvider : IRestRequestProvider
{
    public IRestRequest GetRestRequest(MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        var methodType = GetHttpMethodType(httpServiceMethod);
        var pathTemplate = GetHttpMethodPathTemplate(httpServiceMethod);

        var parameters = httpServiceMethod.GetParameters();

        var urlSegmentParams = GetUrlSegmentParameters(pathTemplate, parameters);
        var queryParams = parameters.Except(urlSegmentParams);

        var req = new RestRequest(pathTemplate, methodType);

        if (methodArguments != null)
        {
            //url segment params
            foreach (var p in urlSegmentParams)
            {
                if (methodArguments[p.Position] is string s && string.IsNullOrEmpty(s))
                {
                    var arg = "%02%03";
                    req.AddUrlSegment(p.Name ?? string.Empty, arg);
                }
                else req.AddUrlSegment(p.Name ?? string.Empty, methodArguments[p.Position]);
            }

            //query params
            foreach (var p in queryParams)
            {
                if (methodArguments[p.Position].ToString() is string s)
                {
                    req.AddQueryParameter(p.Name ?? string.Empty, s);
                }
                else throw new Exception();
            }
        }

        return req;
    }

    IEnumerable<ParameterInfo> GetUrlSegmentParameters(string pathTemplate, IEnumerable<ParameterInfo> parameters)
    {
        var urlSegmentParams = new List<ParameterInfo>();

        foreach (var p in parameters)
        {
            if (pathTemplate.Contains($"{{{p.Name ?? string.Empty}}}"))
                urlSegmentParams.Add(p);
        }

        return urlSegmentParams;
    }

    private string GetHttpMethodPathTemplate(MethodInfo method)
    {
        var type = method.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType.IsAssignableTo(typeof(PlaygroundHttpAttribute)))?.AttributeType;

        if (method.GetCustomAttribute(type) is PlaygroundHttpAttribute attribute)
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