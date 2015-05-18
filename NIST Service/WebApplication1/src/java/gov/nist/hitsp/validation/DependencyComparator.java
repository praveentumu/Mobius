package gov.nist.hitsp.validation;

import java.util.Comparator;

public class DependencyComparator
  implements Comparator<Dependency>
{
  public int compare(Dependency d1, Dependency d2)
  {
    if (d1.getSequenceNumber() == d2.getSequenceNumber()) return 0;
    if (d1.getSequenceNumber() < d2.getSequenceNumber()) return -1;

    return 1;
  }
}