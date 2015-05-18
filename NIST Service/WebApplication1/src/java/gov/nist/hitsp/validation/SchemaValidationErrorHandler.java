package gov.nist.hitsp.validation;

import java.util.Iterator;
import java.util.Vector;
import org.xml.sax.ErrorHandler;
import org.xml.sax.SAXException;
import org.xml.sax.SAXParseException;

public class SchemaValidationErrorHandler
  implements ErrorHandler
{
  private Vector<String> warnings = null;
  private Vector<String> errors = null;
  private Vector<String> fatalErrors = null;

  public void warning(SAXParseException exception)
    throws SAXException
  {
    addWarning(exception.getMessage());
  }

  public void fatalError(SAXParseException exception) throws SAXException {
    addFatalError(exception.getMessage());
  }

  public void error(SAXParseException exception) throws SAXException {
    addError(exception.getMessage());
  }

  public boolean hasWarnings() {
    if (getWarnings() == null) {
      return false;
    }
    return !getWarnings().isEmpty();
  }

  public boolean hasErrors()
  {
    if (getErrors() == null) {
      return false;
    }
    return !getErrors().isEmpty();
  }

  public boolean hasFatalErrors()
  {
    if (getFatalErrors() == null) {
      return false;
    }
    return !getFatalErrors().isEmpty();
  }

  public String getPrintableWarnings()
  {
    if (getWarnings() == null)
      return "";
    Iterator it = getWarnings().iterator();
    StringBuffer sb = new StringBuffer();
    while (it.hasNext()) {
      sb.append("Warning: " + (String)it.next() + "\n");
    }
    return sb.toString();
  }

  public String getPrintableErrors() {
    if (getErrors() == null)
      return "";
    Iterator it = getErrors().iterator();
    StringBuffer sb = new StringBuffer();
    while (it.hasNext()) {
      sb.append("Error: " + (String)it.next() + "\n");
    }
    return sb.toString();
  }

  public String getPrintableFatalErrors() {
    if (getFatalErrors() == null)
      return "";
    Iterator it = getFatalErrors().iterator();
    StringBuffer sb = new StringBuffer();
    while (it.hasNext()) {
      sb.append("Fatal Error: " + (String)it.next() + "\n");
    }
    return sb.toString();
  }

  public boolean addWarning(String warning)
  {
    if (getWarnings() == null)
      setWarnings(new Vector());
    return getWarnings().add(warning);
  }

  public boolean addError(String error) {
    if (getErrors() == null)
      setErrors(new Vector());
    return getErrors().add(error);
  }

  public boolean addFatalError(String fatalError) {
    if (getFatalErrors() == null)
      setFatalErrors(new Vector());
    return getFatalErrors().add(fatalError);
  }

  public Vector<String> getWarnings() {
    return this.warnings;
  }

  public void setWarnings(Vector<String> warnings) {
    this.warnings = warnings;
  }

  public Vector<String> getErrors() {
    return this.errors;
  }

  public void setErrors(Vector<String> errors) {
    this.errors = errors;
  }

  public Vector<String> getFatalErrors() {
    return this.fatalErrors;
  }

  public void setFatalErrors(Vector<String> fatalErrors) {
    this.fatalErrors = fatalErrors;
  }
}