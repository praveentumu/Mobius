package gov.nist.hitsp.validation;

import java.io.InputStream;
import java.io.PrintStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Iterator;
import java.util.List;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class DocumentType
{
  private String id = null;
  private String displayName = null;
  private String description = null;
  private int validationType = 0;
  private String validationLocation = null;
  private List<Dependency> dependencies = null;
  private List<String> dependencyNames = null;
  private boolean backendOnly = false;
  public static final int SCHEMA_VALIDATION = 1;
  public static final int SCHEMATRON_VALIDATION = 2;

  public DocumentType()
  {
  }

  public DocumentType(Element docTypeElement)
  {
    setId(docTypeElement.getAttribute("id"));
    if (docTypeElement.getAttribute("backendOnly") != null) {
      setBackendOnly(Boolean.parseBoolean(docTypeElement.getAttribute("backendOnly")));
    }
    NodeList docChildren = docTypeElement.getChildNodes();
    for (int i = 0; i < docChildren.getLength(); i++) {
      Node child = docChildren.item(i);
      if (!(child instanceof Element))
        continue;
      Element childElement = (Element)child;
      String childName = childElement.getNodeName();

      if ("displayName".equals(childName))
        setDisplayName(childElement.getTextContent());
      else if ("description".equals(childName))
        setDescription(childElement.getTextContent());
      else if ("validation".equals(childName))
        setValidation(childElement);
      else if ("dependencies".equals(childName))
        setDependencies(childElement);
    }
  }

  public String getId()
  {
    return this.id;
  }

  public void setId(String id) {
    this.id = id;
  }

  public String getDisplayName() {
    return this.displayName;
  }

  public void setDisplayName(String displayName) {
    this.displayName = displayName;
  }

  public int getValidationType() {
    return this.validationType;
  }

  public void setValidationType(int validationType) {
    this.validationType = validationType;
  }

  public String getValidationLocation() {
    return this.validationLocation;
  }

  public void setValidationLocation(String validationLocation) {
    this.validationLocation = validationLocation;
  }

  public List<Dependency> getDependencies() {
    if (this.dependencies == null)
      this.dependencies = new ArrayList();
    return this.dependencies;
  }

  public void setDependencies(List<Dependency> dependencies) {
    this.dependencies = dependencies;
  }

  public String getDescription() {
    return this.description;
  }

  public void setDescription(String description) {
    this.description = description;
  }

  public void setValidation(Element validation) {
    setValidationLocation(validation.getTextContent());
    String type = validation.getAttribute("type");
    if ("schema".equals(type))
      setValidationType(1);
    else
      setValidationType(2);
  }

  public void setDependencies(Element dependenciesElement) {
    this.dependencies = new ArrayList();

    NodeList children = dependenciesElement.getElementsByTagName("dependency");
    for (int i = 0; i < children.getLength(); i++) {
      Node child = children.item(i);
      if ((child instanceof Element)) {
        Element childElement = (Element)child;
        Dependency dep = new Dependency(childElement);
        this.dependencies.add(dep);
      }
    }

    Collections.sort(this.dependencies, new DependencyComparator());
  }

  public List<String> getDependencyNames(Collection docTypes) {
    if (this.dependencyNames == null) {
      this.dependencyNames = new ArrayList();
      List dependencies = getDependencies();
      for (int i = 0; i < dependencies.size(); i++) {
        Dependency dependency = (Dependency)dependencies.get(i);
        String id = dependency.getDocumentType();
        DocumentType docType = findDocumentType(id, docTypes);
        this.dependencyNames.add(docType.getDisplayName());
      }
    }
    return this.dependencyNames;
  }

  public List<String> getDependencyDescription(Collection docTypes) {
    ArrayList dependencyDescr = new ArrayList();
    List dependencies = getDependencies();
    for (int i = 0; i < dependencies.size(); i++) {
      Dependency dependency = (Dependency)dependencies.get(i);
      String id = dependency.getDocumentType();
      DocumentType docType = findDocumentType(id, docTypes);
      dependencyDescr.add(docType.getDescription());
    }
    return dependencyDescr;
  }

  public List<String> getDependencyId(Collection docTypes) {
    ArrayList dependencyId = new ArrayList();
    List dependencies = getDependencies();
    for (int i = 0; i < dependencies.size(); i++) {
      Dependency dependency = (Dependency)dependencies.get(i);
      String id = dependency.getDocumentType();
      DocumentType docType = findDocumentType(id, docTypes);
      dependencyId.add(docType.getId());
    }
    return dependencyId;
  }

  public ValidationResults doFullValidation(InputStream is, Document doc, Collection docTypes, Collection<String> phases, boolean htmlFormatted, String skeletonLocation)
  {
      
      
    List dependencies = getDependencies();
    ValidationResults results = new ValidationResults();
    //results.addSchematronErrors("<WSValidationResults>");
    for (int i = 0; i < dependencies.size(); i++) {
      Dependency dependency = (Dependency)dependencies.get(i);
      String id = dependency.getDocumentType();
      System.out.println("Dependency: " + id);
      DocumentType docType = findDocumentType(id, docTypes);
      System.out.print("Display name*********************"+docType.getDisplayName());
      
      ValidationResults individualResults = docType.doSelfValidation(is, doc, phases, htmlFormatted, skeletonLocation);
      results.append(individualResults);
    }
    ValidationResults currentResults = doSelfValidation(is, doc, phases, htmlFormatted, skeletonLocation);
    results.append(currentResults);
    //results.addSchematronErrors("</WSValidationResults>");
    return results;
  }

  public ValidationResults doSelfValidation(InputStream is, Document doc, Collection<String> phases, boolean htmlFormatted, String skeletonLocation)
  {
    ValidationResults results = new ValidationResults();
    if (getValidationType() == 1) {
      results.setSchemaErrors(XmlValidation.validateWithSchema(is, getValidationLocation()));
      results.setSchemaName(getDisplayName());
    } else if (htmlFormatted) {
      results.addSchematronErrors(getDisplayName()+XmlValidation.validateWithSchematron(doc, getValidationLocation(), skeletonLocation, phases, htmlFormatted));
      
    } else {
      results.addSchematronErrors(XmlValidation.validateWithSchematron(doc, getValidationLocation(), skeletonLocation, phases, htmlFormatted));
    }
   
    return results;
  }

  public static List<DocumentType> parseDocumentTypes(Document document) {
    NodeList documentTypes = document.getElementsByTagName("documentType");
    List allDocTypes = new ArrayList();
    for (int i = 0; i < documentTypes.getLength(); i++) {
      Node docType = documentTypes.item(i);
      if ((docType instanceof Element)) {
        Element docTypeElement = (Element)docType;
        allDocTypes.add(new DocumentType(docTypeElement));
      }
    }

    return allDocTypes;
  }

  public static DocumentType findDocumentType(String id, Collection docTypes)
  {
    Iterator it = docTypes.iterator();
    while (it.hasNext()) {
      Object docObj = it.next();

      if ((docObj instanceof DocumentType)) {
        DocumentType docType = (DocumentType)docObj;
        if (docType.getId().equals(id))
          return docType;
      }
    }
    return null;
  }

  public static List<DocumentType> filterRunnableOnly(List<DocumentType> docTypes) {
    List runnable = new ArrayList();
    Iterator it = docTypes.iterator();
    while (it.hasNext()) {
      DocumentType docType = (DocumentType)it.next();
      if (!docType.isBackendOnly())
        runnable.add(docType);
    }
    return runnable;
  }

  public boolean isBackendOnly() {
    return this.backendOnly;
  }

  public void setBackendOnly(boolean backendOnly) {
    this.backendOnly = backendOnly;
  }
}