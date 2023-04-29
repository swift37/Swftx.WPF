namespace Swftx.Wpf.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
internal sealed class DependencyOnAttribute : Attribute
{
    /// <summary>
    /// The name of the property on which the specified property depends
    /// </summary>
    public string? ParamName { get; set; }

    public DependencyOnAttribute() { }

    public DependencyOnAttribute(string? paramName) => ParamName = paramName;
}
