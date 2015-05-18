package com.mhise.xml;

import java.io.StringWriter;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;

public class XMLRequest extends XMLCommon{
	
	Document doc =null;
	static String  TAG_TOPIC= "topic";
	static String  TAG_SUBTOPIC= "subtopic";
	static String  TAG_ROOT ="ims-event";
	static String  TAG_PRIORITY="priority";
	static String 	TAG_PARAMS ="params";
	XMLHolder holder;
	
	
	
	public Document InitializeDoc() {
		DocumentBuilderFactory docFactory = DocumentBuilderFactory.newInstance();
		DocumentBuilder docBuilder;
			try {
				docBuilder = docFactory.newDocumentBuilder();
				doc = docBuilder.newDocument();
				} 
			catch (ParserConfigurationException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			return doc;	
	}
	
	public String getStringFromDocument(Document doc)
	{	
		 StringWriter sw = null;
		 TransformerFactory tf = TransformerFactory.newInstance();
		    Transformer t = null;
				try {
					t = tf.newTransformer();
					t.setOutputProperty(OutputKeys.INDENT, "yes");
				    sw = new StringWriter();
				    t.transform(new DOMSource(doc), new StreamResult(sw));
					} 
					catch (TransformerConfigurationException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					} catch (TransformerException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
		     System.out.println(sw.toString());
			return sw.toString();
	}
}


