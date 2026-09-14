using System.Xml.Schema;
using PineGuard.Xml;

namespace PineGuard.Xml.UnitTests.Schemas;

/// <summary>
/// Shared inline XSD schemas and sample documents used across every <c>PineGuard.Xml.UnitTests</c>
/// test file. Every other test file in this project consumes this class — its member names are
/// not renamed without updating every consumer.
/// </summary>
public static class TestSchemas
{
    public const string DocumentNamespace = "urn:test:doc";
    public const string OtherNamespace = "urn:test:other";

    public const string DocumentXsd = """
        <?xml version="1.0" encoding="utf-8"?>
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:tns="urn:test:doc" targetNamespace="urn:test:doc" elementFormDefault="qualified">
          <xs:element name="Document">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="GrpHdr">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="MsgId">
                        <xs:simpleType>
                          <xs:restriction base="xs:string">
                            <xs:maxLength value="35" />
                          </xs:restriction>
                        </xs:simpleType>
                      </xs:element>
                      <xs:element name="CreDtTm" type="xs:dateTime" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name="SplmtryData" minOccurs="0">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:any processContents="lax" minOccurs="0" maxOccurs="unbounded" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;

    public const string OtherXsd = """
        <?xml version="1.0" encoding="utf-8"?>
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test:other" elementFormDefault="qualified">
          <xs:element name="Other" type="xs:string" />
        </xs:schema>
        """;

    public static XmlSchemaSet DocumentSet { get; } = new XmlSchemaSetBuilder().AddText(DocumentXsd).Build();

    public static XmlSchemaSet BothSet { get; } = new XmlSchemaSetBuilder().AddText(DocumentXsd).AddText(OtherXsd).Build();

    public const string ValidMessage = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <MsgId>MSG-1</MsgId>
            <CreDtTm>2026-09-14T10:00:00Z</CreDtTm>
          </GrpHdr>
        </Document>
        """;

    public const string ValidWithSupplementary = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <MsgId>MSG-1</MsgId>
            <CreDtTm>2026-09-14T10:00:00Z</CreDtTm>
          </GrpHdr>
          <SplmtryData>
            <Extra xmlns="urn:test:other">1</Extra>
          </SplmtryData>
        </Document>
        """;

    public const string MissingMsgId = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <CreDtTm>2026-09-14T10:00:00Z</CreDtTm>
          </GrpHdr>
        </Document>
        """;

    public const string MsgIdTooLong = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <MsgId>AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA</MsgId>
            <CreDtTm>2026-09-14T10:00:00Z</CreDtTm>
          </GrpHdr>
        </Document>
        """;

    public const string BadDateTime = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <MsgId>MSG-1</MsgId>
            <CreDtTm>not-a-date</CreDtTm>
          </GrpHdr>
        </Document>
        """;

    public const string TwoViolations = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:doc">
          <GrpHdr>
            <MsgId>AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA</MsgId>
            <CreDtTm>not-a-date</CreDtTm>
          </GrpHdr>
        </Document>
        """;

    public const string UnknownNamespaceDocument = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:unknown">
          <GrpHdr />
        </Document>
        """;

    public const string NoNamespaceDocument = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document>
          <GrpHdr />
        </Document>
        """;

    public const string Malformed = """
        <Document xmlns="urn:test:doc"><GrpHdr>
        """;

    public const string NotXml = "plain text";

    public const string AttributeNamespace = "urn:test:attr";

    public const string AttributeXsd = """
        <?xml version="1.0" encoding="utf-8"?>
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:test:attr" elementFormDefault="qualified">
          <xs:element name="Document">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="MsgId" minOccurs="0">
                  <xs:complexType>
                    <xs:simpleContent>
                      <xs:extension base="xs:string">
                        <xs:attribute name="Kind" use="required" />
                      </xs:extension>
                    </xs:simpleContent>
                  </xs:complexType>
                </xs:element>
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;

    public static XmlSchemaSet AttributeSet { get; } = new XmlSchemaSetBuilder().AddText(AttributeXsd).Build();

    public const string EmptyElementValid = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:attr"><MsgId Kind="x"/></Document>
        """;

    public const string MissingRequiredAttribute = """
        <?xml version="1.0" encoding="utf-8"?>
        <Document xmlns="urn:test:attr"><MsgId>abc</MsgId></Document>
        """;
}
