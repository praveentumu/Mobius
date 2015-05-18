

package com.mhise.model;

/** 
*@(#)Demographics.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Demographics
* @since 2012-08-10
* @version 1.0 
*/
@SuppressWarnings("serial")
public class Demographics  implements java.io.Serializable {
    private java.lang.String DOB;
    private String deceasedDate;
    public String getDeceasedDate() {
		return deceasedDate;
	}

	public void setDeceasedDate(String deceasedDate) {
		this.deceasedDate = deceasedDate;
	}


	private java.lang.String familyName;

    private java.lang.String gender;

    private java.lang.String givenName;

    private java.lang.String middleName;
    private String prefix;
    private String suffix;
    private boolean advanceSearch=false;
    public boolean isAdvanceSearch() {
		return advanceSearch;
	}

	public void setAdvanceSearch(boolean advanceSearch) {
		this.advanceSearch = advanceSearch;
	}

	public String getPrefix() {
		return prefix;
	}

	public void setPrefix(String prefix) {
		this.prefix = prefix;
	}

	public String getSuffix() {
		return suffix;
	}

	public void setSuffix(String suffix) {
		this.suffix = suffix;
	}


	private java.lang.String localMPIID;

   // private java.lang.String mothersMaidenName;
    private PersonName motherMaiden;

	private java.lang.String SSN;    
    private String communityDescription;
    private String birthPlaceCity;
    private String birthPlaceCountry;
    private String birthPlaceZip;
    private String birthPlaceState;
    private String birthPlaceStreet;
    private String city;
    private String country;
    private String state;
    private String street;
    private String zip;
    private String telephone;
    
    public String getTelephone() {
		return telephone;
	}

	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	public String getCountry() {
		return country;
	}

	public void setCountry(String country) {
		this.country = country;
	}
	public String getState() {
		return state;
	}

	public void setState(String state) {
		this.state = state;
	}

	public String getStreet() {
		return street;
	}

	public void setStreet(String street) {
		this.street = street;
	}

	public String getZip() {
		return zip;
	}

	public void setZip(String zip) {
		this.zip = zip;
	}

	public String getCity() {
		return city;
	}

	public void setCity(String city) {
		this.city = city;
	}

	public String getBirthPlaceCity() {
		return birthPlaceCity;
	}

	public void setBirthPlaceCity(String birthPlaceCity) {
		this.birthPlaceCity = birthPlaceCity;
	}

	public String getBirthPlaceCountry() {
		return birthPlaceCountry;
	}

	public void setBirthPlaceCountry(String birthPlaceCountry) {
		this.birthPlaceCountry = birthPlaceCountry;
	}

	public String getBirthPlaceZip() {
		return birthPlaceZip;
	}

	public void setBirthPlaceZip(String birthPlaceZip) {
		this.birthPlaceZip = birthPlaceZip;
	}

	public String getBirthPlaceState() {
		return birthPlaceState;
	}

	public void setBirthPlaceState(String birthPlaceState) {
		this.birthPlaceState = birthPlaceState;
	}

	public String getBirthPlaceStreet() {
		return birthPlaceStreet;
	}

	public void setBirthPlaceStreet(String birthPlaceStreet) {
		this.birthPlaceStreet = birthPlaceStreet;
	}

	public String getCommunityDescription() {
		return communityDescription;
	}

	public void setCommunityDescription(String communityDescription) {
		this.communityDescription = communityDescription;
	}

	public Demographics() {
    }

    public Demographics(
           java.lang.String DOB,
           java.lang.String familyName,
           java.lang.String middleName,
           java.lang.String gender,
           java.lang.String givenName,
           java.lang.String localMPIID,
           PersonName mothersMaiden,
           java.lang.String SSN) {
           this.DOB = DOB;
           this.familyName = familyName;
           this.gender = gender;
           this.givenName = givenName;
           this.localMPIID = localMPIID;
           //this.mothersMaidenName = mothersMaidenName;
           this.motherMaiden = mothersMaiden;
           this.SSN = SSN;
    }

    
    public PersonName getMotherMaiden() {
		return motherMaiden;
	}

	public void setMotherMaiden(PersonName motherMaiden) {
		this.motherMaiden = motherMaiden;
	}

    /**
     * Gets the DOB value for this Demographics.
     * 
     * @return DOB
     */
    public java.lang.String getDOB() {
        return DOB;
    }


    /**
     * Sets the DOB value for this Demographics.
     * 
     * @param DOB
     */
    public void setDOB(java.lang.String DOB) {
        this.DOB = DOB;
    }


    /**
     * Gets the familyName value for this Demographics.
     * 
     * @return familyName
     */
    public java.lang.String getFamilyName() {
        return familyName;
    }


    /**
     * Sets the familyName value for this Demographics.
     * 
     * @param familyName
     */
    public void setFamilyName(java.lang.String familyName) {
        this.familyName = familyName;
    }


    /**
     * Gets the middleName value for this Demographics.
     * 
     * @return middleName
     */
    public java.lang.String getMiddleName() {
        return middleName;
    }


    /**
     * Sets the middleName value for this Demographics.
     * 
     * @param middleName
     */
    public void setMiddleName(java.lang.String middleName) {
        this.middleName = middleName;
    }

    
    /**
     * Gets the gender value for this Demographics.
     * 
     * @return gender
     */
    public java.lang.String getGender() {
        return gender;
    }


    /**
     * Sets the gender value for this Demographics.
     * 
     * @param gender
     */
    public void setGender(java.lang.String gender) {
        this.gender = gender;
    }


    /**
     * Gets the givenName value for this Demographics.
     * 
     * @return givenName
     */
    public java.lang.String getGivenName() {
        return givenName;
    }


    /**
     * Sets the givenName value for this Demographics.
     * 
     * @param givenName
     */
    public void setGivenName(java.lang.String givenName) {
        this.givenName = givenName;
    }


    /**
     * Gets the localMPIID value for this Demographics.
     * 
     * @return localMPIID
     */
    public java.lang.String getLocalMPIID() {
        return localMPIID;
    }


    /**
     * Sets the localMPIID value for this Demographics.
     * 
     * @param localMPIID
     */
    public void setLocalMPIID(java.lang.String localMPIID) {
        this.localMPIID = localMPIID;
    }


    /**
     * Gets the mothersMaidenName value for this Demographics.
     * 
     * @return mothersMaidenName
     */
   /* public java.lang.String getMothersMaidenName() {
        return mothersMaidenName;
    }*/


    /**
     * Sets the mothersMaidenName value for this Demographics.
     * 
     * @param mothersMaidenName
     */
  /*  public void setMothersMaidenName(java.lang.String mothersMaidenName) {
        this.mothersMaidenName = mothersMaidenName;
    }*/


    /**
     * Gets the SSN value for this Demographics.
     * 
     * @return SSN
     */
    public java.lang.String getSSN() {
        return SSN;
    }


    /**
     * Sets the SSN value for this Demographics.
     * 
     * @param SSN
     */
    public void setSSN(java.lang.String SSN) {
        this.SSN = SSN;
    }
   


   
}
