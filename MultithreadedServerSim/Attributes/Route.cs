namespace MultithreadedServerSim.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class Route : Attribute
{
    public Route(params string[] path)
    {
        if (path.Any(segment => segment.StartsWith('{') && !segment.Contains(':')))
            throw new Exception($"The path variable does not contain a type definition");
        Path = path;
    }

    public string[] Path { get; }
}
