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


        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionsToRemove"></param>
        /// <param name="clinicalDocument"></param>
        /// <returns></returns>
        public Result TruncateDocument(List<string> sectionsToRemove, out string clinicalDocument)
        {
            clinicalDocument = string.Empty;
            try
            {
                POCD_MT000040StructuredBody structuredBody = this.StructuredBody;
                if (structuredBody.component != null && structuredBody.component.Length > 0)
                {
                    List<POCD_MT000040Component3> components = structuredBody.component.ToList();
                    var result = components.Where(t => sectionsToRemove.All(a => string.Join("", t.section.title.Text).ToString().ToUpper() != a.ToUpper()));
                    this.StructuredBody.component = result.ToArray();
                    clinicalDocument = XmlSerializerHelper.SerializeObject(this.ClinicalDocument, Encoding.UTF8);
                    //Re-assign the component in ClinicalDocument
                    this.StructuredBody.component = components.ToArray();
                    this.Result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        public Result GetSharedSectionDcoument(List<string> sectionsToAdd, out string clinicalDocument)
        {
            clinicalDocument = string.Empty;
            try
            {
                POCD_MT000040StructuredBody structuredBody = this.StructuredBody;
                if (structuredBody.component != null && structuredBody.component.Length > 0)
                {
                    List<POCD_MT000040Component3> components = structuredBody.component.ToList();

                    var result = components.Where(t => sectionsToAdd.Any(a => string.Join("", t.section.title.Text).ToString().ToUpper() == a.ToUpper().ToString()));
                    this.StructuredBody.component = result.ToArray();
                    clinicalDocument = XmlSerializerHelper.SerializeObject(this.ClinicalDocument, Encoding.UTF8);
                    //Re-assign the component in ClinicalDocument
                    this.StructuredBody.component = components.ToArray();
                    this.Result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionsToRemove"></param>
        /// <param name="clinicalDocumentBytes"></param>
        /// <returns></returns>
        public Result TruncateDocument(List<string> sectionsToRemove, out byte[] clinicalDocumentBytes)
        {
            clinicalDocumentBytes = null;
            try
            {

                string clinicalDocument = string.Empty;
                this.Result = TruncateDocument(sectionsToRemove, out clinicalDocument);
                if (this.Result.IsSuccess)
                {

                    clinicalDocumentBytes = Encoding.UTF8.GetBytes(clinicalDocument);
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        public Result SharedSectionDocument(List<string> SectiontoShare, out byte[] clinicalDocumentBytes)
        {
            clinicalDocumentBytes = null;
            try
            {

                string clinicalDocument = string.Empty;
                this.Result = GetSharedSectionDcoument(SectiontoShare, out clinicalDocument);
                if (this.Result.IsSuccess)
                {

                    clinicalDocumentBytes = Encoding.UTF8.GetBytes(clinicalDocument);
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }


        /// <summary>
        /// This method will return the component sections of document 
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public Result GetC32Sections(out List<string> sections)
        {
            sections = null;
            try
            {
                POCD_MT000040StructuredBody structuredBody = this.StructuredBody;
                if (structuredBody.component != null && structuredBody.component.Length > 0)
                {
                    sections = new List<string>();
                    List<POCD_MT000040Component3> components = structuredBody.component.ToList();

                    var result = from component in components
                                 select string.Join("", component.section.title.Text);

                    sections = result.ToList();
                    this.Result.IsSuccess = true;
                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CDAHelper_NO_SECTION_FOUND);
                }

            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        /// <summary>
        /// This method will check the patient consent based on sections
        /// </summary>
        /// <param name="C32Sections"></param>
        /// <returns></returns>
        public Result CheckPatientConsent(List<C32Section> C32Sections)
        {
            try
            {
                Boolean flag = false;
                string lonicCode = string.Empty;
                POCD_MT000040StructuredBody structuredBody = this.StructuredBody;

                if (structuredBody.component != null && structuredBody.component.Length > 0)
                {
                    List<POCD_MT000040Component3> components = new List<POCD_MT000040Component3>();
                    components.AddRange(structuredBody.component);
                    foreach (POCD_MT000040Component3 component in components)
                    {
                        lonicCode = string.Empty;
                        flag = false;
                        //Check Lonic code in exits in C32Modules
                        if (component.section != null && component.section.code != null)
                        {
                            lonicCode = component.section.code.code;
                        }

                        if (!string.IsNullOrEmpty(lonicCode))
                        {
                            if (C32Sections.Count(t => t.LONICCode == lonicCode && t.Allow == true) >= 1)
                            {
                                continue;
                            }

                            else
                            {
                                this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                                this.Result.IsSuccess = false;
                                return this.Result;
                            }

                        }
                        //Check for templates
                        if (!flag)
                        {
                            if (component.section.templateId != null && component.section.templateId.Length > 0)
                            {
                                if (C32Sections.Count(t => component.section.templateId.Any(a => a.root == t.TemplateId)) >= 1)
                                {

                                    continue;
                                }
                                else
                                {
                                    this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                                    this.Result.IsSuccess = false;
                                    return this.Result;
                                }
                            }
                        }
                    }
                }

                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        /// <summary>
        /// This method will return the document in XML string format after trimming the document based on provided component sections
        /// </summary>
        /// <param name="C32Sections"></param>
        /// <param name="clinicalDocument"></param>
        /// <returns></returns>
        public Result GetTruncatedDocument(List<C32Section> C32Sections, out string clinicalDocument)
        {
            clinicalDocument = string.Empty;
            try
            {
                POCD_MT000040ClinicalDocument document = this.ClinicalDocument;
                ((POCD_MT000040StructuredBody)document.component.Item).component = this.TrimComponentSection(C32Sections).ToArray();
                clinicalDocument = XmlSerializerHelper.SerializeObject(document, Encoding.UTF8);

                Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;

        }

        /// <summary>
        /// This method will return the document bytes after trimming the document based on provided component sections
        /// </summary>
        /// <param name="C32Sections"></param>
        /// <param name="clinicalDocumentBytes"></param>
        /// <returns></returns>
        public Result GetTruncatedDocument(List<C32Section> C32Sections, out byte[] clinicalDocumentBytes)
        {
            clinicalDocumentBytes = null;
            try
            {
                string clinicalDocument = string.Empty;
                this.Result = GetTruncatedDocument(C32Sections, out clinicalDocument);
                if (this.Result.IsSuccess)
                {
                    clinicalDocumentBytes = Encoding.UTF8.GetBytes(clinicalDocument);
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;

        }

        #endregion

        #region Private Helper

        /// <summary>
        /// 
        /// </summary>
        /// <param name="C32Sections"></param>
        /// <returns></returns>
        private List<POCD_MT000040Component3> TrimComponentSection(List<C32Section> C32Sections)
        {
            Boolean flag = false;
            string lonicCode = string.Empty;
            List<POCD_MT000040Component3> Components = new List<POCD_MT000040Component3>();
            POCD_MT000040StructuredBody structuredBody = this.StructuredBody;

            if (structuredBody.component != null && structuredBody.component.Length > 0)
            {
                List<POCD_MT000040Component3> components = new List<POCD_MT000040Component3>();
                components.AddRange(structuredBody.component);
                foreach (POCD_MT000040Component3 component in components)
                {
                    lonicCode = string.Empty;
                    flag = false;
                    //Check LOINC code in exits in C32Modules
                    if (component.section != null && component.section.code != null)
                    {
                        lonicCode = component.section.code.code;
                    }

                    if (!string.IsNullOrEmpty(lonicCode))
                    {
                        if (C32Sections.Count(t => t.LONICCode == lonicCode && t.Allow == true) >= 1)
                        {
                            flag = true;
                            //getChildSection(component,C32Modules[0].Sections) 
                            Components.Add(component);
                        }

                    }
                    //Check for templates
                    if (!flag)
                    {
                        if (component.section.templateId != null && component.section.templateId.Length > 0)
                        {
                            if (C32Sections.Count(t => component.section.templateId.Any(a => a.root == t.TemplateId) && t.Allow == true) >= 1)
                            {
                                //getChildSection(component,C32Modules[0].Sections) 
                                Components.Add(component);
                            }
                        }
                    }

                }
            }
            return Components;
        }



        /// <summary>
        /// Get Specialty
        /// </summary>
        /// <returns></returns>
        private List<string> GetSpecialty()
        {
            List<string> authorSpecialties = new List<string>();

            if (this.ServiceEvent != null && this.ServiceEvent.performer != null && this.ServiceEvent.performer.Length > 0)
            {
                foreach (var item in this.ServiceEvent.performer)
                {
                    if (item.assignedEntity != null && item.assignedEntity.code != null)
                    {
                        authorSpecialties.Add(item.assignedEntity.code.displayName);
                    }
                }
            }
            return authorSpecialties;
        }

        /// <summary>
        /// Get Author Role
        /// </summary>
        /// <returns></returns>
        private List<string> GetAuthorRole()
        {
            List<string> authorRoles = new List<string>();

            if (this.ServiceEvent != null && this.ServiceEvent.performer != null && this.ServiceEvent.performer.Length > 0)
            {
                foreach (var item in this.ServiceEvent.performer)
                {
                    if (item.functionCode != null)
                    {
                        authorRoles.Add(item.functionCode.displayName);
                    }
                }
            }
            return authorRoles;
        }

        /// <summary>
        /// Get Author Institution
        /// </summary>
        /// <returns></returns>
        private List<string> GetAuthorInstitution()
        {
            List<string> authorInstitutions = new List<string>();
            if (this.Author != null && this.Author.assignedAuthor != null && this.Author.assignedAuthor.representedOrganization != null)
            {
                if (this.Author.assignedAuthor.representedOrganization.name.Length > 0)
                {
                    foreach (var item in this.Author.assignedAuthor.representedOrganization.name)
                    {
                        authorInstitutions.AddRange(item.Text);
                    }
                }
            }
            return authorInstitutions;
        }

        /// <summary>
        /// Get Author Name
        /// </summary>
        /// <returns></returns>
        private List<string> GetAuthorName()
        {
            List<string> authorName = new List<string>();

            if (this.Author != null && this.Author.assignedAuthor != null && this.Author.assignedAuthor.Item is POCD_MT000040Person)
            {
                POCD_MT000040Person Person = (POCD_MT000040Person)this.Author.assignedAuthor.Item;
                if (Person.name != null && (Person.name.Length > 0))
                {
                    foreach (var item in Person.name)
                    {
                        if (item != null && item.Text != null)
                            authorName.Add(string.Join("", item.Text));
                    }
                    if (authorName.Count == 0)
                    {
                        var namePart = GetNamePart(Person.name[0].Items);
                        string completeName="";
                        if (namePart.Prefix != null && namePart.Prefix.Count > 0)
                            completeName+= namePart.Prefix[0] + " ";
                        if (namePart.GivenName != null && namePart.GivenName.Count > 0)
                            completeName += namePart.GivenName[0] + " ";
                        if (namePart.MiddleName != null && namePart.MiddleName.Count > 0)
                            completeName += namePart.MiddleName[0] + " ";
                        if (namePart.FamilyName != null && namePart.FamilyName.Count > 0)
                            completeName += namePart.FamilyName[0] + " ";
                        if (namePart.Suffix != null && namePart.Suffix.Count > 0)
                            completeName += namePart.Suffix[0];

                        authorName.Add(completeName);
                    }

                }
            }





            return authorName;
        }


        /// <summary>
        /// Get Patient Address
        /// </summary>
        private List<PatientAddress> GetPatientAddress()
        {
            if (this.RecordTarget.patientRole.addr.Length > 0)
            {
                _PatientAddress = new List<PatientAddress>();
                PatientAddress patientAddress = null;
                foreach (var address in this.RecordTarget.patientRole.addr)
                {
                    patientAddress = new PatientAddress();

                    if (address.Items.Length > 0)
                    {
                        foreach (var item in address.Items)
                        {
                            if (item is adxpstreetAddressLine)
                            {
                                patientAddress.StreetAddressLine = string.Join(" ", item.Text);
                                continue;
                            }
                            else if (item is adxpcity)
                            {
                                patientAddress.City = string.Join(" ", item.Text);
                                continue;
                            }

                            else if (item is adxpstate)
                            {
                                patientAddress.State = string.Join(" ", item.Text);
                                continue;
                            }

                            else if (item is adxppostalCode)
                            {
                                patientAddress.PostalCode = string.Join(" ", item.Text);
                                continue;
                            }

                            else if (item is adxpcountry)
                            {
                                patientAddress.Country = string.Join(" ", item.Text);
                                continue;
                            }

                        }
                        _PatientAddress.Add(patientAddress);
                    }
                }
            }
            return _PatientAddress;
        }



        /// <summary>
        /// This method will load the Name part of Patient from PatientRole object.
        /// </summary>
        private void getPatientNamePart()
        {
            if (this.RecordTarget.patientRole.patient.name.Length > 0)
            {
                if (this.RecordTarget.patientRole.patient.name[0].Items.Length > 0)
                {
                    this._PatientName = GetNamePart(this.RecordTarget.patientRole.patient.name[0].Items);
                }
            }
        }

        /// <summary>
        /// Get Name object after reading collection of ENXP object
        /// </summary>
        /// <param name="ENXP"></param>
        /// <returns></returns>
        private Name GetNamePart(ENXP[] ENXP)
        {
            Name name = new Name();
            bool IsMiddleName = false;
            foreach (var item in ENXP)
            {
                //Given Name
                if (item is engiven)
                {
                    name.GivenName.AddRange(item.Text);
                    IsMiddleName = true;
                    continue;
                }
                //Middle Name
                else if (item is engiven && IsMiddleName)
                {
                    name.MiddleName.AddRange(item.Text);
                    continue;
                }
                //Family Name
                else if (item is enfamily)
                {
                    name.FamilyName.AddRange(item.Text);
                    continue;
                }
                //prefix
                else if (item is enprefix)
                {
                    name.Prefix.AddRange(item.Text);
                    continue;
                }
                //ensuffix
                else if (item is ensuffix)
                {
                    name.Suffix.AddRange(item.Text);
                    continue;
                }
            }
            return name;
        }


        /// <summary>
        /// This method trim clinical document based on provided component sections
        /// </summary>
        /// <param name="C32Sections"></param>
        /// <returns></returns>
        private List<POCD_MT000040Component3> GetC32SectionBasedOnConsent(List<C32Section> C32Sections)
        {
            Boolean flag = false;
            string lonicCode = string.Empty;
            List<POCD_MT000040Component3> Components = new List<POCD_MT000040Component3>();
            POCD_MT000040StructuredBody structuredBody = this.StructuredBody;

            if (structuredBody.component != null && structuredBody.component.Length > 0)
            {
                List<POCD_MT000040Component3> components = new List<POCD_MT000040Component3>();
                components.AddRange(structuredBody.component);
                foreach (POCD_MT000040Component3 component in components)
                {
                    lonicCode = string.Empty;
                    flag = false;
                    //Check Lonic code in exits in C32Modules
                    if (component.section != null && component.section.code != null)
                    {
                        lonicCode = component.section.code.code;
                    }

                    if (!string.IsNullOrEmpty(lonicCode))
                    {
                        //if (C32Sections.Count(t => t.LONICCode == lonicCode && t.Allow == true) >= 1)
                        //{
                        //    flag = true;
                        //    //getChildSection(component,C32Modules[0].Sections) 
                        //    Components.Add(component);
                        //}

                    }
                    //Check for templates
                    if (!flag)
                    {
                        if (component.section.templateId != null && component.section.templateId.Length > 0)
                        {
                            //if (C32Sections.Count(t => component.section.templateId.Any(a => a.root == t.TemplateId) && t.Allow == true) >= 1)
                            //{
                            //    //getChildSection(component,C32Modules[0].Sections) 
                            //    Components.Add(component);
                            //}
                        }
                    }

                }
            }
            return Components;
        }

        /// <summary>
        /// Get Service Time from clinical document
        /// </summary>
        /// <param name="effectiveTime"></param>
        private void GetServiceTime()
        {
            if (this.EffectiveTime != null)
            {
                _ServiceTime = new List<int>();
                foreach (var items in this.EffectiveTime.Items)
                {
                    if (items is IVXB_TS)
                    {
                        _ServiceTime.Add(Convert.ToInt32(((IVXB_TS)items).value));
                    }
                }
            }
        }

        /// <summary>
        /// Common  method which will desterilize clinical document     
        /// </summary>
        /// <param name="XMlDocument"></param>
        private void DeserializeObject(string XMlDocument)
        {
            try
            {
                this.ClinicalDocument = (POCD_MT000040ClinicalDocument)XmlSerializerHelper.DeserializeObject(XMlDocument, typeof(POCD_MT000040ClinicalDocument));
            }
            catch (Exception ex)
            {
                this.Result = new Result();
                this.Result.SetError(ErrorCode.CDAHELPER_INVALID_DOCUMENT);
                throw new ApplicationException(this.Result.ErrorMessage, ex);
            }
        }
        #endregion Private Helper
    }
}
