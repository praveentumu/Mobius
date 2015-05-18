package gov.nist.hitsp.validation;

import gov.nist.hl7.v3.xml.util.MiscXmlUtil;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMResult;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;
import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.xml.sax.SAXException;

public class XmlValidation
{
  public static SchemaValidationErrorHandler validateWithSchema(InputStream xml, String schemaLocation)
  {
    DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
    factory.setNamespaceAware(true);
    factory.setValidating(true);
    factory.setAttribute("http://java.sun.com/xml/jaxp/properties/schemaLanguage", "http://www.w3.org/2001/XMLSchema");

    factory.setAttribute("http://java.sun.com/xml/jaxp/properties/schemaSource", schemaLocation);

    factory.setIgnoringElementContentWhitespace(true);
    DocumentBuilder builder = null;Document doc=null;
    try {
      builder = factory.newDocumentBuilder();
    } catch (ParserConfigurationException pce) {
      pce.printStackTrace();
      return null;
    }
    SchemaValidationErrorHandler handler = new SchemaValidationErrorHandler();
    builder.setErrorHandler(handler);
    try
    {
      doc = builder.parse(xml);
    }
    catch (SAXException e)
    {
      
      System.out.println("Message is not valid XML.");
      handler.addError("Message is not valid XML.");
      e.printStackTrace();
    } catch (IOException e) {
      System.out.println("Message is not valid XML.  Possible empty message.");
      handler.addError("Message is not valid XML.  Possible empty message.");
      e.printStackTrace();
    }

    return handler;
  }

  public static String validateWithSchematron(Document xml, String schematronLocation, String skeletonLocation, Collection<String> phases, boolean htmlFormatted)
  {
    if (phases == null)
      phases = emptyPhases();
    try
    {
      StringBuilder result = new StringBuilder();

    //  if (!htmlFormatted)
       // result.append("<result>");
      Iterator it = phases.iterator();
      while (it.hasNext())
      {
        System.out.println("skeletonLocation = " + skeletonLocation);
        URL skeletonUrl = new URL(skeletonLocation);

        InputStream skeleton = skeletonUrl.openStream();
        URL schematronUrl = new URL(schematronLocation);
        InputStream schematron = schematronUrl.openStream();

        String phase = (String)it.next();
        System.out.println(phase);
        Node schematronTransform = doTransform(schematron, skeleton, phase);
        if (htmlFormatted)
          result.append(doTransform(xml, schematronTransform));
        else
          result.append(MiscXmlUtil.removeXmlHeader(doTransform(xml, schematronTransform)));
      }
      if (!htmlFormatted)
       // result.append("</result>");
      return result.toString();
    }
    catch (MalformedURLException mue)
    {
      mue.printStackTrace();
      return null;
    } catch (IOException ioe) {
      ioe.printStackTrace();
    }return null;
  }

  public static Node doTransform(InputStream originalXml, InputStream transform, String phase)
  {
    System.out.println("javax.xml.transform.TransformerFactory = " + System.getProperty("javax.xml.transform.TransformerFactory"));

    System.setProperty("javax.xml.transform.TransformerFactory", "net.sf.saxon.TransformerFactoryImpl");
    DOMResult result = new DOMResult();
    try {
      Source xmlSource = new StreamSource(originalXml);
      Source xsltSource = new StreamSource(transform);

      TransformerFactory tf = TransformerFactory.newInstance();
      Transformer transformer = tf.newTransformer(xsltSource);
      transformer.setParameter("phase", phase);
      transformer.transform(xmlSource, result);
    } catch (TransformerConfigurationException tce) {
      tce.printStackTrace();
      return null;
    } catch (TransformerException te) {
      te.printStackTrace();
      return null;
    }
    System.clearProperty("javax.xml.transform.TransformerFactory");
    return result.getNode();
  }

  public static String doTransform(Document originalXml, Node transform) {
    System.out.println("javax.xml.transform.TransformerFactory = " + System.getProperty("javax.xml.transform.TransformerFactory"));

    System.setProperty("javax.xml.transform.TransformerFactory", "net.sf.saxon.TransformerFactoryImpl");
    ByteArrayOutputStream os = new ByteArrayOutputStream();

    StreamResult result = new StreamResult(os);
    try {
      Source xmlSource = new DOMSource(originalXml);
      Source xsltSource = new DOMSource(transform);

      TransformerFactory tf = TransformerFactory.newInstance();
      Transformer transformer = tf.newTransformer(xsltSource);
      transformer.transform(xmlSource, result);
    } catch (TransformerConfigurationException tce) {
      tce.printStackTrace();
      return null;
    } catch (TransformerException te) {
      te.printStackTrace();
      return null;
    }
    System.clearProperty("javax.xml.transform.TransformerFactory");

    return os.toString();
  }

  public static String doTransform(InputStream originalXml, Node transform)
  {
    System.out.println("javax.xml.transform.TransformerFactory = " + System.getProperty("javax.xml.transform.TransformerFactory"));

    System.setProperty("javax.xml.transform.TransformerFactory", "net.sf.saxon.TransformerFactoryImpl");
    ByteArrayOutputStream os = new ByteArrayOutputStream();

    StreamResult result = new StreamResult(os);
    try {
      Source xmlSource = new StreamSource(originalXml);
      Source xsltSource = new DOMSource(transform);

      TransformerFactory tf = TransformerFactory.newInstance();
      Transformer transformer = tf.newTransformer(xsltSource);
      transformer.transform(xmlSource, result);
    } catch (TransformerConfigurationException tce) {
      tce.printStackTrace();
      return null;
    } catch (TransformerException te) {
      te.printStackTrace();
      return null;
    }
    System.clearProperty("javax.xml.transform.TransformerFactory");

    return os.toString();
  }

  public static Node doTransform(InputStream originalXml, String transformLocation)
  {
    System.out.println("javax.xml.transform.TransformerFactory = " + System.getProperty("javax.xml.transform.TransformerFactory"));

    System.setProperty("javax.xml.transform.TransformerFactory", "net.sf.saxon.TransformerFactoryImpl");
    DOMResult result = new DOMResult();
    try {
      Source xmlSource = new StreamSource(originalXml);
      Source xsltSource = new StreamSource(transformLocation);

      TransformerFactory tf = TransformerFactory.newInstance();
      Transformer transformer = tf.newTransformer(xsltSource);
      transformer.transform(xmlSource, result);
    } catch (TransformerConfigurationException tce) {
      tce.printStackTrace();
      return null;
    } catch (TransformerException te) {
      te.printStackTrace();
      return null;
    }

    System.clearProperty("javax.xml.transform.TransformerFactory");
    return result.getNode();
  }

  public static void main(String[] args)
  {
  }

  public static Collection<String> emptyPhases()
  {
    Collection phases = new ArrayList();
    phases.add("#ALL");
    return phases;
  }
}