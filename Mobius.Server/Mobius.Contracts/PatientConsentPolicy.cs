using System.Collections.Generic;

namespace MobiusServiceLibrary
{
    public class PatientConsentPolicy
    {

        List<ModulePermission> _Modules = null;
        public List<ModulePermission> Modules
        {
            get
            {
                return _Modules != null ? _Modules : _Modules = new List<ModulePermission>();
            }
            set
            {
                if (_Modules == null) _Modules = new List<ModulePermission>();
                _Modules = value;
            }
        }

    }


    public class ModulePermission : Consent
    {
        public ModulePermission() { }

        List<Consent> _C32SectionConsent = null;

        public List<Consent> Sections
        {
            get
            {
                return _C32SectionConsent != null ? _C32SectionConsent : _C32SectionConsent = new List<Consent>();
            }
            set
            {
                if (_C32SectionConsent == null) _C32SectionConsent = new List<Consent>();
                _C32SectionConsent = value;
            }
        }
    }


    public class Consent
    {
        public Consent()
        { }
        public int Id { get; set; }
        public bool Allow { get; set; }
    }
}
