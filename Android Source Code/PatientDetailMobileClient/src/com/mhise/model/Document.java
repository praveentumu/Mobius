
package com.mhise.model;

/** 
*@(#)Document.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Document
* @since 2012-08-10
* @version 1.0 
*/
@SuppressWarnings("serial")
public class Document  implements java.io.Serializable {
    private java.lang.String author;

    private java.lang.String createdOn;

    private java.lang.String dataSource;

    private byte[] documentBytes;

    private java.lang.String documentTitle;

    private java.lang.Integer documentType;

    private java.lang.String documentUniqueId;

    private java.lang.String documnetTargetID;

    private java.lang.Boolean isShared;

    private java.lang.String location;

    private java.lang.Boolean reposed;

    private java.lang.String repositoryUniqueId;

    private java.lang.String sourceCommunityId;

    private java.lang.String sourcePatientId;

    private java.lang.String uploadedBy;

    private java.lang.String XACMLDocumentId;

    public Document() {
    }

    public Document(
           java.lang.String author,
           java.lang.String createdOn,
           java.lang.String dataSource,
           byte[] documentBytes,
           java.lang.String documentTitle,
           java.lang.Integer documentType,
           java.lang.String documentUniqueId,
           java.lang.String documnetTargetID,
           java.lang.Boolean isShared,
           java.lang.String location,
           java.lang.Boolean reposed,
           java.lang.String repositoryUniqueId,
           java.lang.String sourceCommunityId,
           java.lang.String sourcePatientId,
           java.lang.String uploadedBy,
           java.lang.String XACMLDocumentId) {
           this.author = author;
           this.createdOn = createdOn;
           this.dataSource = dataSource;
           this.documentBytes = documentBytes;
           this.documentTitle = documentTitle;
           this.documentType = documentType;
           this.documentUniqueId = documentUniqueId;
           this.documnetTargetID = documnetTargetID;
           this.isShared = isShared;
           this.location = location;
           this.reposed = reposed;
           this.repositoryUniqueId = repositoryUniqueId;
           this.sourceCommunityId = sourceCommunityId;
           this.sourcePatientId = sourcePatientId;
           this.uploadedBy = uploadedBy;
           this.XACMLDocumentId = XACMLDocumentId;
    }


    /**
     * Gets the author value for this Document.
     * 
     * @return author
     */
    public java.lang.String getAuthor() {
        return author;
    }


    /**
     * Sets the author value for this Document.
     * 
     * @param author
     */
    public void setAuthor(java.lang.String author) {
        this.author = author;
    }


    /**
     * Gets the createdOn value for this Document.
     * 
     * @return createdOn
     */
    public java.lang.String getCreatedOn() {
        return createdOn;
    }


    /**
     * Sets the createdOn value for this Document.
     * 
     * @param createdOn
     */
    public void setCreatedOn(java.lang.String createdOn) {
        this.createdOn = createdOn;
    }


    /**
     * Gets the dataSource value for this Document.
     * 
     * @return dataSource
     */
    public java.lang.String getDataSource() {
        return dataSource;
    }


    /**
     * Sets the dataSource value for this Document.
     * 
     * @param dataSource
     */
    public void setDataSource(java.lang.String dataSource) {
        this.dataSource = dataSource;
    }


    /**
     * Gets the documentBytes value for this Document.
     * 
     * @return documentBytes
     */
    public byte[] getDocumentBytes() {
        return documentBytes;
    }


    /**
     * Sets the documentBytes value for this Document.
     * 
     * @param documentBytes
     */
    public void setDocumentBytes(byte[] documentBytes) {
        this.documentBytes = documentBytes;
    }


    /**
     * Gets the documentTitle value for this Document.
     * 
     * @return documentTitle
     */
    public java.lang.String getDocumentTitle() {
        return documentTitle;
    }


    /**
     * Sets the documentTitle value for this Document.
     * 
     * @param documentTitle
     */
    public void setDocumentTitle(java.lang.String documentTitle) {
        this.documentTitle = documentTitle;
    }


    /**
     * Gets the documentType value for this Document.
     * 
     * @return documentType
     */
    public java.lang.Integer getDocumentType() {
        return documentType;
    }


