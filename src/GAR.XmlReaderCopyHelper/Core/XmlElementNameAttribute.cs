namespace GAR.XmlReaderCopyHelper.Core;

[AttributeUsage(AttributeTargets.Class)]
public class XmlElementNameAttribute(string elementName) : Attribute
{
    public string ElementName { get; } = elementName;
}
