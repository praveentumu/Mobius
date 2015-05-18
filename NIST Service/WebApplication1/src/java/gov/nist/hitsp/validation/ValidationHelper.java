package gov.nist.hitsp.validation;

import gov.nist.hl7.v3.xml.util.MiscXmlUtil;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.Vector;
import javax.servlet.ServletContext;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;
import javax.xml.parsers.ParserConfigurationException;
import org.apache.commons.fileupload.FileItem;
import org.apache.commons.fileupload.FileItemFactory;
import org.apache.commons.fileupload.FileUploadException;
import org.apache.commons.fileupload.disk.DiskFileItemFactory;
import org.apache.commons.fileupload.servlet.ServletFileUpload;
import org.w3c.dom.Document;
import org.xml.sax.SAXException;

public class ValidationHelper
{
  HttpServletRequest request = null;
  List docTypes = null;
  private String errorMessage = null;
  Document validDocument = null;
  ValidationResults validationResults = null;
  Collection schematronWarnings = null;
  private String documentType = null;
  private List runnableValidations = null;
  Collection resultDetail = null;
  String skeletonLocation = null;

  public void processRequest(HttpServletRequest request, boolean upload)
  {
    this.request = request;
    clearTopErrorMessage();
    this.skeletonLocation = request.getSession().getServletContext().getInitParameter("skeletonLocation");
    try {
      if (this.docTypes == null) {
        String docTypesLocation = request.getSession().getServletContext().getInitParameter("docTypesLocation");

        this.docTypes = DocumentType.parseDocumentTypes(MiscXmlUtil.fileToDom(docTypesLocation));
      }
    } catch (ParserConfigurationException ex) {
      ex.printStackTrace();
    } catch (IOException ex) {
      ex.printStackTrace();
    } catch (SAXException ex) {
      ex.printStackTrace();
    }

    if (upload)
      upload();
  }

  public boolean upload(InputStream fileStream, String resultDetail,
			String validationType, String skeletonLocation ,String docTypesLocation,
			String docType, List<DocumentType> docTypes, Document doc )
  {
	  
	  if (fileStream == null) {
	      setTopErrorMessage("Empty or no document found!");
	      return false;
	  }
	      
	      try
	      {	    
	    	  this.docTypes = docTypes;
	    	  this.skeletonLocation = skeletonLocation;
	    	  
	    	setDocumentType(docType);
	        DocumentType documentType = DocumentType.findDocumentType(getDocumentType(), this.docTypes);
	        this.validationResults = documentType.doFullValidation(fileStream, doc, this.docTypes, this.resultDetail, false, this.skeletonLocation);
	      }
	      catch (Exception e) {
	        e.printStackTrace();
	        setTopErrorMessage("Error.  Unable to parse incoming XML.  Check for valid content.  (Also check for extraneous characters at the beginning of the file.)");
	        return false;
	      }
	      return true;
	  
  }
  
  public boolean upload()
  {
    FileItemFactory factory = new DiskFileItemFactory();
    ServletFileUpload upload = new ServletFileUpload(factory);
    List items = null;
    try
    {
      items = upload.parseRequest(this.request);
    } catch (FileUploadException ex) {
      ex.printStackTrace();
      setTopErrorMessage("Error on document upload!");
      return false;
    }
    if ((items == null) || (items.size() == 0)) {
      setTopErrorMessage("No document found!");
      return false;
    }

    Iterator it = items.iterator();
    FileItem fileItem = null;
    while (it.hasNext()) {
      FileItem fit = (FileItem)it.next();
      if ("file".equals(fit.getFieldName())) {
        fileItem = fit;
      } else if ("documentType".equals(fit.getFieldName())) {
        String docTypeParameter = fit.getString();
        setDocumentType(docTypeParameter);
        System.out.println(fit.getString());
      } else if ("resultDetail".equals(fit.getFieldName())) {
        String resultDetailInput = fit.getString();
        processResultDetail(resultDetailInput);
      }
    }

    if ((fileItem == null) || (fileItem.getSize() == 0L)) {
      setTopErrorMessage("Empty or no document found!");
      return false;
    }

    InputStream is = null;
    Document doc = null;
    try
    {
      doc = MiscXmlUtil.inputStreamToDom(fileItem.getInputStream());
      DocumentType documentType = DocumentType.findDocumentType(getDocumentType(), this.docTypes);
      this.validationResults = documentType.doFullValidation(fileItem.getInputStream(), doc, this.docTypes, this.resultDetail, true, this.skeletonLocation);
    }
    catch (Exception e) {
      e.printStackTrace();
      setTopErrorMessage("Error.  Unable to parse incoming XML.  Check for valid content.  (Also check for extraneous characters at the beginning of the file.)");
      return false;
    }
    return true;
  }