    /**
     * Sets the documentType value for this Document.
     * 
     * @param documentType
     */
    public void setDocumentType(java.lang.Integer documentType) {
        this.documentType = documentType;
    }


    /**
     * Gets the documentUniqueId value for this Document.
     * 
     * @return documentUniqueId
     */
    public java.lang.String getDocumentUniqueId() {
        return documentUniqueId;
    }


    /**
     * Sets the documentUniqueId value for this Document.
     * 
     * @param documentUniqueId
     */
    public void setDocumentUniqueId(java.lang.String documentUniqueId) {
        this.documentUniqueId = documentUniqueId;
    }


    /**
     * Gets the documnetTargetID value for this Document.
     * 
     * @return documnetTargetID
     */
    public java.lang.String getDocumnetTargetID() {
        return documnetTargetID;
    }


    /**
     * Sets the documnetTargetID value for this Document.
     * 
     * @param documnetTargetID
     */
    public void setDocumnetTargetID(java.lang.String documnetTargetID) {
        this.documnetTargetID = documnetTargetID;
    }


    /**
     * Gets the isShared value for this Document.
     * 
     * @return isShared
     */
    public java.lang.Boolean getIsShared() {
        return isShared;
    }


    /**
     * Sets the isShared value for this Document.
     * 
     * @param isShared
     */
    public void setIsShared(java.lang.Boolean isShared) {
        this.isShared = isShared;
    }


    /**
     * Gets the location value for this Document.
     * 
     * @return location
     */
    public java.lang.String getLocation() {
        return location;
    }


    /**
     * Sets the location value for this Document.
     * 
     * @param location
     */
    public void setLocation(java.lang.String location) {
        this.location = location;
    }


    /**
     * Gets the reposed value for this Document.
     * 
     * @return reposed
     */
    public java.lang.Boolean getReposed() {
        return reposed;
    }


    /**
     * Sets the reposed value for this Document.
     * 
     * @param reposed
     */
    public void setReposed(java.lang.Boolean reposed) {
        this.reposed = reposed;
    }


    /**
     * Gets the repositoryUniqueId value for this Document.
     * 
     * @return repositoryUniqueId
     */
    public java.lang.String getRepositoryUniqueId() {
        return repositoryUniqueId;
    }


    /**
     * Sets the repositoryUniqueId value for this Document.
     * 
     * @param repositoryUniqueId
     */
    public void setRepositoryUniqueId(java.lang.String repositoryUniqueId) {
        this.repositoryUniqueId = repositoryUniqueId;
    }


    /**
     * Gets the sourceCommunityId value for this Document.
     * 
     * @return sourceCommunityId
     */
    public java.lang.String getSourceCommunityId() {
        return sourceCommunityId;
    }


    /**
     * Sets the sourceCommunityId value for this Document.
     * 
     * @param sourceCommunityId
     */
    public void setSourceCommunityId(java.lang.String sourceCommunityId) {
        this.sourceCommunityId = sourceCommunityId;
    }


    /**
     * Gets the sourcePatientId value for this Document.
     * 
     * @return sourcePatientId
     */
    public java.lang.String getSourcePatientId() {
        return sourcePatientId;
    }


    /**
     * Sets the sourcePatientId value for this Document.
     * 
     * @param sourcePatientId
     */
    public void setSourcePatientId(java.lang.String sourcePatientId) {
        this.sourcePatientId = sourcePatientId;
    }


    /**
     * Gets the uploadedBy value for this Document.
     * 
     * @return uploadedBy
     */
    public java.lang.String getUploadedBy() {
        return uploadedBy;
    }


    /**
     * Sets the uploadedBy value for this Document.
     * 
     * @param uploadedBy
     */
    public void setUploadedBy(java.lang.String uploadedBy) {
        this.uploadedBy = uploadedBy;
    }


    /**
     * Gets the XACMLDocumentId value for this Document.
     * 
     * @return XACMLDocumentId
     */
    public java.lang.String getXACMLDocumentId() {
        return XACMLDocumentId;
    }


    /**
     * Sets the XACMLDocumentId value for this Document.
     * 
     * @param XACMLDocumentId
     */
    public void setXACMLDocumentId(java.lang.String XACMLDocumentId) {
        this.XACMLDocumentId = XACMLDocumentId;
    }



  
}
