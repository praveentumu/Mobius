package gov.nist.hitsp.validation;

import java.util.ArrayList;
import java.util.List;

public class ValidationResults
{
  private String schemaName = null;
  private SchemaValidationErrorHandler schemaErrors = null;
  private List<String> schematronErrors = null;

  public SchemaValidationErrorHandler getSchemaErrors()
  {
    return this.schemaErrors;
  }

  public void setSchemaErrors(SchemaValidationErrorHandler schemaErrors) {
    this.schemaErrors = schemaErrors;
  }

  public List<String> getSchematronErrors() {
    if (this.schematronErrors == null)
      this.schematronErrors = new ArrayList();
    return this.schematronErrors;
  }

  public void addSchematronErrors(String schematronError)
  {
    getSchematronErrors().add(schematronError);
  }

  public void append(ValidationResults newResult)
  {
    if (newResult.getSchemaName() != null)
      setSchemaName(newResult.getSchemaName());
    if (newResult.getSchemaErrors() != null)
      setSchemaErrors(newResult.getSchemaErrors());
    if (newResult.getSchematronErrors() != null)
      getSchematronErrors().addAll(newResult.getSchematronErrors());
  }

  public String getSchemaName() {
    return this.schemaName;
  }

  public void setSchemaName(String schemaName) {
    this.schemaName = schemaName;
  }
}