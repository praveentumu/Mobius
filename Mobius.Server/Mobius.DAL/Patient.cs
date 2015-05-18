
namespace Mobius.DAL
{
    #region Namespace
    using System;
    using System.Data;
    using System.Data.Common;
    using Mobius.Entity;
    using System.Data.SqlClient;
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using Mobius.CoreLibrary;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public partial class MobiusDAL
    {
        #region GetPatientDetails
        /// <summary>
        /// Get Patient Details via MPIID
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="MPIID"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result GetPatientDetails(out Mobius.Entity.Patient patient, string MPIID = "", string email = "")
        {
            Mobius.Entity.Telephone telephoneEntity = null;
            Mobius.Entity.Address addressEntity = null;
            Mobius.Entity.City cityEntity = null;
            Mobius.Entity.State stateEntity = null;
            Mobius.Entity.Country countryEntity = null;
            DataSet patientData = null;
            Result result = new Result();
            patient = null;
            try
            {
                // Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientDetails"))
                {
                    if (!string.IsNullOrEmpty(MPIID))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, MPIID);
                    }
                    if (!string.IsNullOrEmpty(email))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, email);
                    }
                    patientData = DataAccessManager.GetInstance.ExecuteDataSet(dbCommand);
                }
                if (patientData != null || patientData.Tables.Count > 0)
                {

                    patient = new Mobius.Entity.Patient();
                    // patient entity object filling
                    if (patientData.Tables[0] != null)
                    {
                        patient.PatientId = patientData.Tables[0].Rows[0]["PatientId"].ToString();
                        patient.EmailAddress = patientData.Tables[0].Rows[0]["EmailAddress"].ToString();
                        patient.CommunityId = patientData.Tables[0].Rows[0]["CommunityID"].ToString();
                        patient.SerialNumber = patientData.Tables[0].Rows[0]["SerialNumber"].ToString();
                        patient.PublicKey = patientData.Tables[0].Rows[0]["PublicKey"].ToString();

                        patient.DOB = patientData.Tables[0].Rows[0]["DOB"].ToString();
                        patient.Gender = (Gender)Convert.ToInt16(patientData.Tables[0].Rows[0]["Gender"]);

                        patient.LocalMPIID = MPIID;
                        //TODO
                        patient.MothersMaidenName.GivenName = patientData.Tables[0].Rows[0]["MothersMaidenGivenName"].ToString();
                        patient.MothersMaidenName.MiddleName = patientData.Tables[0].Rows[0]["MothersMaidenMiddleName"].ToString();
                        patient.MothersMaidenName.Prefix = patientData.Tables[0].Rows[0]["MothersMaidenPrefix"].ToString();
                        patient.MothersMaidenName.Suffix = patientData.Tables[0].Rows[0]["MothersMaidenSuffix"].ToString();
                        patient.MothersMaidenName.FamilyName = patientData.Tables[0].Rows[0]["MothersMaidenFamilyName"].ToString();

                        patient.SSN = patientData.Tables[0].Rows[0]["SSN"].ToString();
                        patient.BirthPlaceAddress = patientData.Tables[0].Rows[0]["BirthPlaceAddress"].ToString();
                        patient.BirthPlaceCity = patientData.Tables[0].Rows[0]["BirthPlaceCity"].ToString();
                        patient.BirthPlaceState = patientData.Tables[0].Rows[0]["BirthPlaceState"].ToString();
                        patient.BirthPlaceCountry = patientData.Tables[0].Rows[0]["BirthPlaceCountry"].ToString();
                        patient.BirthPlaceZip = patientData.Tables[0].Rows[0]["BirthPlaceZip"].ToString();


                    }
                    if (patientData.Tables[2] != null)
                    {
                        foreach (DataRow drDemo in patientData.Tables[2].Rows)
                        {
                            patient.IDNames.Add(Convert.ToInt32(drDemo["Id"]));
                            patient.GivenName.Add(drDemo["GivenName"].ToString());
                            patient.MiddleName.Add(drDemo["MiddleName"].ToString());
                            patient.FamilyName.Add(drDemo["FamilyName"].ToString());
                        }
                    }
                    if (patientData.Tables[3] != null)
                    {
                        foreach (DataRow drTelephone in patientData.Tables[3].Rows)
                        {
                            telephoneEntity = new Mobius.Entity.Telephone();
                            telephoneEntity.Id = Convert.ToInt32(drTelephone["TelephoneId"]);
                            telephoneEntity.Type = drTelephone["Type"].ToString();
                            telephoneEntity.Extensionnumber = drTelephone["ExtensionNumber"].ToString();
                            telephoneEntity.Number = drTelephone["TelephoneNumber"].ToString();
                            telephoneEntity.Status = Convert.ToBoolean(drTelephone["TelephoneStatus"]);
                            patient.Telephone.Add(telephoneEntity);
                        }
                    }

                    if (patientData.Tables[1] != null)
                    {
                        foreach (DataRow drAddress in patientData.Tables[1].Rows)
                        {
                            addressEntity = new Mobius.Entity.Address();
                            addressEntity.Id = Convert.ToInt32(drAddress["AddressId"]);
                            addressEntity.AddressLine1 = drAddress["AddressLine1"].ToString();
                            addressEntity.AddressLine2 = drAddress["AddressLine2"].ToString();
                            addressEntity.Zip = drAddress["Zip"].ToString();
                            // addressEntity.AddressStatus = Convert.ToBoolean(drAddress["AdressStatus"]);

                            addressEntity.AddressStatus = (AddressStatus)drAddress["AdressStatus"];

                            countryEntity = new Mobius.Entity.Country();
                            countryEntity.CountryName = drAddress["CountryName"].ToString();


                            stateEntity = new Mobius.Entity.State();
                            stateEntity.StateName = drAddress["StateName"].ToString();
                            stateEntity.Country = countryEntity;

                            cityEntity = new Mobius.Entity.City();
                            cityEntity.CityName = drAddress["CityName"].ToString();
                            cityEntity.State = stateEntity;
                            addressEntity.City = cityEntity;
                            patient.PatientAddress.Add(addressEntity);
                        }
                    }
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            // Added for issue id #138
            finally
            {
                telephoneEntity = null;
                addressEntity = null;
                cityEntity = null;
                stateEntity = null;
                countryEntity = null;
                if (patientData != null)
                {
                    patientData.Dispose();
                }

            }
            return result;
        }
        #endregion

        #region SearchPatient
        /// <summary>
        /// Get Patient details in to Patient table.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Result SearchPatient(Demographics demographics, out List<Patient> Patients)
        {
            // DbCommand dbCommand;
            Result result = new Result();
            DataSet dsPatients = new DataSet();
            DataTable dtPatient;
            Patient patient = null;
            Patients = new List<Patient>();
            try
            {
                result.IsSuccess = false;
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("SearchPatient"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "GivenName", DbType.String, demographics.GivenName);
                    _dataAccessManager.AddInParameter(dbCommand, "FamilyName", DbType.String, demographics.FamilyName);
                    _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, demographics.MiddleName);
                    _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.DateTime, Convert.ToDateTime(demographics.DOB));
                    if (!string.IsNullOrEmpty(demographics.DeceasedTime))
                        _dataAccessManager.AddInParameter(dbCommand, "DeceasedTime", DbType.DateTime, Convert.ToDateTime(demographics.DeceasedTime));

                    _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, demographics.SSN);
                    _dataAccessManager.AddInParameter(dbCommand, "Gender", DbType.Int16, demographics.Gender.GetHashCode());

                    _dataAccessManager.AddInParameter(dbCommand, "Suffix", DbType.String, demographics.Suffix);
                    _dataAccessManager.AddInParameter(dbCommand, "Prefix", DbType.String, demographics.Prefix);


                    _dataAccessManager.AddInParameter(dbCommand, "MPIID", DbType.String, demographics.LocalMPIID);

                    if (demographics.MothersMaidenName != null)
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenPrefix", DbType.String, demographics.MothersMaidenName.Prefix);
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenGivenName", DbType.String, demographics.MothersMaidenName.GivenName);
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenMiddleName", DbType.String, demographics.MothersMaidenName.MiddleName);
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenFamilyName", DbType.String, demographics.MothersMaidenName.FamilyName);
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenSuffix", DbType.String, demographics.MothersMaidenName.Suffix);
                    }

                    if (demographics.Addresses != null && demographics.Addresses.Count > 0)
                    {
                        string AddressLine = string.Empty;
                        string Zip = string.Empty;
                        string CityName = string.Empty;
                        string StateName = string.Empty;
                        string countryName = string.Empty;

                        foreach (Address address in demographics.Addresses)
                        {
                            if (!string.IsNullOrWhiteSpace(address.AddressLine1) || string.IsNullOrWhiteSpace(address.AddressLine2))
                                AddressLine = AddressLine + address.AddressLine1 + address.AddressLine2 + ",";
                            if (address.City != null && !string.IsNullOrWhiteSpace(address.City.CityName))
                            {
                                CityName = CityName + address.City.CityName + ",";
                            }
                            if (address.City != null && address.City.State != null && !string.IsNullOrWhiteSpace(address.City.State.StateName))
                            {
                                StateName = StateName + address.City.State.StateName + ",";
                            }

                            if (address.City != null && address.City.State != null && address.City.State.Country != null && !string.IsNullOrWhiteSpace(address.City.State.Country.CountryName))
                            {
                                countryName = countryName + address.City.State.Country.CountryName + ",";
                            }

                            if (!string.IsNullOrWhiteSpace(address.Zip))
                                Zip = Zip + address.Zip + ",";
                        }

                        if (!string.IsNullOrEmpty(AddressLine))
                        {
                            AddressLine = AddressLine.Remove(AddressLine.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "Street", DbType.String, AddressLine);
                        }

                        if (!string.IsNullOrEmpty(Zip))
                        {
                            Zip = Zip.Remove(Zip.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "Zip", DbType.String, Zip);
                        }

                        if (!string.IsNullOrEmpty(CityName))
                        {
                            CityName = CityName.Remove(CityName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "City", DbType.String, CityName);
                        }
                        if (!string.IsNullOrEmpty(StateName))
                        {
                            StateName = StateName.Remove(StateName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "State", DbType.String, StateName);
                        }

                        if (!string.IsNullOrEmpty(countryName))
                        {
                            countryName = countryName.Remove(countryName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "Country", DbType.String, countryName);
                        }
                    }


                    if (demographics.BirthPlaceAddress != null && demographics.BirthPlaceAddress.Count > 0)
                    {
                        string AddressLine = string.Empty;
                        string Zip = string.Empty;
                        string CityName = string.Empty;
                        string StateName = string.Empty;
                        string CountryName = string.Empty;
                        foreach (Address address in demographics.BirthPlaceAddress)
                        {
                            if (!string.IsNullOrWhiteSpace(address.AddressLine1) || string.IsNullOrWhiteSpace(address.AddressLine2))
                                AddressLine = AddressLine + address.AddressLine1 + address.AddressLine2 + ",";
                            if (address.City != null && !string.IsNullOrWhiteSpace(address.City.CityName))
                            {
                                CityName = CityName + address.City.CityName + ",";
                            }
                            if (address.City != null && address.City.State != null && !string.IsNullOrWhiteSpace(address.City.State.StateName))
                            {
                                StateName = StateName + address.City.State.StateName + ",";
                            }

                            if (address.City != null && address.City.State != null && address.City.State.Country != null
                                && !string.IsNullOrEmpty(address.City.State.Country.CountryName))
                            {
                                CountryName = CountryName + address.City.State.Country.CountryName + ",";
                            }

                            if (!string.IsNullOrWhiteSpace(address.Zip))
                                Zip = Zip + address.Zip + ",";
                        }

                        if (!string.IsNullOrEmpty(AddressLine))
                        {
                            AddressLine = AddressLine.Remove(AddressLine.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceAddress", DbType.String, AddressLine);
                        }

                        if (!string.IsNullOrEmpty(Zip))
                        {
                            Zip = Zip.Remove(Zip.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceZip", DbType.String, Zip);
                        }

                        if (!string.IsNullOrEmpty(CityName))
                        {
                            CityName = CityName.Remove(CityName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCity", DbType.String, CityName);
                        }
                        if (!string.IsNullOrEmpty(StateName))
                        {
                            StateName = StateName.Remove(StateName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceState", DbType.String, StateName);
                        }

                        if (!string.IsNullOrEmpty(CountryName))
                        {
                            CountryName = CountryName.Remove(CountryName.Length - 1);
                            _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCountry", DbType.String, CountryName);
                        }

                    }


                    if (demographics.Telephones != null && demographics.Telephones.Count > 0)
                    {
                        string telNumbers = "";
                        foreach (var item in demographics.Telephones)
                        {
                            telNumbers = telNumbers + item.Number + ",";
                        }

                        telNumbers = telNumbers.Remove(telNumbers.Length - 1);
                        _dataAccessManager.AddInParameter(dbCommand, "TelephoneNumber", DbType.String, telNumbers);
                    }
                    dsPatients = _dataAccessManager.ExecuteDataSet(dbCommand);
                }
                if (dsPatients.Tables.Count > 0)
                {
                    dtPatient = dsPatients.Tables[0];
                    if (dtPatient.Rows.Count > 0)
                    {
                        string MPPID = string.Empty;
                        bool IsNewPatient = true;
                        Telephone telephone = null;
                        Address address = null;

                        foreach (DataRow row in dtPatient.Rows)
                        {
                            if (row["MPIID"] != null)
                            {
                                MPPID = row["MPIID"].ToString();
                            }

                            if (!string.IsNullOrEmpty(MPPID))
                            {
                                if (Patients.Count == 0 || Patients.Count(t => t.LocalMPIID == MPPID) == 0)
                                {
                                    patient = new Patient();
                                    IsNewPatient = true;
                                    patient.RemoteMPIID = patient.LocalMPIID = MPPID;
                                    if (row["MothersMaidenPrefix"] != DBNull.Value)
                                    {
                                        patient.MothersMaidenName.Prefix = row["MothersMaidenPrefix"].ToString();
                                    }
                                    if (row["MothersMaidenGivenName"] != DBNull.Value)
                                    {
                                        patient.MothersMaidenName.GivenName = row["MothersMaidenGivenName"].ToString();
                                    }
                                    if (row["MothersMaidenMiddleName"] != DBNull.Value)
                                    {
                                        patient.MothersMaidenName.MiddleName = row["MothersMaidenMiddleName"].ToString();
                                    }
                                    if (row["MothersMaidenFamilyName"] != DBNull.Value)
                                    {
                                        patient.MothersMaidenName.GivenName = row["MothersMaidenFamilyName"].ToString();
                                    }
                                    if (row["MothersMaidenSuffix"] != DBNull.Value)
                                    {
                                        patient.MothersMaidenName.Suffix = row["MothersMaidenSuffix"].ToString();
                                    }

                                    if (row["BirthPlaceAddress"] != DBNull.Value)
                                    {
                                        patient.BirthPlaceAddress = row["BirthPlaceAddress"].ToString();
                                    }

                                    if (row["BirthPlaceCity"] != DBNull.Value)
                                    {
                                        patient.BirthPlaceCity = row["BirthPlaceCity"].ToString();
                                    }

                                    if (row["BirthPlaceState"] != DBNull.Value)
                                    {
                                        patient.BirthPlaceState = row["BirthPlaceState"].ToString();
                                    }
                                    if (row["BirthPlaceCountry"] != DBNull.Value)
                                    {
                                        patient.BirthPlaceCountry = row["BirthPlaceCountry"].ToString();
                                    }
                                    if (row["BirthPlaceZip"] != DBNull.Value)
                                    {
                                        patient.BirthPlaceZip = row["BirthPlaceZip"].ToString();
                                    }
                                    if (row["DOB"] != DBNull.Value)
                                    {
                                        patient.DOB = ReversdateFormatter(Convert.ToDateTime(row["DOB"].ToString()).ToShortDateString());
                                    }

                                    if (row["DeceasedTime"] != DBNull.Value)
                                    {
                                        patient.DeceasedTime = ReversdateFormatter(Convert.ToDateTime(row["DeceasedTime"].ToString()).ToShortDateString());
                                    }

                                    if (row["Gender"] != DBNull.Value)
                                    {
                                        patient.Gender = (Gender)Convert.ToInt16(row["Gender"]);
                                    }
                                    if (row["SSN"] != DBNull.Value)
                                    {
                                        patient.SSN = row["SSN"].ToString();
                                    }

                                    if (row["CommunityID"] != DBNull.Value)
                                    {
                                        patient.CommunityId = row["CommunityID"].ToString();
                                    }

                                    //Add 
                                    if (row["FamilyName"] != DBNull.Value)
                                    {
                                        patient.FamilyName.Add(row["FamilyName"].ToString());
                                    }

                                    if (row["MiddleName"] != DBNull.Value)
                                    {
                                        patient.MiddleName.Add(row["MiddleName"].ToString());
                                    }
                                    if (row["GivenName"] != DBNull.Value)
                                    {
                                        patient.GivenName.Add(row["GivenName"].ToString());
                                    }

                                    if (row["Suffix"] != DBNull.Value)
                                    {
                                        patient.Suffix.Add(row["Suffix"].ToString());
                                    }

                                    if (row["Prefix"] != DBNull.Value)
                                    {
                                        patient.Prefix.Add(row["Prefix"].ToString());
                                    }


                                }
                                else
                                {
                                    IsNewPatient = false;
                                }
                            }
                            else
                            {
                                continue;
                            }

                            //Add Multiple Telephone Numbers
                            telephone = new Telephone();
                            telephone.MPIID = MPPID;
                            if (row["TelephoneNumber"] != DBNull.Value)
                            {
                                telephone.Number = row["TelephoneNumber"].ToString();
                            }
                            if (row["ExtensionNumber"] != DBNull.Value)
                            {
                                telephone.Extensionnumber = row["ExtensionNumber"].ToString();
                            }

                            if (patient.Telephone.Count(t => t.Extensionnumber == telephone.Extensionnumber && t.Number == telephone.Number) == 0)
                            {
                                //Add Telephone number in Patient object
                                patient.Telephone.Add(telephone);
                            }


                            //Add Multiple Telephone Numbers
                            address = new Address();
                            if (row["AddressLine1"] != DBNull.Value)
                            {
                                address.AddressLine1 = row["AddressLine1"].ToString();
                            }
                            if (row["AddressLine2"] != DBNull.Value)
                            {
                                address.AddressLine2 = row["AddressLine2"].ToString();
                            }
                            if (row["Status"] != DBNull.Value)
                            {
                                address.AddressStatus = (AddressStatus)Convert.ToInt16(row["Status"]);
                            }
                            else
                            {
                                address.AddressStatus = AddressStatus.Primary;
                            }
                            if (row["CityName"] != DBNull.Value)
                            {
                                address.City.CityName = row["CityName"].ToString();
                            }
                            if (row["StateName"] != DBNull.Value)
                            {
                                address.City.State.StateName = row["StateName"].ToString();
                            }
                            if (row["CountryName"] != DBNull.Value)
                            {
                                address.City.State.Country.CountryName = row["CountryName"].ToString();
                            }
                            if (row["Zip"] != DBNull.Value)
                            {
                                address.Zip = row["Zip"].ToString();
                            }
                            address.MPIID = MPPID;

                            if (patient.PatientAddress.Count(t => t.AddressLine1 == address.AddressLine1 && t.AddressLine2 == address.AddressLine2
                                    && t.AddressStatus == address.AddressStatus && t.Zip == address.Zip
                                    && t.City.CityName == address.City.CityName && t.City.State.StateName == address.City.State.StateName
                                    && t.City.State.Country.CountryName == address.City.State.Country.CountryName) == 0)
                            {
                                //Add Address in Patient collection 
                                patient.PatientAddress.Add(address);
                            }

                            if (IsNewPatient)
                            {

                                Patients.Add(patient);
                            }
                        }

                        result.IsSuccess = true;
                    }
                }

                if (Patients.Count == 0)
                {
                    result.IsSuccess = false;
                    result.SetError(ErrorCode.No_Result_Found);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dsPatients != null)
                {
                    demographics = null;
                    dsPatients.Dispose();
                }
            }
            return result;
        }
        #endregion

        #region CheckPatient
        /// <summary>
        /// Verify Patient available in to Patient table of database or not
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Result CheckPatient(Patient patient)
        {

            DataSet dsPatients = new DataSet();
            this.Result = new Result();
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("CheckPatient"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "EmailAddress", DbType.String, patient.EmailAddress);
                    dsPatients = _dataAccessManager.ExecuteDataSet(dbCommand);
                }
                if (dsPatients.Tables.Count > 0 && dsPatients.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPatients.Tables[0].Rows)
                    {
                        if (dr["PatientId"] != DBNull.Value)
                        {
                            this.Result.IsSuccess = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                if (dsPatients != null)
                {
                    patient = null;
                    dsPatients.Dispose();
                }
            }
            return this.Result;
        }
        #endregion

        #region AddPatient
        /// <summary>
        ///  insert patient details in to Patient table of database
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Result AddPatient(Patient patient)
        {
            Result result = new Result();
            DbCommand dbCommand = null;
            Int64 patientId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("AddPatient");
                _dataAccessManager.AddInParameter(dbCommand, "EmailAddress", DbType.String, patient.EmailAddress);

                _dataAccessManager.AddInParameter(dbCommand, "Gender", DbType.String, patient.Gender.GetHashCode().ToString());

                _dataAccessManager.AddInParameter(dbCommand, "CommunityId", DbType.String, patient.CommunityId);
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.DateTime, patient.DOB);
                _dataAccessManager.AddInParameter(dbCommand, "MPIID", DbType.String, patient.LocalMPIID);

                //Add new column "Certificate Expiration Date" in Patient Table to store certificate Expiration time
                _dataAccessManager.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime,patient.CreatedOn);
                _dataAccessManager.AddInParameter(dbCommand, "ExpiryOn", DbType.DateTime, patient.ExpiryOn);

                if (string.IsNullOrWhiteSpace(patient.SSN))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, patient.SSN);
                }

                //Add new column "Certificate Creation Date" in Patient Table to store certificate creation time

                if (string.IsNullOrWhiteSpace(patient.PublicKey))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "PublicKey", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "PublicKey", DbType.String, patient.PublicKey);
                }
                if (string.IsNullOrWhiteSpace(patient.SerialNumber))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SerialNumber", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SerialNumber", DbType.String, patient.SerialNumber);
                }
                if (string.IsNullOrWhiteSpace(patient.Password))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, patient.Password);
                }

                if (string.IsNullOrWhiteSpace(patient.BirthPlaceAddress))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceAddress", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceAddress", DbType.String, patient.BirthPlaceAddress);
                }

                if (string.IsNullOrWhiteSpace(patient.BirthPlaceCity))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCity", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCity", DbType.String, patient.BirthPlaceCity);
                }

                if (string.IsNullOrWhiteSpace(patient.BirthPlaceState))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceState", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceState", DbType.String, patient.BirthPlaceState);
                }

                if (string.IsNullOrWhiteSpace(patient.BirthPlaceCountry))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCountry", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCountry", DbType.String, patient.BirthPlaceCountry);
                }

                if (string.IsNullOrWhiteSpace(patient.BirthPlaceZip))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceZip", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceZip", DbType.String, patient.BirthPlaceZip);
                }


                if (patient.MothersMaidenName != null)
                {
                    if (string.IsNullOrWhiteSpace(patient.MothersMaidenName.Prefix))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenPrefix", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenPrefix", DbType.String, patient.MothersMaidenName.Prefix);
                    }

                    if (string.IsNullOrWhiteSpace(patient.MothersMaidenName.GivenName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenGivenName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenGivenName", DbType.String, patient.MothersMaidenName.GivenName);
                    }

                    if (string.IsNullOrWhiteSpace(patient.MothersMaidenName.MiddleName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenMiddleName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenMiddleName", DbType.String, patient.MothersMaidenName.MiddleName);
                    }

                    if (string.IsNullOrWhiteSpace(patient.MothersMaidenName.FamilyName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenFamilyName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenFamilyName", DbType.String, patient.MothersMaidenName.FamilyName);
                    }

                    if (string.IsNullOrWhiteSpace(patient.MothersMaidenName.Suffix))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenSuffix", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenSuffix", DbType.String, patient.MothersMaidenName.Suffix);
                    }
                }


                //Get Sql connection Object
                DbProviderFactory factory = SqlClientFactory.Instance;
                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = MobiusAppSettingReader.ConnectionString;
                    connection.Open();

                    using (DbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            patientId = Int64.Parse(_dataAccessManager.ExecuteScalar(dbCommand, transaction).ToString());
                            if (patientId > 0)
                            {
                                patient.PatientId = patientId.ToString();
                                //insert patient Telephone
                                if (!InsertTelephoneDetails(patient, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }
                                //insert patient Address
                                if (!InsertAddressDetails(patient, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }

                                //insert patient name
                                if (!InsertPatientNames(patient, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }
                                result.IsSuccess = true;

                            }
                            transaction.Commit();
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            result.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            result.IsSuccess = false;
                            result.SetError(ErrorCode.UnknownException, ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
                dbCommand.Transaction.Rollback();

            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
            }

            return result;
        }
        #endregion

        #region UpdatePatient
        /// <summary>
        /// UpdatePatient details
        /// </summary>
        /// <param name="patientEntity"></param>
        /// <returns></returns>
        public Result UpdatePatient(Patient patientEntity)
        {
            Result result = new Result();
            DbCommand dbCommand = null;
            Int64 patientId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdatePatient");
                _dataAccessManager.AddInParameter(dbCommand, "EmailAddress", DbType.String, patientEntity.EmailAddress);
                _dataAccessManager.AddInParameter(dbCommand, "Gender", DbType.String, patientEntity.Gender.GetHashCode().ToString());
                _dataAccessManager.AddInParameter(dbCommand, "CommunityId", DbType.String, patientEntity.CommunityId);
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.DateTime, patientEntity.DOB);
                _dataAccessManager.AddInParameter(dbCommand, "MPIID", DbType.String, patientEntity.LocalMPIID);

                if (string.IsNullOrWhiteSpace(patientEntity.SSN))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, patientEntity.SSN);
                }
                if (string.IsNullOrWhiteSpace(patientEntity.BirthPlaceAddress))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceAddress", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceAddress", DbType.String, patientEntity.BirthPlaceAddress);
                }

                if (string.IsNullOrWhiteSpace(patientEntity.BirthPlaceCity))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCity", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCity", DbType.String, patientEntity.BirthPlaceCity);
                }

                if (string.IsNullOrWhiteSpace(patientEntity.BirthPlaceState))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceState", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceState", DbType.String, patientEntity.BirthPlaceState);
                }

                if (string.IsNullOrWhiteSpace(patientEntity.BirthPlaceCountry))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCountry", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceCountry", DbType.String, patientEntity.BirthPlaceCountry);
                }

                if (string.IsNullOrWhiteSpace(patientEntity.BirthPlaceZip))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceZip", DbType.String, DBNull.Value);
                }
                else
                {
                    _dataAccessManager.AddInParameter(dbCommand, "BirthPlaceZip", DbType.String, patientEntity.BirthPlaceZip);
                }

                if (patientEntity.MothersMaidenName != null)
                {
                    if (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.Prefix))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenPrefix", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenPrefix", DbType.String, patientEntity.MothersMaidenName.Prefix);
                    }

                    if (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.GivenName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenGivenName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenGivenName", DbType.String, patientEntity.MothersMaidenName.GivenName);
                    }

                    if (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.MiddleName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenMiddleName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenMiddleName", DbType.String, patientEntity.MothersMaidenName.MiddleName);
                    }

                    if (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.FamilyName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenFamilyName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenFamilyName", DbType.String, patientEntity.MothersMaidenName.FamilyName);
                    }

                    if (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.Suffix))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenSuffix", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MothersMaidenSuffix", DbType.String, patientEntity.MothersMaidenName.Suffix);
                    }
                }
                //Get Sql connection Object
                DbProviderFactory factory = SqlClientFactory.Instance;
                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = MobiusAppSettingReader.ConnectionString;
                    connection.Open();

                    using (DbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            patientId = Int64.Parse(_dataAccessManager.ExecuteNonQuery(dbCommand, transaction).ToString());
                            if (patientId > 0)
                            {
                                //update patient Telephone
                                if (!UpdateTelephoneDetails(patientEntity, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }

                                //update patient Address
                                if (!UpdateAddressDetails(patientEntity, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }

                                //update patient name
                                if (!UpdatePatientNames(patientEntity, transaction).IsSuccess)
                                {
                                    result.IsSuccess = false;
                                }
                            }


                            transaction.Commit();
                            result.IsSuccess = true;
                        }
                        catch (SqlException)
                        {
                            result.IsSuccess = false;
                            transaction.Rollback();

                        }
                        catch (Exception ex)
                        {
                            result.IsSuccess = false;
                            throw new Exception(ex.Message.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                dbCommand.Transaction.Rollback();
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
            }

            return result;
        }
        #endregion

        #region UpdatePatientPassword
        /// <summary>
        /// This method will update the Patient record for new passowrd
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Result UpdatePatientPassword(string emailAddress, string newPassword)
        {
            try
            {
                int returnValue = 0;
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdatePatientPassword"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "EmailAddress", DbType.String, emailAddress);
                    _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, newPassword);
                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    if (returnValue == 1)
                    {
                        this.Result.IsSuccess = true;
                    }
                    else
                    {
                        this.Result.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }
        #endregion UpdatePatientPassword

        #region PatientNames

        private Result UpdatePatientNames(Patient patient, DbTransaction transaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DataSet dataSet = new DataSet();
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            // Establish the Insert commands.

            //Added try and catch for issue id #138
            try
            {
                dtInsertRows = this.GetPatientNameDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);
                int returnValue;

                foreach (DataRow dr in dtInsertRows.Rows)
                {
                    string action = string.Empty;
                    ActionType Action = (ActionType)Enum.Parse(typeof(ActionType), dr["Action"].ToString());
                    if (Action == ActionType.Insert || Action == ActionType.Update)
                    {
                        updateCommand = _dataAccessManager.GetStoredProcCommand("UpdateNameDetails");
                        _dataAccessManager.AddInParameter(updateCommand, "ID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "FamilyName", DbType.String, dr["FamilyName"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "MiddleName", DbType.String, dr["MiddleName"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "GivenName", DbType.String, dr["GivenName"].ToString());
                        returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(updateCommand));
                    }
                    else
                    {
                        if (Action == ActionType.Delete)
                        {
                            deleteCommand = _dataAccessManager.GetStoredProcCommand("DeleteNameDetails");
                            _dataAccessManager.AddInParameter(deleteCommand, "DeletedID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                            _dataAccessManager.AddInParameter(deleteCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                            returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(deleteCommand));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                patient = null;
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
            }
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// Insert Patient Names into to PatientNAMES table of database
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        private Result InsertPatientNames(Patient patient, DbTransaction dbTransaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DataSet dataSet = new DataSet();
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            // Establish the Insert commands.
            //Added rty catch and finally for issue id #138
            try
            {
                dtInsertRows = this.GetPatientNameDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);
                //Modified for issue id #138
                using (DbCommand insertCommand = _dataAccessManager.GetStoredProcCommand("InsertPatientNames"))
                {
                    _dataAccessManager.AddInParameter(insertCommand, "PatientId", DbType.Int64, "PatientId", DataRowVersion.Current);
                    _dataAccessManager.AddInParameter(insertCommand, "GivenName", DbType.String, "GivenName", DataRowVersion.Current);
                    _dataAccessManager.AddInParameter(insertCommand, "MiddleName", DbType.String, "MiddleName", DataRowVersion.Current);
                    _dataAccessManager.AddInParameter(insertCommand, "FamilyName", DbType.String, "FamilyName", DataRowVersion.Current);



                    _dataAccessManager.AddInParameter(insertCommand, "PrefixName", DbType.String, "PrefixName", DataRowVersion.Current);
                    _dataAccessManager.AddInParameter(insertCommand, "SuffixName", DbType.String, "SuffixName", DataRowVersion.Current);

                    if (_dataAccessManager.UpdateBatch(dataSet, "PatientName", insertCommand, updateCommand, deleteCommand, dbTransaction) > 0)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            //Added for issue id #138
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }

                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }
            }

            return result;
        }
        /// <summary>
        /// Get Patient Name DataTable
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private DataTable GetPatientNameDataTable(Patient patient)
        {
            // You First need a DataTable and have all the insert values in it
            DataTable dtInsertRows = new DataTable();
            dtInsertRows.TableName = "PatientName";
            if (patient.IDNames != null)
            {
                dtInsertRows.Columns.Add("ID", Type.GetType("System.Int64"));
            }
            dtInsertRows.Columns.Add("PatientId", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("FamilyName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("MiddleName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("GivenName", Type.GetType("System.String"));

            dtInsertRows.Columns.Add("PrefixName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("SuffixName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("Action", Type.GetType("System.String"));

            if (patient.GivenName.Count > 0)
            {

                for (int i = 0; i < patient.GivenName.Count; i++)
                {
                    // Add Rows
                    DataRow drInsertRow = dtInsertRows.NewRow();
                    if (patient.IDNames != null)
                    {
                        drInsertRow["ID"] = patient.IDNames[i];
                    }
                    drInsertRow["PatientId"] = patient.PatientId;
                    drInsertRow["FamilyName"] = patient.FamilyName[i];
                    drInsertRow["MiddleName"] = patient.MiddleName[i]; ;
                    drInsertRow["GivenName"] = patient.GivenName[i];
                    drInsertRow["PrefixName"] = patient.Prefix[i]; ;
                    drInsertRow["SuffixName"] = patient.Suffix[i];
                    drInsertRow["Action"] = patient.Action[i].ToString();
                    dtInsertRows.Rows.Add(drInsertRow);
                }

            }
            //if (patient.Action.Count > patient.GivenName.Count)
            //{
            //    for (int Count = patient.GivenName.Count; Count < patient.Action.Count(); Count++)
            //    {
            //        DataRow drInsertRow = dtInsertRows.NewRow();
            //        drInsertRow["ID"] = patient.IDNames[Count];
            //        drInsertRow["PatientId"] = patient.PatientId;
            //        drInsertRow["Action"] = patient.Action[Count].ToString();
            //        dtInsertRows.Rows.Add(drInsertRow);
            //    }
            //}
            return dtInsertRows;

        }
        #endregion

        #region AddressDetails

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientEntity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private Result UpdateAddressDetails(Patient patient, DbTransaction transaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DbCommand deleteCommand = null;
            DataSet dataSet = new DataSet();
            DbCommand updateCommand = null;
            // Establish the Update commands.

            // //Added try,catch & finally for issue id #138
            try
            {
                dtInsertRows = this.GetPatientAddressDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);
                int returnValue;


                foreach (DataRow dr in dtInsertRows.Rows)
                {
                    string action = string.Empty;
                    ActionType Action = (ActionType)Enum.Parse(typeof(ActionType), dr["Action"].ToString());
                    if (Action == ActionType.Insert || Action == ActionType.Update)
                    {
                        updateCommand = _dataAccessManager.GetStoredProcCommand("UpdateAddressDetails");
                        _dataAccessManager.AddInParameter(updateCommand, "ID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "AddressLine1", DbType.String, dr["AddressLine1"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "AddressLine2", DbType.String, dr["AddressLine2"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "CountryName", DbType.String, dr["CountryName"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "StateName", DbType.String, dr["StateName"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "CityName", DbType.String, dr["CityName"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "Zip", DbType.String, dr["Zip"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "Status", DbType.String, dr["Status"]);
                        returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(updateCommand));
                    }
                    else
                    {
                        if (Action == ActionType.Delete)
                        {
                            deleteCommand = _dataAccessManager.GetStoredProcCommand("DeleteAddressDetails");
                            _dataAccessManager.AddInParameter(deleteCommand, "DeletedID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                            _dataAccessManager.AddInParameter(deleteCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                            returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(deleteCommand));
                        }
                    }

                }

            }
            //Added for issue id #138
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
            }
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// Insert Patient Address into to PatientAddress table of database
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        private Result InsertAddressDetails(Patient patient, DbTransaction dbTransaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DataSet dataSet = new DataSet();
            DbCommand insertCommand = null;
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            // Establish the Insert commands.

            //Added try,catch & finally for issue id #138
            try
            {
                dtInsertRows = this.GetPatientAddressDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);

                insertCommand = _dataAccessManager.GetStoredProcCommand("InsertAddressDetails");
                _dataAccessManager.AddInParameter(insertCommand, "PatientId", DbType.Int64, "PatientId", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "AddressLine1", DbType.String, "AddressLine1", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "AddressLine2", DbType.String, "AddressLine2", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "CountryName", DbType.String, "CountryName", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "StateName", DbType.String, "StateName", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "CityName", DbType.String, "CityName", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "Zip", DbType.String, "Zip", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "Status", DbType.String, "Status", DataRowVersion.Current);

                if (_dataAccessManager.UpdateBatch(dataSet, "PatientAddress", insertCommand, updateCommand, deleteCommand, dbTransaction) > 0)
                {
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                patient = null;
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
                if (insertCommand != null)
                {
                    insertCommand.Dispose();
                }
                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }

            }
            return result;
        }
        /// <summary>
        /// Get Patient Address DataTable
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private DataTable GetPatientAddressDataTable(Patient patient)
        {
            // You First need a DataTable and have all the insert values in it
            DataTable dtInsertRows = new DataTable();
            dtInsertRows.TableName = "PatientAddress";
            dtInsertRows.Columns.Add("ID", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("PatientId", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("AddressLine1", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("AddressLine2", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("CountryName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("StateName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("CityName", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("Zip", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("Status", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("Action", Type.GetType("System.String"));
            if (patient.GivenName.Count > 0)
            {
                for (int i = 0; i < patient.PatientAddress.Count; i++)
                {
                    // Add Rows
                    DataRow drInsertRow = dtInsertRows.NewRow();
                    drInsertRow["ID"] = patient.PatientAddress[i].Id;
                    drInsertRow["PatientId"] = patient.PatientId;
                    drInsertRow["AddressLine1"] = patient.PatientAddress[i].AddressLine1;
                    drInsertRow["AddressLine2"] = patient.PatientAddress[i].AddressLine2;
                    drInsertRow["CountryName"] = patient.PatientAddress[i].City.State.Country.CountryName;
                    drInsertRow["StateName"] = patient.PatientAddress[i].City.State.StateName;
                    drInsertRow["CityName"] = patient.PatientAddress[i].City.CityName;
                    drInsertRow["Zip"] = patient.PatientAddress[i].Zip;
                    drInsertRow["Action"] = patient.PatientAddress[i].Action;
                    int addressState = 0;
                    if (patient.PatientAddress[i].AddressStatus == AddressStatus.Primary)
                    {
                        addressState = 1;
                    }
                    else if (patient.PatientAddress[i].AddressStatus == AddressStatus.Secondary)
                    {
                        addressState = 2;
                    }
                    else
                    {
                        addressState = 3;
                    }
                    drInsertRow["Status"] = addressState;
                    dtInsertRows.Rows.Add(drInsertRow);
                }
            }
            return dtInsertRows;

        }
        #endregion

        #region TelephoneDetails

        /// <summary>
        /// Update Patient telephone into to PatientTelephone table of database 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        private Result UpdateTelephoneDetails(Patient patient, DbTransaction dbTransaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DataSet dataSet = new DataSet();
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            // Establish the Insert commands.
            //Added try,catch & finally for issue id #138
            try
            {
                dtInsertRows = this.GetPatientTelephoneDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);
                int returnValue;

                foreach (DataRow dr in dtInsertRows.Rows)
                {
                    string action = string.Empty;
                    ActionType Action = (ActionType)Enum.Parse(typeof(ActionType), dr["Action"].ToString());
                    if (Action == ActionType.Insert || Action == ActionType.Update)
                    {
                        updateCommand = _dataAccessManager.GetStoredProcCommand("UpdateTelephoneDetails");
                        _dataAccessManager.AddInParameter(updateCommand, "ID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                        _dataAccessManager.AddInParameter(updateCommand, "TelephoneNumber", DbType.String, dr["TelephoneNumber"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "Type", DbType.String, dr["Type"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "ExtensionNumber", DbType.String, dr["ExtensionNumber"].ToString());
                        _dataAccessManager.AddInParameter(updateCommand, "Status", DbType.Boolean, Convert.ToBoolean(dr["Status"]));
                        returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(updateCommand));
                    }
                    else
                    {
                        if (Action == ActionType.Delete)
                        {
                            deleteCommand = _dataAccessManager.GetStoredProcCommand("DeleteTelephoneDetails");
                            _dataAccessManager.AddInParameter(deleteCommand, "DeletedID", DbType.Int64, Convert.ToInt64(dr["ID"].ToString()));
                            _dataAccessManager.AddInParameter(deleteCommand, "PatientId", DbType.Int64, Convert.ToInt64(dr["PatientId"].ToString()));
                            returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(deleteCommand));
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                patient = null;
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }
            }
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// Insert Patient telephone into to PatientTelephone table of database 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        private Result InsertTelephoneDetails(Patient patient, DbTransaction dbTransaction)
        {
            Result result = new Result();
            DataTable dtInsertRows = null;
            DataSet dataSet = new DataSet();
            DbCommand insertCommand = null;
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            // Establish the Insert commands.
            //Added try,catch & finally for issue id #138
            try
            {
                dtInsertRows = this.GetPatientTelephoneDataTable(patient);
                dataSet.Tables.Add(dtInsertRows);

                insertCommand = _dataAccessManager.GetStoredProcCommand("InsertTelephoneDetails");
                _dataAccessManager.AddInParameter(insertCommand, "PatientId", DbType.Int64, "PatientId", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "TelephoneNumber", DbType.String, "TelephoneNumber", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "ExtensionNumber", DbType.String, "ExtensionNumber", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "Type", DbType.String, "Type", DataRowVersion.Current);
                _dataAccessManager.AddInParameter(insertCommand, "Status", DbType.Boolean, "Status", DataRowVersion.Current);


                if (_dataAccessManager.UpdateBatch(dataSet, "PatientTelephone", insertCommand, updateCommand, deleteCommand, dbTransaction) > 0)
                {
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                if (dtInsertRows != null)
                {
                    dtInsertRows.Dispose();
                }
                if (dataSet != null)
                {
                    dataSet.Dispose();
                }
                if (insertCommand != null)
                {
                    insertCommand.Dispose();
                }
                if (updateCommand != null)
                {
                    updateCommand.Dispose();
                }
                if (deleteCommand != null)
                {
                    deleteCommand.Dispose();
                }


            }
            return result;
        }
        /// <summary>
        /// Get Patient telephone DataTable
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private DataTable GetPatientTelephoneDataTable(Patient patient)
        {
            // You First need a DataTable and have all the insert values in it
            DataTable dtInsertRows = new DataTable();
            dtInsertRows.TableName = "PatientTelephone";
            dtInsertRows.Columns.Add("ID", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("PatientId", Type.GetType("System.Int64"));
            dtInsertRows.Columns.Add("TelephoneNumber", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("ExtensionNumber", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("Type", Type.GetType("System.String"));
            dtInsertRows.Columns.Add("Status", Type.GetType("System.Boolean"));
            dtInsertRows.Columns.Add("Action", Type.GetType("System.String"));
            if (patient.GivenName.Count > 0)
            {
                for (int i = 0; i < patient.Telephone.Count; i++)
                {
                    // Add Rows
                    DataRow drInsertRow = dtInsertRows.NewRow();
                    drInsertRow["ID"] = patient.Telephone[i].Id;
                    drInsertRow["PatientId"] = patient.PatientId;
                    drInsertRow["TelephoneNumber"] = patient.Telephone[i].Number;
                    drInsertRow["ExtensionNumber"] = patient.Telephone[i].Extensionnumber;
                    drInsertRow["Type"] = patient.Telephone[i].Type;
                    drInsertRow["Status"] = patient.Telephone[i].Status;
                    drInsertRow["Action"] = patient.Telephone[i].Action.ToString();
                    dtInsertRows.Rows.Add(drInsertRow);
                }
            }
            return dtInsertRows;

        }
        #endregion

        #region GetPatientConsent
        /// <summary>
        /// GetPatientConsent method would fetch consent for given MPIID 
        /// </summary>
        /// <param name="MPIID">MPIID of the patient</param>
        /// <param name="patientConsent">patient consent object</param>
        /// <returns>Result object</returns>
        public Result GetPatientConsent(string MPIID, out List<MobiusPatientConsent> PatientConsents)
        {
            #region Variables
            Result result = null;
            PatientConsents = null;
            DataTable dtConsent = null;
            #endregion
            try
            {
                PatientConsents = new List<MobiusPatientConsent>();
                result = new Result();

                MobiusPatientConsent PatientConsent = null;

                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientConsent"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "MpiId", DbType.String, MPIID);

                    using (DataSet dsPatientConsent = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {

                        if (dsPatientConsent.Tables.Count > 0)
                        {
                            dtConsent = dsPatientConsent.Tables[0];
                            if (dtConsent.Rows.Count > 0)
                            {

                                foreach (DataRow row in dtConsent.Rows)
                                {
                                    PatientConsent = new MobiusPatientConsent();

                                    if (row["Role"] != DBNull.Value)
                                        PatientConsent.Role = row["Role"].ToString();

                                    if (row["PatientConsentID"] != DBNull.Value)
                                        PatientConsent.PatientConsentID = Convert.ToInt32(row["PatientConsentID"]);

                                    if (row["PurposeOfUseId"] != DBNull.Value)
                                        PatientConsent.PurposeOfUseId = Convert.ToInt32(row["PurposeOfUseId"]);

                                    //if (row["C32SectionId"] != DBNull.Value)
                                    //    PatientConsent.C32SectionId = Convert.ToInt32(row["C32SectionId"]);

                                    if (row["Code"] != DBNull.Value)
                                        PatientConsent.Code = row["Code"].ToString();

                                    if (row["RoleId"] != DBNull.Value)
                                        PatientConsent.RoleId = Convert.ToInt32(row["RoleId"]);

                                    if (row["RuleStartDate"] != DBNull.Value)
                                        PatientConsent.RuleStartDate = row["RuleStartDate"].ToString();

                                    if (row["RuleEndDate"] != DBNull.Value)
                                        PatientConsent.RuleEndDate = row["RuleEndDate"].ToString();

                                    if (row["Allow"] != DBNull.Value)
                                        PatientConsent.Allow = Convert.ToBoolean(row["Allow"]);

                                    if (row["Active"] != DBNull.Value)
                                        PatientConsent.Active = Convert.ToBoolean(row["Active"]);

                                    if (row["Permission"] != DBNull.Value)
                                        PatientConsent.Permission = row["Permission"].ToString();

                                    PatientConsents.Add(PatientConsent);
                                }

                            }
                        }
                    }
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                if (dtConsent != null)
                {
                    dtConsent.Dispose();
                }
            }
            return result;
        }
        #endregion

        #region GetPatientConsentById
        /// <summary>
        /// Gets patientconsent using mpiid and consentId
        /// </summary>
        /// <param name="MPIID">Patient's Master Patient Id</param>
        /// <param name="patientConsentID">Id of the consent</param>
        /// <param name="PatientConsents">Consent set of the patient</param>
        /// <returns>returns result object</returns>
        /// 
        public Result GetPatientConsentByConsentId(string MPIID, int patientConsentID, out MobiusPatientConsent PatientConsent)
        {
            #region variables
            Result result = null;
            PatientConsent = null;
            #endregion

            try
            {
                //MobiusPatientConsent PatientConsent = null;
                DataTable dtConsent = null;

                result = new Result();
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientConsentById"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@MPIId", DbType.String, MPIID);
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.Int32, patientConsentID);
                    //Modified for issue id #138
                    using (DataSet dsPatientConsent = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {

                        if (dsPatientConsent.Tables.Count > 0)
                        {
                            dtConsent = dsPatientConsent.Tables[0];
                            if (dtConsent.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtConsent.Rows)
                                {
                                    PatientConsent = new MobiusPatientConsent();

                                    if (row["Allow"] != DBNull.Value)
                                        PatientConsent.Allow = Convert.ToBoolean(row["Allow"]);

                                    if (row["Consent"] != DBNull.Value && !string.IsNullOrEmpty(row["Consent"].ToString()))
                                        PatientConsent.PatientConsentPolicy = (MobiusPatientConsentPolicy)(XmlSerializerHelper.DeserializeObject(Convert.ToString(row["Consent"]), typeof(MobiusPatientConsentPolicy)));


                                    PatientConsent.MPIID = MPIID;
                                    PatientConsent.PatientConsentID = patientConsentID;

                                    if (row["PermissionID"] != DBNull.Value)
                                        PatientConsent.Permission = row["PermissionID"].ToString();

                                    if (row["PurposeOfUseId"] != DBNull.Value)
                                        PatientConsent.PurposeOfUseId = Convert.ToInt32(row["PurposeOfUseId"]);

                                    //if (row["CategoryID"] != DBNull.Value)
                                    //    PatientConsent.C32SectionId = Convert.ToInt32(row["CategoryID"]);

                                    //if (row["Code"] != DBNull.Value)
                                    //    PatientConsent.Code = row["Code"].ToString();

                                    if (row["RoleId"] != DBNull.Value)
                                        PatientConsent.RoleId = Convert.ToInt32(row["RoleId"]);

                                    if (row["RuleStartDate"] != DBNull.Value)
                                        PatientConsent.RuleStartDate = row["RuleStartDate"].ToString();

                                    if (row["RuleEndDate"] != DBNull.Value)
                                        PatientConsent.RuleEndDate = row["RuleEndDate"].ToString();

                                    //if (row["Status"] != DBNull.Value)
                                    //    PatientConsent.Status = Convert.ToBoolean(row["Status"]);

                                    if (row["Active"] != DBNull.Value)
                                        PatientConsent.Active = Convert.ToBoolean(row["Active"]);

                                    //if (row["Permission"] != DBNull.Value)
                                    //    PatientConsent.Permission = row["Permission"].ToString();

                                    //PatientConsents.Add(PatientConsent);
                                }
                                result.IsSuccess = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;

        }
        //public Result GetPatientConsentByConsentId(string MPIID, int patientConsentID, out List<MobiusPatientConsent> PatientConsents)
        //{
        //    #region variables
        //    Result result = null;
        //    PatientConsents = null;
        //    #endregion

        //    try
        //    {
        //        MobiusPatientConsent PatientConsent = null;
        //        DataTable dtConsent = null;
        //        PatientConsents = new List<MobiusPatientConsent>();
        //        result = new Result();
        //        //Modified for issue id #138
        //        using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetSpecificPatientConstent"))
        //        {
        //            _dataAccessManager.AddInParameter(dbCommand, "@MPIId", DbType.String, MPIID);
        //            _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.Int32, patientConsentID);

        //            //Modified for issue id #138
        //            using (DataSet dsPatientConsent = _dataAccessManager.ExecuteDataSet(dbCommand))
        //            {

        //                if (dsPatientConsent.Tables.Count > 0)
        //                {
        //                    dtConsent = dsPatientConsent.Tables[0];
        //                    if (dtConsent.Rows.Count > 0)
        //                    {

        //                        foreach (DataRow row in dtConsent.Rows)
        //                        {
        //                            PatientConsent = new MobiusPatientConsent();
        //                            if (row["Role"] != DBNull.Value)
        //                                PatientConsent.Role = row["Role"].ToString();

        //                            if (row["PatientConsentID"] != DBNull.Value)
        //                                PatientConsent.PatientConsentID = Convert.ToInt32(row["PatientConsentID"]);

        //                            if (row["PurposeOfUseId"] != DBNull.Value)
        //                                PatientConsent.PurposeOfUseId = Convert.ToInt32(row["PurposeOfUseId"]);

        //                            //if (row["CategoryID"] != DBNull.Value)
        //                            //    PatientConsent.C32SectionId = Convert.ToInt32(row["CategoryID"]);

        //                            if (row["Code"] != DBNull.Value)
        //                                PatientConsent.Code = row["Code"].ToString();

        //                            if (row["RoleId"] != DBNull.Value)
        //                                PatientConsent.RoleId = Convert.ToInt32(row["RoleId"]);

        //                            if (row["RuleStartDate"] != DBNull.Value)
        //                                PatientConsent.RuleStartDate = row["RuleStartDate"].ToString();

        //                            if (row["RuleEndDate"] != DBNull.Value)
        //                                PatientConsent.RuleEndDate = row["RuleEndDate"].ToString();

        //                            //if (row["Status"] != DBNull.Value)
        //                            //    PatientConsent.Status = Convert.ToBoolean(row["Status"]);

        //                            if (row["Active"] != DBNull.Value)
        //                                PatientConsent.Active = Convert.ToBoolean(row["Active"]);

        //                            if (row["Permission"] != DBNull.Value)
        //                                PatientConsent.Permission = row["Permission"].ToString();

        //                            PatientConsents.Add(PatientConsent);
        //                        }
        //                        result.IsSuccess = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsSuccess = false;
        //        this.Result.SetError(ErrorCode.UnknownException, ex.Message);
        //    }
        //    return result;
        //}
        #endregion

        #region DeletePatientConsent
        /// <summary>
        /// Delete Patient Consent
        /// </summary>
        /// <param name="patientConsentId"></param>
        /// <returns>return Result class object</returns>
        public Result DeletePatientConsent(string patientConsentId)
        {
            int returnValue = 0;
            Result result = null;
            try
            {
                result = new Result();
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("DeletePatientConsent"))
                {

                    _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.Int32, patientConsentId);
                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    if (returnValue == 1)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }

        #endregion

        #region UpdateOptInStatus
        /// <summary>
        /// Update OptIn and OptOut Status in dataBase
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="isOptIn"></param>
        /// <returns>return Result class object</returns>
        public Result UpdateOptInStatus(string MPIID, bool isOptIn)
        {
            Result result = null;
            int returnValue = 0;
            try
            {
                result = new Result();
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateOptInStatus"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, MPIID);
                    _dataAccessManager.AddInParameter(dbCommand, "@isOptIn", DbType.String, isOptIn);

                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    if (returnValue >= 1)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return result;
        }
        #endregion

        #region GetSpecificPatientConsent
        //public Result GetSpecificPatientConsent(string MPIID, int PatientConsentId, out DataSet dsSpecificPatientConsent)
        //{
        //    dsSpecificPatientConsent = null;
        //    Result result = null;
        //    try
        //    {
        //        result = new Result();
        //        //Modified for issue id #138
        //        using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetSpecificPatientConstent"))
        //        {
        //            _dataAccessManager.AddInParameter(dbCommand, "@MPIId", DbType.String, MPIID);
        //            _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.Int32, PatientConsentId);
        //            dsSpecificPatientConsent = _dataAccessManager.ExecuteDataSet(dbCommand);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsSuccess = false;
        //        this.Result.SetError(ErrorCode.UnknownException, ex.Message);
        //    }
        //    return result;
        //}
        #endregion

        #region UpdatePatientConsentPolicy
        /// <summary>
        /// Add and Update Patient Consent
        /// </summary>
        /// <param name="patientConsentPolicy"></param>
        /// <param name="ConsentId"></param>
        /// <returns>return Result class object</returns>
        public Result UpdatePatientConsentPolicy(MobiusPatientConsent patientConsentPolicy)
        {
            Result result = null;
            int returnValue = 0;
            int ConsentId = 0;
            try
            {
                result = new Result();
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdatePatientConsentPolicy"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "PatientConsentId", DbType.Int32, patientConsentPolicy.PatientConsentID);
                    _dataAccessManager.AddInParameter(dbCommand, "IsNew", DbType.Boolean, patientConsentPolicy.PatientConsentID <= 0 ? true : false);
                    _dataAccessManager.AddInParameter(dbCommand, "MPIID", DbType.String, patientConsentPolicy.MPIID);

                    _dataAccessManager.AddInParameter(dbCommand, "RoleID", DbType.Int32, patientConsentPolicy.RoleId);
                    _dataAccessManager.AddInParameter(dbCommand, "PermissionID", DbType.Int32, patientConsentPolicy.Permission);

                    _dataAccessManager.AddInParameter(dbCommand, "Consent", DbType.Xml, Mobius.CoreLibrary.XmlSerializerHelper.SerializeObject(patientConsentPolicy.PatientConsentPolicy));

                    _dataAccessManager.AddInParameter(dbCommand, "PurposeOfUseId", DbType.Int32, patientConsentPolicy.PurposeOfUseId);
                    _dataAccessManager.AddInParameter(dbCommand, "Allow", DbType.Int32, patientConsentPolicy.Allow);
                    _dataAccessManager.AddInParameter(dbCommand, "Active", DbType.Int32, patientConsentPolicy.Active);

                    //_dataAccessManager.AddInParameter(dbCommand, "status", DbType.Int32, patientConsentPolicy.Status);
                    _dataAccessManager.AddInParameter(dbCommand, "ruleStartDate", DbType.Date, patientConsentPolicy.RuleStartDate);
                    _dataAccessManager.AddInParameter(dbCommand, "ruleEndDate", DbType.Date, patientConsentPolicy.RuleEndDate);
                    _dataAccessManager.AddOutParameter(dbCommand, "ConsentId", DbType.Int32, ConsentId);

                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    ConsentId = Convert.ToInt32(dbCommand.Parameters["ConsentId"].Value);
                    patientConsentPolicy.PatientConsentID = ConsentId;
                    if (returnValue != 0)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                patientConsentPolicy = null;
            }
            return result;
        }
        #endregion

        #region CheckPatientConsentPolicyExistence
        /// <summary>
        /// Check Patient Consent Existence in Database.
        /// </summary>
        /// <param name="patientConsent"></param>
        /// <returns>return Result class object</returns>
        public Result CheckPatientConsentPolicyExistence(MobiusPatientConsent patientConsent)
        {
            Result result = null;
            int returnValue = 0;
            int checkConsentExistence = 0;
            try
            {
                result = new Result();
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("CheckPatientConsentPolicyExistence"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, patientConsent.MPIID);
                    _dataAccessManager.AddInParameter(dbCommand, "@RoleID", DbType.Int32, patientConsent.RoleId);
                    _dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, patientConsent.PurposeOfUseId);
                    _dataAccessManager.AddOutParameter(dbCommand, "@RecordExists", DbType.Int16, checkConsentExistence);
                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    checkConsentExistence = Convert.ToInt32(dbCommand.Parameters["@RecordExists"].Value);
                    if (checkConsentExistence == 1)
                    {
                        result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region GetPatientInformationviaDocumentID
        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="Patients"></param>
        /// <returns></returns>
        public Result GetPatientInformationByDocumentID(string DocumentID, out Patient patient)
        {
            DataSet dsPatient = null;
            Result result = null;
            patient = null;

            try
            {
                result = new Result();
                result.IsSuccess = true;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientInformation"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentID", DbType.String, DocumentID);
                    dsPatient = _dataAccessManager.ExecuteDataSet(dbCommand);
                }

                if (dsPatient.Tables.Count > 0)
                {
                    patient = new Patient();

                    if (dsPatient.Tables[0].Rows.Count > 0)
                    {

                        if (dsPatient.Tables[0].Rows[0]["MPIID"] != DBNull.Value)
                        {
                            patient.LocalMPIID = dsPatient.Tables[0].Rows[0]["MPIID"].ToString();
                        }
                        if (dsPatient.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                        {
                            patient.DOB = ReversdateFormatter(dsPatient.Tables[0].Rows[0]["DOB"].ToString());
                        }
                        if (dsPatient.Tables[0].Rows[0]["Gender"] != DBNull.Value)
                        {
                            patient.Gender = (Mobius.CoreLibrary.Gender)Enum.Parse(typeof(Mobius.CoreLibrary.Gender), dsPatient.Tables[0].Rows[0]["Gender"].ToString(), true);
                        }
                        patient.GivenName = new List<string>();
                        if (dsPatient.Tables[0].Rows[0]["GivenName"] != DBNull.Value)
                        {
                            patient.GivenName.Add(dsPatient.Tables[0].Rows[0]["GivenName"].ToString());
                        }
                        if (dsPatient.Tables[0].Rows[0]["FamilyName"] != DBNull.Value)
                        {
                            patient.FamilyName.Add(dsPatient.Tables[0].Rows[0]["FamilyName"].ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region AddPFXCertificate
        /// <summary>
        ///  Add PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public Result AddPFXCertificate(PFXCertificate pFXCertificate)
        {
            DbCommand dbCommand;
            Result result = null;
            int Count = 0;
            try
            {
                result = new Result();

                dbCommand = _dataAccessManager.GetStoredProcCommand("AddPFXCertificate");
                _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.Int32, pFXCertificate.UserType.GetHashCode());
                _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, pFXCertificate.EmailAddress);
                _dataAccessManager.AddInParameter(dbCommand, "@PFXCertificate", DbType.String, pFXCertificate.Certificate);
                Count = _dataAccessManager.ExecuteNonQuery(dbCommand);

                if (Count > 0)
                {
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region GetPFXCertificate
        /// <summary>
        /// Get PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public Result GetPFXCertificate(ref PFXCertificate pFXCertificate)
        {
            DbCommand dbCommand;
            Result result = null;
            DataSet ds = null;
            try
            {
                result = new Result();
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPFXCertificate");
                _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.Int32, pFXCertificate.UserType.GetHashCode());
                _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, pFXCertificate.EmailAddress);
                ds = _dataAccessManager.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0)
                {
                    pFXCertificate = new PFXCertificate();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["PFXCertificate"] != DBNull.Value)
                            {
                                result.IsSuccess = true;
                                pFXCertificate.Certificate = row["PFXCertificate"].ToString();
                            }
                        }
                    }
                    if (!result.IsSuccess)
                    {
                        result.SetError(ErrorCode.Certificate_Data_Missing);
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region GetUserDetials
        /// <summary>
        /// Get list of UserDetials into data base Patient and provider table
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public Result GetUserDetials(ref List<UserDetails> userDetails)
        {
            DbCommand dbCommand;
            Result result = null;
            DataSet ds = null;

            UserDetails userDetail = null;
            try
            {
                result = new Result();
                if (userDetails != null && userDetails.Count > 0)
                {
                    dbCommand = _dataAccessManager.GetStoredProcCommand("GetUserDetials");
                    _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.Int32, userDetails[0].UserType.GetHashCode());
                    _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, userDetails[0].EmailAddress);
                    ds = _dataAccessManager.ExecuteDataSet(dbCommand);
                }
                if (ds.Tables.Count > 0)
                {
                    userDetails = new List<UserDetails>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result.IsSuccess = true;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            userDetail = new UserDetails();
                            if (row["SerialNumber"] != DBNull.Value)
                            {
                                userDetail.SerialNumber = row["SerialNumber"].ToString();
                            }
                            if (row["FamilyName"] != DBNull.Value)
                            {
                                userDetail.FamilyName = row["FamilyName"].ToString();
                            }
                            if (row["GivenName"] != DBNull.Value)
                            {
                                userDetail.GivenName = row["GivenName"].ToString();
                            }
                            if (row["CityName"] != DBNull.Value)
                            {
                                userDetail.City = row["CityName"].ToString();
                            }
                            if (row["StateName"] != DBNull.Value)
                            {
                                userDetail.State = row["StateName"].ToString();
                            }
                            if (row["PostalCode"] != DBNull.Value)
                            {
                                userDetail.PostalCode = row["PostalCode"].ToString();
                            }
                            if (row["Status"] != DBNull.Value)
                            {
                                userDetail.Status = row["Status"].ToString();
                            }
                            if (row["EmailAddress"] != DBNull.Value)
                            {
                                userDetail.EmailAddress = row["EmailAddress"].ToString();
                            }
                            userDetails.Add(userDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region GetProviderDetails
        /// <summary>
        ///  Get Provider Details by MedicalRecordsDeliveryEmailAddress
        /// </summary>
        /// <param name="email"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Result GetProviderDetails(string email, out Mobius.Entity.Provider provider)
        {

            DataSet providerData = null;
            Result result = new Result();
            provider = null;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetProviderDetails"))
                {

                    if (!string.IsNullOrEmpty(email))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, email);
                    }
                    result.IsSuccess = true;
                    providerData = DataAccessManager.GetInstance.ExecuteDataSet(dbCommand);

                    if (providerData != null || providerData.Tables.Count > 0)
                    {
                        if (providerData.Tables[0] != null)
                        {
                            provider = new Mobius.Entity.Provider();
                            foreach (DataRow row in providerData.Tables[0].Rows)
                            {
                                if (row["OrganizationName"] != DBNull.Value)
                                {
                                    provider.OrganizationName = row["OrganizationName"].ToString();
                                }
                                if (row["ProviderFirstName"] != DBNull.Value)
                                {
                                    provider.FirstName = row["ProviderFirstName"].ToString();
                                }
                                if (row["ProviderLastName"] != DBNull.Value)
                                {
                                    provider.LastName = row["ProviderLastName"].ToString();
                                }
                                if (row["MedicalRecordsDeliveryEmailAddress"] != DBNull.Value)
                                {
                                    provider.MedicalRecordsDeliveryEmailAddress = row["MedicalRecordsDeliveryEmailAddress"].ToString();
                                }
                                if (row["IndividualProvider"] != DBNull.Value)
                                {
                                    provider.IndividualProvider = Convert.ToBoolean(row["IndividualProvider"]);
                                }
                                provider.City = new City();
                                if (row["CityName"] != DBNull.Value)
                                {
                                    provider.City.CityName = row["CityName"].ToString();
                                }
                                provider.City.State = new State();
                                if (row["StateName"] != DBNull.Value)
                                {
                                    provider.City.State.StateName = row["StateName"].ToString();
                                }
                                provider.City.State.Country = new Country();
                                if (row["CountryName"] != DBNull.Value)
                                {
                                    provider.City.State.Country.CountryName = row["CountryName"].ToString();
                                }
                                if (row["CertificateSerialNumber"] != DBNull.Value)
                                {
                                    provider.CertificateSerialNumber = row["CertificateSerialNumber"].ToString();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region ActivateUser
        /// <summary>
        /// ActivateUser
        /// </summary>
        /// <param name="email"></param>
        /// <param name="serialNumber"></param>
        /// <param name="publicKey"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public Result ActivateUser(string email, string serialNumber, string publicKey, UserType userType, string CreatedOn, string ExpiryOn)
        {


            Result result = new Result();
            Int64 activationCount = 0;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("ActivateUser"))
                {

                    _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, email);
                    _dataAccessManager.AddInParameter(dbCommand, "@SerialNumber", DbType.String, serialNumber);
                    _dataAccessManager.AddInParameter(dbCommand, "@PublicKey", DbType.String, publicKey);
                    _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.String, userType.GetHashCode());
                    _dataAccessManager.AddInParameter(dbCommand, "@CreatedOn", DbType.DateTime, CreatedOn);
                    _dataAccessManager.AddInParameter(dbCommand, "@ExpiryOn", DbType.DateTime, ExpiryOn);
                    activationCount = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (activationCount > 0)
                    {
                        result.IsSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion


        #region UpgradeUser
        /// <summary>
        /// ActivateUser
        /// </summary>
        /// <param name="email"></param>
        /// <param name="serialNumber"></param>
        /// <param name="publicKey"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public Result UpgradeUser(string email, string serialNumber, string publicKey, UserType userType, string CreatedOn, string ExpiryOn)
        {


            Result result = new Result();
            Int64 activationCount = 0;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("ActivateUser"))
                {

                    _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, email);
                    _dataAccessManager.AddInParameter(dbCommand, "@SerialNumber", DbType.String, serialNumber);
                    _dataAccessManager.AddInParameter(dbCommand, "@PublicKey", DbType.String, publicKey);
                    _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.String, userType.GetHashCode());
                    _dataAccessManager.AddInParameter(dbCommand, "@CreatedOn", DbType.DateTime, CreatedOn);
                    _dataAccessManager.AddInParameter(dbCommand, "@ExpiryOn", DbType.DateTime, ExpiryOn);
                    activationCount = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (activationCount > 0)
                    {
                        result.IsSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region privartemethod
        private string ReversdateFormatter(string date)
        {
            string correctDate = string.Empty;
            string[] dt = date.Split('/');
            if (dt.Length > 0)
            {
                if (dt[0].Length == 1 && dt[0].Length != 4)
                {
                    dt[0] = "0" + dt[0].ToString();
                }
                if (dt[1].Length == 1 && dt[1].Length != 4)
                {
                    dt[1] = "0" + dt[1].ToString();
                }
                if (dt[2].Length == 1 && dt[2].Length != 4)
                {
                    dt[2] = "0" + dt[2].ToString();
                }
                date = dt[0] + "/" + dt[1] + "/" + dt[2];
            }
            if (date.Length == 10)
            {
                string year = date.Substring(6, 4);
                string month = date.Substring(0, 2);
                string dd = date.Substring(3, 2);
                correctDate = year + month + dd;
            }
            if (date.Length == 8)
            {
                string year = date.Substring(4, 4);
                string month = "0" + date.Substring(0, 1);
                string dd = "0" + date.Substring(2, 1);
                correctDate = year + month + dd;
            }
            return correctDate;

        }
        #endregion
    }
}
