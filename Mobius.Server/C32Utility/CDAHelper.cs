using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;
using Mobius.Entity;
using urn.hl7org.sdtc;

namespace C32Utility
{
    public partial class CDAHelper
    {
        #region Varibale
        private int _sectionCount = 0;
        private string _AdministrativeGenderCode = string.Empty;
        private string _MaritalStatusCode = string.Empty;
        private string _PatientDOB = string.Empty;
        private string _Title = string.Empty;
        private string _DocumentCreationDate = string.Empty;
        private string _LanguageCode = string.Empty;
        private List<PatientAddress> _PatientAddress = null;
        private POCD_MT000040StructuredBody _structuredBody = null;
        private List<POCD_MT000040Component3> _Sections = null;
        private POCD_MT000040RecordTarget _POCD_MT000040RecordTarget = null;
        private Name _PatientName = null;
        private List<int> _ServiceTime = null;
        private POCD_MT000040Author _POCD_MT000040Author = null;
        private List<Author> _Authors = null;
        #endregion Varibale


        #region constructor
        /// <summary>
        ///  
        /// </summary>
        /// <param name="XMlDocument">C32 Document XML string</param>
        public CDAHelper(string XMlDocument)
        {
            if (string.IsNullOrEmpty(XMlDocument))
            {
                this.Result = new Result();
                this.Result.SetError(ErrorCode.CDAHELPER_INVALID_DOCUMENT);
                throw new ApplicationException(this.Result.ErrorMessage);
            }

            this.DeserializeObject(XMlDocument);
            this.Result = new Result();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="XMlDocument">C32 Document byte array</param>
        public CDAHelper(byte[] C32Document)
        {
            if (C32Document.Length == 0)
            {
                this.Result = new Result();
                this.Result.SetError(ErrorCode.CDAHELPER_INVALID_DOCUMENT);
                throw new ApplicationException(this.Result.ErrorMessage);
            }

            this.DeserializeObject(Encoding.UTF8.GetString(C32Document));
            this.Result = new Result();
        }


        #endregion constructor


        #region Destructor

        ~CDAHelper()
        {
            this.Result = null;
            _structuredBody = null;
            _Sections = null;
            _POCD_MT000040RecordTarget = null;
            _PatientName = null;
            _PatientAddress = null;
            _ServiceTime = null;
            _POCD_MT000040Author = null;
            _Authors = null;
        }

        #endregion Destructor

        #region Properties

        #region Private Helper Properties
        /// <summary>
        /// Get Client Document object
        /// </summary>
        private POCD_MT000040ClinicalDocument ClinicalDocument
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Result Result
        {
            get;
            set;
        }

        /// <summary>
        /// Get POCD_MT000040Author object for Author information 
        /// </summary>
        private POCD_MT000040Author Author
        {
            get
            {
                if (_POCD_MT000040Author == null 
                    && this.ClinicalDocument.author!=null 
                    && this.ClinicalDocument.author.Length > 0)
                    _POCD_MT000040Author = this.ClinicalDocument.author[0];
                return _POCD_MT000040Author;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private POCD_MT000040RecordTarget RecordTarget
        {
            get
            {
                if (_POCD_MT000040RecordTarget == null && this.ClinicalDocument.recordTarget.Length > 0)
                {
                    _POCD_MT000040RecordTarget = this.ClinicalDocument.recordTarget[0];
                }

                return _POCD_MT000040RecordTarget;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private POCD_MT000040StructuredBody StructuredBody
        {
            get
            {
                if (_structuredBody == null && this.ClinicalDocument.component != null && this.ClinicalDocument.component.Item != null)
                {
                    if (this.ClinicalDocument.component.Item is POCD_MT000040StructuredBody)
                    {
                        _structuredBody = (POCD_MT000040StructuredBody)this.ClinicalDocument.component.Item;

                    }

                }
                return _structuredBody;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<POCD_MT000040DocumentationOf> DocumentationOf
        {
            get
            {
                return this.ClinicalDocument.documentationOf != null ? this.ClinicalDocument.documentationOf.ToList() : new List<POCD_MT000040DocumentationOf>();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private POCD_MT000040ServiceEvent ServiceEvent
        {
            get
            {
                return this.DocumentationOf.Count > 0 ? this.DocumentationOf.FirstOrDefault().serviceEvent : null;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private IVL_TS EffectiveTime
        {
            get
            {
                return this.ServiceEvent != null ? this.ServiceEvent.effectiveTime : null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private POCD_MT000040Participant1 Participant
        {
            get
            {
                if (this.ClinicalDocument.participant != null && this.ClinicalDocument.participant.Length > 0)
                    return this.ClinicalDocument.participant[0];
                else
                    return null;
            }
        }
        private CE ClinicalDocumentCode
        {
            get
            {
                return this.ClinicalDocument.code;
            }
        }
        #endregion Private Helper Properties


        /// <summary>
        /// Get of Legal Authenticator Name
        /// </summary>
        public string LegalAuthenticatorName
        {
            get
            {
                string name = "";
                if (this.ClinicalDocument.legalAuthenticator != null && this.ClinicalDocument.legalAuthenticator.assignedEntity != null
                    && this.ClinicalDocument.legalAuthenticator.assignedEntity.representedOrganization != null
                    && this.ClinicalDocument.legalAuthenticator.assignedEntity.representedOrganization.name != null
                    && this.ClinicalDocument.legalAuthenticator.assignedEntity.representedOrganization.name.Length > 0)
                {
                    name = string.Join(" ", this.ClinicalDocument.legalAuthenticator.assignedEntity.representedOrganization.name[0].Text);

                }

                return name;
            }
        }

        /// <summary>
        /// Get Patient Address
        /// </summary>
        public List<PatientAddress> PatientAddress
        {
            get
            {
                if (_PatientAddress == null)
                {
                    _PatientAddress = GetPatientAddress();
                }
                return _PatientAddress;
            }
        }


        /// <summary>
        /// Get Code of clinical document 
        /// </summary>
        public string Code
        {
            get
            {
                return this.ClinicalDocumentCode.code;
            }
        }

        /// <summary>
        /// Get code system of clinical document
        /// </summary>
        public string CodeSystem
        {
            get
            {
                return this.ClinicalDocumentCode.codeSystem;
            }
        }

        /// <summary>
        /// Get display name of code of clinical document
        /// </summary>
        public string CodeDisplayName
        {
            get
            {
                return this.ClinicalDocumentCode.displayName;
            }
        }

        /// <summary>
        /// Gte system name of clinical document
        /// </summary>
        public string CodeSystemName
        {
            get
            {
                return this.ClinicalDocumentCode.codeSystemName;
            }
        }


        /// <summary>
        /// Get Author Information like Person, Institution, Role, Specialty
        /// </summary>
        public List<Author> Authors
        {
            get
            {
                if (_Authors == null)
                {
                    _Authors = new List<C32Utility.Author>();
                    string Person = string.Empty;
                    List<string> persons = GetAuthorName();
                    List<string> authorInstitutions = GetAuthorInstitution();
                    List<string> authorRoles = GetAuthorRole();
                    List<string> authorSpecialties = GetSpecialty();
                    Author author = null;
                    /*
                    Person
                    Institution
                    Role
                    Specialty
                     */
                    var result = authorInstitutions.SelectMany(x => authorRoles.SelectMany(inner => authorSpecialties, (Role, Specialty) => 
                    { return new { Role, Specialty }; }), (Institution, t) => { return new { Person, Institution, t.Role, t.Specialty }; }).Distinct();

                    var resultNew = persons.SelectMany(x => result, (Author, t) => { return new { Author, t.Institution, t.Role, t.Specialty }; }).Distinct();


                    if (resultNew.Count() > 0)
                    {
                        foreach (var item in resultNew)
                        {
                            author = new Author();
                            author.Person = item.Author;
                            author.Institution = item.Institution;
                            author.Role = item.Role;
                            author.Specialty = item.Specialty;
                            _Authors.Add(author);
                        }
                    }

                   
                }
                return _Authors;
            }
        }


        /// <summary>
        /// Get Start time of service
        /// </summary>
        public string ServiceStartTime
        {
            get
            {
                if (_ServiceTime == null)
                {
                    GetServiceTime();
                }
                return _ServiceTime.Min().ToString();
            }
        }

        /// <summary>
        /// Get Stop time of service
        /// </summary>
        public string ServiceStopTime
        {
            get
            {
                if (_ServiceTime == null)
                {
                    GetServiceTime();
                }
                return _ServiceTime.Max().ToString();

            }


        }


        /// <summary>
        /// Get language of clinical document
        /// </summary>
        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty(_LanguageCode))
                {
                    _LanguageCode = this.ClinicalDocument.languageCode.code;
                }
                return _LanguageCode;
            }
        }


        /// <summary>
        /// Get document creation date
        /// </summary>
        public string DocumentCreationDate
        {

            get
            {
                if (string.IsNullOrEmpty(_DocumentCreationDate))
                {
                    _DocumentCreationDate = this.ClinicalDocument.effectiveTime.value;
                }

                return _DocumentCreationDate;
            }
        }

        /// <summary>
        /// Return count of sections
        /// </summary>
        public int SectionCount
        {
            get
            {
                if (_sectionCount == 0 && this.StructuredBody != null)
                {
                    _sectionCount = this.StructuredBody.component.Length;
                }
                return _sectionCount;
            }
        }

        /// <summary>
        /// Get Document Title 
        /// </summary>
        public string DocumentTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_Title) && this.ClinicalDocument.title!=null && this.ClinicalDocument.title.Text!=null )
                {
                    _Title = string.Join(" ", this.ClinicalDocument.title.Text);
                }

                return _Title;
            }
        }

        /// <summary>
        /// Return Sections
        /// </summary>
        public List<POCD_MT000040Component3> Sections
        {
            get
            {
                if (_Sections == null)
                {
                    if (this.StructuredBody.component != null && this.StructuredBody.component.Length > 0)
                    {
                        _Sections = new List<POCD_MT000040Component3>();

                        _Sections.AddRange(this.StructuredBody.component);
                    }
                }
                return _Sections;
            }
        }


        /// <summary>
        /// Get facility display name
        /// </summary>
        public string FacilityDisplayName
        {
            get
            {
                if (this.Participant != null && this.Participant.associatedEntity != null && this.Participant.associatedEntity.code != null)
                {
                    return this.Participant.associatedEntity.code.displayName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get facility type code system
        /// </summary>
        public string FacilityTypeCodeSystem
        {
            get
            {
                if (this.Participant != null && this.Participant.associatedEntity != null && this.Participant.associatedEntity.code != null)
                {
                    return this.Participant.associatedEntity.code.codeSystem;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get facility type system name
        /// </summary>
        public string FacilityTypeSystemName
        {
            get
            {
                if (this.Participant != null && this.Participant.associatedEntity != null && this.Participant.associatedEntity.code != null)
                {
                    return this.Participant.associatedEntity.code.codeSystemName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get facility type code
        /// </summary>
        public string FacilityTypeCode
        {
            get
            {
                if (this.Participant != null && this.Participant.associatedEntity != null && this.Participant.associatedEntity.code != null)
                {
                    return this.Participant.associatedEntity.code.code;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get Name of Patient
        /// </summary>
        public Name PatientName
        {
            get
            {
                if (this._PatientName == null) this.getPatientNamePart();
                return this._PatientName;
            }
        }

        /// <summary>
        /// Get date of birth of patient
        /// </summary>
        public string PatientDOB
        {
            get
            {
                if (string.IsNullOrEmpty(_PatientDOB) && this.RecordTarget.patientRole.patient.birthTime != null)
                {
                    _PatientDOB = this.RecordTarget.patientRole.patient.birthTime.value;
                }
                return _PatientDOB;
            }
        }

        /// <summary>
        /// Get marital status of Patient
        /// </summary>
        public string MaritalStatusCode
        {
            get
            {
                if (string.IsNullOrEmpty(_MaritalStatusCode) && this.RecordTarget.patientRole.patient.maritalStatusCode != null)
                {
                    _MaritalStatusCode = this.RecordTarget.patientRole.patient.maritalStatusCode.code;
                }
                return _MaritalStatusCode;
            }
        }

        /// <summary>
        /// Get gender of patient
        /// </summary>
        public string PatientGender
        {
            get
            {
                if (string.IsNullOrEmpty(_AdministrativeGenderCode) && this.RecordTarget.patientRole.patient.administrativeGenderCode != null)
                {

                    _AdministrativeGenderCode = this.RecordTarget.patientRole.patient.administrativeGenderCode.code;
                }
                return _AdministrativeGenderCode;
            }
        }

        /// <summary>
        /// Get confidentiality code
        /// </summary>
        public string ConfidentialityCode
        {
            get
            {
                return this.ClinicalDocument.confidentialityCode.code;
            }
        }

        /// <summary>
        /// get confidentiality display name
        /// </summary>
        public string ConfidentialityDisplayName
        {
            get
            {
                return this.ClinicalDocument.confidentialityCode.displayName == null ? "" : this.ClinicalDocument.confidentialityCode.displayName;
            }
        }


        /// <summary>
        /// Get confidentiality system name
        /// </summary>
        public string ConfidentialitySystemName
        {
            get
            {
                return this.ClinicalDocument.confidentialityCode.codeSystemName == null ? "" : this.ClinicalDocument.confidentialityCode.codeSystemName;
            }
        }

        /// <summary>
        /// Get confidentiality system code
        /// </summary>
        public string ConfidentialitySystemCode
        {
            get
            {
                return this.ClinicalDocument.confidentialityCode.codeSystem == null ? "" : this.ClinicalDocument.confidentialityCode.codeSystem;
            }
        }

        #endregion Properties


    }
}
