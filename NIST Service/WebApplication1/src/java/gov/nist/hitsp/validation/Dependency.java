package gov.nist.hitsp.validation;

import org.w3c.dom.Element;

public class Dependency
{
  private int sequenceNumber = 0;
  private String documentType = null;

  public Dependency(Element dependency)
  {
    setSequenceNumber(Integer.parseInt(dependency.getAttribute("sequenceNumber")));
    setDocumentType(dependency.getTextContent());
  }

  public int getSequenceNumber() {
    return this.sequenceNumber;
  }

  public void setSequenceNumber(int sequenceNumber) {
    this.sequenceNumber = sequenceNumber;
  }

  public String getDocumentType() {
    return this.documentType;
  }

  public void setDocumentType(String documentType) {
    this.documentType = documentType;
  }
}