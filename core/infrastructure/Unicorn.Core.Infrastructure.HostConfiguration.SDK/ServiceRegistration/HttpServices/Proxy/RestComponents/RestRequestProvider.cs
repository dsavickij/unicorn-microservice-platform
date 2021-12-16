using System.Reflection;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

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
        AddQueryStringParametersToRequestIfNeeded(request, httpServiceMethod, methodArguments);

        AddJsonBodyToRequestIfNeeded(request, httpServiceMethod, methodArguments);
        AddFileIfNeeded(request, httpServiceMethod, methodArguments);

        return request;
    }

    private void AddFileIfNeeded(IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        if (GetHttpMethodType(httpServiceMethod) is Method.POST && methodArguments.Any(x => x is IFormFile))
        {
            foreach (var file in methodArguments.Where(x => x is IFormFile).Cast<IFormFile>())
            {
                request.AddFile(file.Name, file.CopyTo, file.FileName, file.Length);
            }

            request.AlwaysMultipartFormData = true;
        }
    }

    private void AddJsonBodyToRequestIfNeeded(IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        if (GetHttpMethodType(httpServiceMethod) is Method.POST or Method.PUT)
        {
            foreach (var p in GetBodyMethodParameters(httpServiceMethod))
            {
                request.AddJsonBody(methodArguments[p.Position]);
            }
        }
    }

    private IEnumerable<ParameterInfo> GetBodyMethodParameters(MethodInfo httpServiceMethod)
    {
        return httpServiceMethod.GetParameters()
            .Except(GetUrlSegmentMethodParameters(httpServiceMethod))
            .Except(GetQueryStringMethodParameters(httpServiceMethod))
            .Where(x => x.CustomAttributes.Any(ca => ca.AttributeType == typeof(UnicornFromBodyAttribute)));
    }

    // http://example.com?clientId=123 - clientId=123 is query parameter
    private void AddQueryStringParametersToRequestIfNeeded(IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        foreach (var p in GetQueryStringMethodParameters(httpServiceMethod))
        {
            if (methodArguments[p.Position].ToString() is string s)
            {
                request.AddParameter(p.Name!, s, ParameterType.QueryString);
            }
            else
            {
                throw new ArgumentException($"Argument of type '{methodArguments[p.Position].GetType().FullName}'" +
                    $"is not a string");
            }
        }
    }

    // http://example.com/clients/{clientId} - {clientId} is url segment
    private void AddUrlSegmentParametersToRequestIfNeeded(
        IRestRequest request, MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        var urlSegmentParameters = GetUrlSegmentParameters(httpServiceMethod, methodArguments);

        foreach (var p in urlSegmentParameters)
        {
            request.AddParameter(p);
        }
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
            if (pathTemplate.Contains($"{{{p.Name!}}}", StringComparison.InvariantCulture))
            {
                urlSegmentParams.Add(p);
            }
        }

        return urlSegmentParams;
    }

    private IEnumerable<ParameterInfo> GetQueryStringMethodParameters(MethodInfo httpServiceMethod)
    {
        return httpServiceMethod
            .GetParameters()
            .Except(GetUrlSegmentMethodParameters(httpServiceMethod))
            .Where(x => x.CustomAttributes.All(
                ca => ca.AttributeType != typeof(UnicornFromBodyAttribute)))
            .Where(x => x.ParameterType != typeof(IFormFile));
    }

    private string GetHttpMethodPathTemplate(MethodInfo method)
    {
        var type = method.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType.IsAssignableTo(typeof(UnicornHttpAttribute)))?.AttributeType;

        if (method.GetCustomAttribute(type!) is UnicornHttpAttribute attribute)
        {
            return attribute.PathTemplate;
        }

        throw new ArgumentException($"Attribute of type '{type?.FullName}' is does not inherit from attribute " +
            $"'{typeof(UnicornHttpAttribute).FullName}'");
    }

    private Method GetHttpMethodType(MethodInfo method)
    {
        var httpAttributeType = method.CustomAttributes
             .SingleOrDefault(ca => ca.AttributeType.IsAssignableTo(typeof(UnicornHttpAttribute)))?.AttributeType;

        if (httpAttributeType is not null)
        {
            return httpAttributeType switch
            {
                _ when httpAttributeType == typeof(UnicornHttpGetAttribute) => Method.GET,
                _ when httpAttributeType == typeof(UnicornHttpPostAttribute) => Method.POST,
                _ when httpAttributeType == typeof(UnicornHttpPutAttribute) => Method.PUT,
                _ when httpAttributeType == typeof(UnicornHttpDeleteAttribute) => Method.DELETE,
                _ => throw new NotSupportedException(httpAttributeType.AssemblyQualifiedName)
            };
        }

        throw new ArgumentException($"Method '{method.Name}' in interface {method.DeclaringType?.AssemblyQualifiedName} " +
            $"is not decorated with derivative from '{typeof(UnicornHttpAttribute).FullName}'");
    }
}
