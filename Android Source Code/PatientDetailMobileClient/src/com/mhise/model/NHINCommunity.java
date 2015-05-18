

package com.mhise.model;

/** 
*@(#)NHINCommunity.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to NHINCommunity
* @since 2012-08-10
* @version 1.0 
*/

@SuppressWarnings("serial")
public class NHINCommunity  implements java.io.Serializable {
    private java.lang.String communityDescription;

    private java.lang.String communityIdentifier;

    private java.lang.String communityName;

    private java.lang.Boolean isHomeCommunity;

    public NHINCommunity() {
    }

    public NHINCommunity(
           java.lang.String communityDescription,
           java.lang.String communityIdentifier,
           java.lang.String communityName,
           java.lang.Boolean isHomeCommunity) {
           this.communityDescription = communityDescription;
           this.communityIdentifier = communityIdentifier;
           this.communityName = communityName;
           this.isHomeCommunity = isHomeCommunity;
    }


    /**
     * Gets the communityDescription value for this NHINCommunity.
     * 
     * @return communityDescription
     */
    public java.lang.String getCommunityDescription() {
        return communityDescription;
    }


    /**
     * Sets the communityDescription value for this NHINCommunity.
     * 
     * @param communityDescription
     */
    public void setCommunityDescription(java.lang.String communityDescription) {
        this.communityDescription = communityDescription;
    }


    /**
     * Gets the communityIdentifier value for this NHINCommunity.
     * 
     * @return communityIdentifier
     */
    public java.lang.String getCommunityIdentifier() {
        return communityIdentifier;
    }


    /**
     * Sets the communityIdentifier value for this NHINCommunity.
     * 
     * @param communityIdentifier
     */
    public void setCommunityIdentifier(java.lang.String communityIdentifier) {
        this.communityIdentifier = communityIdentifier;
    }


    /**
     * Gets the communityName value for this NHINCommunity.
     * 
     * @return communityName
     */
    public java.lang.String getCommunityName() {
        return communityName;
    }


    /**
     * Sets the communityName value for this NHINCommunity.
     * 
     * @param communityName
     */
    public void setCommunityName(java.lang.String communityName) {
        this.communityName = communityName;
    }


    /**
     * Gets the isHomeCommunity value for this NHINCommunity.
     * 
     * @return isHomeCommunity
     */
    public java.lang.Boolean getIsHomeCommunity() {
        return isHomeCommunity;
    }


    /**
     * Sets the isHomeCommunity value for this NHINCommunity.
     * 
     * @param isHomeCommunity
     */
    public void setIsHomeCommunity(java.lang.Boolean isHomeCommunity) {
        this.isHomeCommunity = isHomeCommunity;
    }



   
}
