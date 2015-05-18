package com.mhise.xml;

import org.w3c.dom.Attr;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class XMLCommon {
	
	String request =null;
	Document doc;
	Element rootElement;
	public static  String  TAG_TOPIC= "topic";
	public static  String  TAG_SUBTOPIC= "subtopic";
	public static  String  TAG_ROOT ="ims-event";
	public static  String  TAG_PRIORITY="priority";
	public static  String  TAG_PARAMS ="params";

	
	//Make request 
	public XMLHolder addRoot(Document doc,String ROOTTAG) {		 
			 //Add root element 
			 Element root = doc.createElement(ROOTTAG);
			 doc.appendChild(root); 
			 XMLHolder xhd = new XMLHolder();
			 xhd.doc = doc;
			 xhd.el=root;
			 return xhd;
	}
	
	// added on 23 oct 2012
	public XMLHolder addRoot(Document doc, String tagName, String attributeName, 
			String attrValue, String textNode)
	{		 
		 //Add root element 
		 Element root = doc.createElement(tagName);
		 Attr attr = doc.createAttribute(attributeName);
		 attr.setValue(attrValue);
		 root.setAttributeNode(attr);
		 
		 doc.appendChild(root);
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el=root;
		 return xhd;
	}
	
	// added on 23 oct 2012
	public XMLHolder addAttribute(Document doc, Element element , String attributeName, 
			String attrValue )
	{		 
			
		 Attr attr = doc.createAttribute(attributeName);
		 attr.setValue(attrValue);
		 element.setAttributeNode(attr);
		 
		 
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el=element;
		 return xhd;
	} 
	
	public XMLHolder addElementToParent(Document doc,Element root,String tagName,String attributeName,String attrValue)
	{	
		Element rootpeer = doc.createElement(tagName);
		root.appendChild(rootpeer);
		// set attribute  element
		Attr attr = doc.createAttribute(attributeName);
		attr.setValue(attrValue);
		rootpeer.setAttributeNode(attr);
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el=rootpeer;
		 return xhd;
	}

	public XMLHolder addElementToParent(Document doc,Element root,String tagName,String textnode)
	{	
		Element  rootpeer = doc.createElement(tagName);
		rootpeer.appendChild(doc.createTextNode(textnode));
		root.appendChild(rootpeer);
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el = rootpeer;
		 return xhd;
	}
	
	public XMLHolder addElementToParent(Document doc,Element root,String tagName)
	{	
		Element  rootpeer = doc.createElement(tagName);
		root.appendChild(rootpeer);
		
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el=rootpeer;
		 return xhd;
	}
	
	public XMLHolder addElementToParent(Document doc,Element root,String tagName,String attributeName,String attrValue,String textNode)
	{	
		Element rootpeer = doc.createElement(tagName);
		rootpeer.appendChild(doc.createTextNode(textNode));
		root.appendChild(rootpeer);
		// set attribute  element
		Attr attr = doc.createAttribute(attributeName);
		attr.setValue(attrValue);
		rootpeer.setAttributeNode(attr);
		 XMLHolder xhd = new XMLHolder();
		 xhd.doc = doc;
		 xhd.el=rootpeer;
		 return xhd;
	}
		
}
