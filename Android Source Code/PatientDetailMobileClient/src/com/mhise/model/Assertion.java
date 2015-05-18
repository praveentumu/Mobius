package com.mhise.model;

public class Assertion {
	private String addressLine1;
	private String addressLine2;
	private String addressStatus;
	private City city;
	private String id;
	private String zip;
	private String assertionMode;
	private String explanationNonClaimantSignature;
	private boolean haveSignature;
	private NHINCommunity homeCommunity;
	private String messageId;
	private String patientId;
	private PersonName personName;
	private String purposeOfUse;
	private String[] relatesToList;
	private String ssn;
	private User userInformation;
	private boolean authorized;
	private Telephone phoneNumber;
	
	public String getAddressLine1() {
		return addressLine1;
	}
	public void setAddressLine1(String addressLine1) {
		this.addressLine1 = addressLine1;
	}
	public String getAddressLine2() {
		return addressLine2;
	}
	public void setAddressLine2(String addressLine2) {
		this.addressLine2 = addressLine2;
	}
	public String getAddressStatus() {
		return addressStatus;
	}
	public void setAddressStatus(String addressStatus) {
		this.addressStatus = addressStatus;
	}
	public City getCity() {
		return city;
	}
	public void setCity(City city) {
		this.city = city;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getZip() {
		return zip;
	}
	public void setZip(String zip) {
		this.zip = zip;
	}
	public String getAssertionMode() {
		return assertionMode;
	}
	public void setAssertionMode(String assertionMode) {
		this.assertionMode = assertionMode;
	}
	public String getExplanationNonClaimantSignature() {
		return explanationNonClaimantSignature;
	}
	public void setExplanationNonClaimantSignature(
			String explanationNonClaimantSignature) {
		this.explanationNonClaimantSignature = explanationNonClaimantSignature;
	}
	public boolean isHaveSignature() {
		return haveSignature;
	}
	public void setHaveSignature(boolean haveSignature) {
		this.haveSignature = haveSignature;
	}
	public NHINCommunity getNhinCommunity() {
		return homeCommunity;
	}
	public void setNhinCommunity(NHINCommunity nhinCommunity) {
		this.homeCommunity = nhinCommunity;
	}
	public String getMessageId() {
		return messageId;
	}
	public void setMessageId(String messageId) {
		this.messageId = messageId;
	}
	public String getPatientId() {
		return patientId;
	}
	public void setPatientId(String patientId) {
		this.patientId = patientId;
	}
	public PersonName getPersonName() {
		return personName;
	}
	public void setPersonName(PersonName personName) {
		this.personName = personName;
	}
	public String getPurposeOfUse() {
		return purposeOfUse;
	}
	public void setPurposeOfUse(String purposeOfUse) {
		this.purposeOfUse = purposeOfUse;
	}
	public String[] getRelatesToList() {
		return relatesToList;
	}
	public void setRelatesToList(String[] relatesToList) {
		this.relatesToList = relatesToList;
	}
	public String getSsn() {
		return ssn;
	}
	public void setSsn(String ssn) {
		this.ssn = ssn;
	}
	public User getUserInformation() {
		return userInformation;
	}
	public void setUserInformation(User userInformation) {
		this.userInformation = userInformation;
	}
	public boolean isAuthorized() {
		return authorized;
	}
	public void setAuthorized(boolean authorized) {
		this.authorized = authorized;
	}
	public Telephone getPhoneNumber() {
		return phoneNumber;
	}
	public void setPhoneNumber(Telephone phoneNumber) {
		this.phoneNumber = phoneNumber;
	}
}
