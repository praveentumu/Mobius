package gov.nist.hitsp.validation;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.PrintStream;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import org.w3c.dom.Document;
import org.xml.sax.SAXException;

public class Testing
{
  public static void main(String[] args)
  {
    DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
    factory.setNamespaceAware(true);
    factory.setValidating(true);
    factory.setAttribute("http://java.sun.com/xml/jaxp/properties/schemaLanguage", "http://www.w3.org/2001/XMLSchema");

    String schemaLocation = "http://localhost:8080/test/schematron.xsd";

    factory.setAttribute("http://java.sun.com/xml/jaxp/properties/schemaSource", schemaLocation);

    factory.setIgnoringElementContentWhitespace(true);
    File file = new File("/home/mccaffrey/schematron/test1.xml");
    FileInputStream fis = null;
    try {
      fis = new FileInputStream(file);
    } catch (FileNotFoundException fnfe) {
      fnfe.printStackTrace();
      System.exit(-1);
    }

    DocumentBuilder builder = null;
    try {
      builder = factory.newDocumentBuilder();
    } catch (ParserConfigurationException pce) {
      pce.printStackTrace();
      System.exit(-1);
    }
    SchemaValidationErrorHandler handler = new SchemaValidationErrorHandler();
    builder.setErrorHandler(handler);
    Document doc = null;
    try
    {
      doc = builder.parse(fis);
    } catch (SAXException e) {
      System.out.println("Message is not valid XML.");
      e.printStackTrace();
    } catch (IOException e) {
      System.out.println("Message is not valid XML.  Possible empty message.");
      e.printStackTrace();
    } catch (NullPointerException e) {
      System.out.println("Unknown Error.  Possible invalid schema location.  Verify location and check host is accepting connections.");
      e.printStackTrace();
    }
  }
}