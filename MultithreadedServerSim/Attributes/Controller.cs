namespace MultithreadedServerSim.Attributes;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true)]
internal class Controller(string path) : Attribute
{
    public string Path { get; } = path;
}
