using System.Text;
using System.Xml.Schema;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class XmlSchemaSetBuilderTestData
{
    public static class AddFile
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("TempFile", BuildFromTempFile, (true, false, true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null", null, new ExpectedException(typeof(ArgumentNullException), "path")),
            new InvalidCase("Whitespace", "   ", new ExpectedException(typeof(ArgumentException), "path")),
            new InvalidCase("Nonexistent", "nonexistent-schema-xyz.xsd", new ExpectedException(typeof(FileNotFoundException)))
        ];

        public sealed record ValidCase(string Name, Func<XmlSchemaSet> Value, (bool containsDocument, bool containsOther, bool isCompiled) Expected)
            : ReturnCase<Func<XmlSchemaSet>, (bool containsDocument, bool containsOther, bool isCompiled)>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        private static XmlSchemaSet BuildFromTempFile()
        {
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xsd");

            try
            {
                File.WriteAllText(path, TestSchemas.DocumentXsd);
                return new XmlSchemaSetBuilder().AddFile(path).Build();
            }
            finally
            {
                File.Delete(path);
            }
        }
    }

    public static class AddStream
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Utf8MemoryStream", BuildFromStream, (true, false, true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null", null, new ExpectedException(typeof(ArgumentNullException), "stream"))
        ];

        public sealed record ValidCase(string Name, Func<XmlSchemaSet> Value, (bool containsDocument, bool containsOther, bool isCompiled) Expected)
            : ReturnCase<Func<XmlSchemaSet>, (bool containsDocument, bool containsOther, bool isCompiled)>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Stream? Value, ExpectedException ExpectedException)
            : ThrowsCase<Stream?>(Name, Value, ExpectedException);

        private static XmlSchemaSet BuildFromStream()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(TestSchemas.DocumentXsd));
            return new XmlSchemaSetBuilder().AddStream(stream).Build();
        }
    }

    public static class AddReader
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("StringReader", BuildFromReader, (true, false, true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null", null, new ExpectedException(typeof(ArgumentNullException), "reader"))
        ];

        public sealed record ValidCase(string Name, Func<XmlSchemaSet> Value, (bool containsDocument, bool containsOther, bool isCompiled) Expected)
            : ReturnCase<Func<XmlSchemaSet>, (bool containsDocument, bool containsOther, bool isCompiled)>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, TextReader? Value, ExpectedException ExpectedException)
            : ThrowsCase<TextReader?>(Name, Value, ExpectedException);

        private static XmlSchemaSet BuildFromReader()
        {
            using var reader = new StringReader(TestSchemas.DocumentXsd);
            return new XmlSchemaSetBuilder().AddReader(reader).Build();
        }
    }

    public static class AddText
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("SingleSource", BuildSingleSource, (true, false, true)),
            new("TwoSources", BuildTwoSources, (true, true, true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null", null, new ExpectedException(typeof(ArgumentNullException), "schemaXml")),
            new InvalidCase("Whitespace", "   ", new ExpectedException(typeof(ArgumentException), "schemaXml")),
            new InvalidCase("InvalidSchemaText", "<not-a-schema/>", new ExpectedException(typeof(XmlSchemaException)))
        ];

        public sealed record ValidCase(string Name, Func<XmlSchemaSet> Value, (bool containsDocument, bool containsOther, bool isCompiled) Expected)
            : ReturnCase<Func<XmlSchemaSet>, (bool containsDocument, bool containsOther, bool isCompiled)>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        private static XmlSchemaSet BuildSingleSource() =>
            new XmlSchemaSetBuilder().AddText(TestSchemas.DocumentXsd).Build();

        private static XmlSchemaSet BuildTwoSources() =>
            new XmlSchemaSetBuilder().AddText(TestSchemas.DocumentXsd).AddText(TestSchemas.OtherXsd).Build();
    }

    public static class Build
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("BuildTwice", BuildTwice, new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase("NothingAdded", BuildNothingAdded, new ExpectedException(typeof(InvalidOperationException)))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        private static void BuildTwice()
        {
            var builder = new XmlSchemaSetBuilder().AddText(TestSchemas.DocumentXsd);
            builder.Build();
            builder.Build();
        }

        private static void BuildNothingAdded() => new XmlSchemaSetBuilder().Build();
    }
}
