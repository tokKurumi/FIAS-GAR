namespace XmlSerializationBenchmark;

using System.Xml.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using global::XmlSerializationBenchmark.Models;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByMethod)]
public class XmlSerializationBenchmarks
{
    private readonly string _xmlFilePath = "test_file.xml";

    [Benchmark]
    public void Deserialization__XmlSerializer()
    {
        var serializer = new XmlSerializer(typeof(ADDRESSOBJECTS));
        var result = (ADDRESSOBJECTS)serializer.Deserialize(File.OpenRead(_xmlFilePath));
    }
}
