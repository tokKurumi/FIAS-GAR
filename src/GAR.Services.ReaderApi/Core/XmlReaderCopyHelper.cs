namespace GAR.Services.ReaderApi.Core;

using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

public class XmlReaderCopyHelper<TModel>(
    string name)
    where TModel : new()
{
    private readonly string _name = name;
    private readonly Dictionary<string, Expression<Func<TModel, object>>> _mappings = [];

    public XmlReaderCopyHelper<TModel> Map(string attributeName, Expression<Func<TModel, object>> property)
    {
        _mappings.Add(attributeName, property);
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

    private static void SetProperty<TProperty>(TModel item, Expression<Func<TModel, TProperty>> propertyLambda, string value)
    {
        var memberExpression = propertyLambda.Body as MemberExpression;
        if (memberExpression is null)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            memberExpression = unaryExpression?.Operand as MemberExpression;
        }

        if (memberExpression is null)
        {
            throw new ArgumentException("Invalid property expression");
        }

        var property = (PropertyInfo)memberExpression.Member;
        var propType = property.PropertyType;
        var propValue = Convert.ChangeType(value, propType);

        property.SetValue(item, propValue);
    }

    private TModel ReadItem(XmlReader reader)
    {
        var item = Activator.CreateInstance<TModel>();

        foreach (var (attributeName, property) in _mappings)
        {
            var value = reader.GetAttribute(attributeName);
            ArgumentException.ThrowIfNullOrEmpty(value, attributeName);

            XmlReaderCopyHelper<TModel>.SetProperty(item, property, value);
        }

        return item;
    }
}
