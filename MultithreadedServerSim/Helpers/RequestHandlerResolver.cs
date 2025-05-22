using Autofac.Features.Metadata;
using MultithreadedServerSim.Attributes;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;
using MultithreadedServerSim.RequestHandlers;
using System.ComponentModel;
using System.Dynamic;

namespace MultithreadedServerSim.Helpers;

internal class RequestHandlerResolver(IEnumerable<Meta<IRequestHandler>> requestHandlers) : IRequestHandlerResolver
{
    private static readonly Dictionary<string, Type> _typeDict = new()
    {
        ["int"] = typeof(int),
        ["string"] = typeof(string),
    };

    public Func<Action?, Response> Resolve(Request request, CancellationToken cancellationToken = default)
    {
        var splitPath = request.Path.Split("/");
        var candidateHandlers = requestHandlers.Where(a => (string)a.Metadata[nameof(Controller)]! == splitPath[0]).Where(a => ((string[])a.Metadata[nameof(Route)]!).Length == splitPath.Length - 1);

        var details = new ExpandoObject() as IDictionary<string, object>;
        details.Add("path", request.Path);
        details.Add("body", request.Body);

        Meta<IRequestHandler>? handler = null;
        if (candidateHandlers.Any())
        {
            if (splitPath.Length == 1) handler = candidateHandlers.First();
            else
            {
                handler = candidateHandlers.FirstOrDefault(a =>
                {
                    var candidatePath = ((string[])a.Metadata[nameof(Route)]!);
                    for (var i = 0; i < candidatePath.Length; i++)
                    {
                        if (splitPath[i + 1] != candidatePath[i] && !candidatePath[i].StartsWith('{'))
                            return false;
                    }
                    for (var i = 0; i < candidatePath.Length; i++)
                    {
                        if (candidatePath[i].StartsWith('{'))
                        {
                            var trimmed = candidatePath[i].Trim('{').Trim('}').Split(":");

                            var targetType = _typeDict.GetValueOrDefault(trimmed.First(), typeof(string));
                            var converter = TypeDescriptor.GetConverter(targetType);

                            details.Add(trimmed.Last(), converter.ConvertFrom(splitPath[i + 1])!);
                        }
                    }
                    return true;
                });
            }
        }

        return (Action? a) =>
        {
            a?.Invoke();
            return (handler?.Value ?? new DefaultRequestHandler()).HandleRequest(details);
        };
    }
}