namespace GAR.Services.ReaderApi.Core;

using System.Xml;

public class XmlReaderCopyHelper<TModel>(string name)
{
    private readonly string _name = name;
    private readonly List<(IEnumerable<string> Attributes, Action<TModel, IReadOnlyList<string>> Setter)> _mappings = [];

    public XmlReaderCopyHelper<TModel> Map(IEnumerable<string> attributeNames, Action<TModel, IReadOnlyList<string>> setter)
    {
        _mappings.Add((attributeNames, setter));
        return this;
    }

    public XmlReaderCopyHelper<TModel> Map(string attributeName, Action<TModel, string> propertyAction)
    {
        _mappings.Add(([attributeName], (model, values) => propertyAction(model, values.First())));

        return this;
    }

    public async IAsyncEnumerable<TModel> GetAsync(XmlReader reader)
    {
        while (await reader.ReadAsync())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == _name)
            {
                var item = ReadItem(reader);
                if (item is not null)
                {
                    yield return item;
                }
            }
        }
    }

    private TModel ReadItem(XmlReader reader)
    {
        var item = Activator.CreateInstance<TModel>();

        foreach (var (attributes, setter) in _mappings)
        {
            var values = attributes
                .Select(reader.GetAttribute)
                .Where(val => val is not null);

            setter(item, values.ToList()!);
        }

        return item;
    }
}
