#region namesapcelist
using System.Runtime.Serialization;
using FirstGenesis.Mobius.Common;
#endregion

namespace FirstGenesis.Mobius.Server.MobiusServiceLibrary
{
  
        #region DataContract

    
    #region Person Authentication DC starts

    [DataContract]
        public class Authenticate
        {
            [DataMember]
            ErrorCode _errorCode;

            public ErrorCode errorCode
            {
                get { return _errorCode; }
                set { _errorCode = value; }
            }
            [DataMember]
            string _userName;

            public string UserName
            {
                get { return _userName; }
                set { _userName = value; }
            }
            [DataMember]
            string _hashPassWord;

            public string HashPassWord
            {
                get { return _hashPassWord; }
                set { _hashPassWord = value; }
            }

        }
        #endregion
        
        #region Person DC starts
        
        [DataContract]
        public class Person
        {
            public Person()
            { }
            public Person(Person P)
            {
                Given = P.Given;
                Family = P.Family;
                FacilityID = P.FacilityID;
                SSN = P.SSN;
                Gender = P.Gender;
                DOB = P.DOB;
                MobiusID = P.MobiusID;
            }
            [DataMember]
            string _given;
            public string Given
            {
                get { return _given; }
                set { _given = value; }
            }

            [DataMember]
            string _family;
            public string Family
            {
                get { return _family; }
                set { _family = value; }
            }

            [DataMember]
            string _facilityID;
            public string FacilityID
            {
                get { return _facilityID; }
                set { _facilityID = value; }
            }
 
            [DataMember]
            string _SSN;
            public string SSN
            {
                get { return _SSN; }
                set { _SSN = value; }
            }

            [DataMember]
            string _DOB;
            public string DOB
            {
                get { return _DOB; }
                set { _DOB = value; }
            }

            [DataMember]
            string _gender;
            public string Gender
            {
                get { return _gender; }
                set { _gender = value; }
            }

            [DataMember]
            string _mobiusID;

            public string MobiusID
            {
                get { return _mobiusID; }
                set { _mobiusID = value; }
            }

            [DataMember]
            string _MPI_ID;
            

            public string MPI_ID
            {
                get { return _MPI_ID; }
                set { _MPI_ID = value; }
            }
            [DataMember]
            string _docId;

            public string DocId
            {
                get { return _docId; }
                set { _docId = value; }
            }

            [DataMember]
            string _Guid;
            public string Guid
            {
                get { return _Guid; }
                set { _Guid = value; }
            }



        }
        // Person DC ends.
        #endregion

   
        #endregion
    
}