  public String getSchemaName() {
    if ((this.validationResults != null) && (this.validationResults.getSchemaName() != null))
      return this.validationResults.getSchemaName();
    return "Schema";
  }

  public boolean hasSchemaErrors() {
    if ((this.validationResults == null) || (this.validationResults.getSchemaErrors() == null))
      return false;
    if (this.validationResults.getSchemaErrors().getErrors() == null) {
      return false;
    }
    return this.validationResults.getSchemaErrors().getErrors().size() != 0;
  }

  public List getSchemaErrors()
  {
    return this.validationResults.getSchemaErrors().getErrors();
  }
  public List getSchemaWarnings() {
    return this.validationResults.getSchemaErrors().getWarnings();
  }

  public boolean hasSchemaWarnings() {
    if ((this.validationResults == null) || (this.validationResults.getSchemaErrors() == null))
      return false;
    if (this.validationResults.getSchemaErrors().getWarnings() == null) {
      return false;
    }
    return this.validationResults.getSchemaErrors().getWarnings().size() != 0;
  }

  public boolean hasSchematronErrors()
  {
    if (this.validationResults.getSchematronErrors() == null) {
      return false;
    }
    return this.validationResults.getSchematronErrors().size() != 0;
  }

  public List getSchematronErrors()
  {
    return this.validationResults.getSchematronErrors();
  }

  public boolean hasTopErrorMessage()
  {
    return this.errorMessage != null;
  }

  public String getTopErrorMessage()
  {
    return this.errorMessage; } 
  public void setTopErrorMessage(String errorMessage) { this.errorMessage = errorMessage; } 
  public void clearTopErrorMessage() { this.errorMessage = null;
  }

  public String getDocumentType()
  {
    return this.documentType;
  }

  public void setDocumentType(String documentType) {
    this.documentType = documentType;
  }

  public List getRunnableValidations()
  {
    if ((this.runnableValidations == null) && (this.docTypes != null)) {
      this.runnableValidations = DocumentType.filterRunnableOnly(this.docTypes);
    }
    return this.runnableValidations;
  }
  public int getNumberOfRunnable() {
    List runnable = getRunnableValidations();
    if (runnable == null) return 0;
    return runnable.size();
  }
  public DocumentType getRunnableDocumentTypeAt(int i) {
    return (DocumentType)getRunnableValidations().get(i);
  }
  public int getNumberOfDependenciesAt(int i) {
    return getRunnableDocumentTypeAt(i).getDependencyNames(this.docTypes).size();
  }
  public String getDependenciesNameAt(int i, int j) {
    return (String)getRunnableDocumentTypeAt(i).getDependencyNames(this.docTypes).get(j);
  }
  public void processResultDetail(String resultDetailInput) {
    this.resultDetail = new ArrayList();
    System.out.println(resultDetailInput);
    if ("all".equals(this.resultDetail)) {
      this.resultDetail.add("#ALL");
    } else if ("errorsWarnings".equals(resultDetailInput)) {
      this.resultDetail.add("errors");
      this.resultDetail.add("warning");
      this.resultDetail.add("violation");
    }
    else if ("errors".equals(resultDetailInput)) {
      this.resultDetail.add("errors");
    }
    else {
      this.resultDetail.add("#ALL");
    }
  }
}