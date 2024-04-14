namespace GAR.Services.ReaderApi.Attributes;

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ZipEntryNameAttribute(string zipEntryName) : Attribute
{
    public string ZipEntryName { get; } = zipEntryName;
}