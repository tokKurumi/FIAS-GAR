namespace GAR.XmlReaderCopyHelper.Core;

using System.Reflection;
using System.Xml;

public class XmlReaderCopyHelper<TModel>
    where TModel : new()
{
    private readonly string _name;
    private readonly Dictionary<IEnumerable<string>, Action<TModel, IReadOnlyList<string>>> _mappings = [];
    private readonly Dictionary<string, Predicate<string>> _conditions = [];

    public XmlReaderCopyHelper(string name)
        => _name = name;

    public XmlReaderCopyHelper()
    {
        var nameAttribute = typeof(TModel).GetCustomAttribute<XmlElementNameAttribute>()
            ?? throw new ArgumentException("TModel must have XmlElementNameAttribute", nameof(TModel));

        _name = nameAttribute.ElementName;
    }

    public XmlReaderCopyHelper<TModel> Map(string attributeName, Action<TModel, string> setter)
    {
        _mappings.Add([attributeName], (model, values) => setter(model, values[0]));
        return this;
    }

    public XmlReaderCopyHelper<TModel> Map(IEnumerable<string> attributeNames, Action<TModel, IReadOnlyList<string>> setter)
    {
        _mappings.Add(attributeNames, setter);
        return this;
    }

    public XmlReaderCopyHelper<TModel> WithCondition(string attributeName, Predicate<string> condition)
    {
        _conditions.Add(attributeName, condition);
        return this;
    }

    public async IAsyncEnumerable<TModel> GetAsync(XmlReader reader)
    {
        while (await reader.ReadAsync())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != _name)
            {
                continue;
            }

            var flagConditions = true;
            foreach (var (attribute, predicate) in _conditions)
            {
                var conditionAttributeValue = reader.GetAttribute(attribute);

                if (conditionAttributeValue is not null)
                {
                    flagConditions &= predicate(conditionAttributeValue);
                }

                if (!flagConditions)
                {
                    break;
                }
            }

            if (flagConditions)
            {
                var item = ReadItem(reader);
                yield return item;
            }
        }
    }

    private TModel ReadItem(XmlReader reader)
    {
        var item = new TModel();

        foreach (var (attributes, setter) in _mappings)
        {
            var values = attributes
                .Select(reader.GetAttribute)
                .Where(val => val is not null)
                .ToList();

            setter(item, values!);
        }

        return item;
    }
}
