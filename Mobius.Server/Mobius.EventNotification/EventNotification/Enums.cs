using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.EventNotification
{
    /// <summary>
    /// Enumeration Events 
    /// </summary>
    public enum EventType
    {
        PatientReferral = 1,
        ReferralOutcome = 2,
        PatientReferralCompleted = 3,
        ReferralAccepted = 4,
        ReferralDeclined = 5,
        SendPatientDocument = 6,
        RegisterProvider=7,
        RegisterPatient=8,
        PatientDiscovery=9,
        DocumentQuery=10,
        DocumentRetrieval=11,
        RegisterProviderFailed=12,
        RegisterPatientFailed=13,
        PatientDiscoveryFailed=14,
        DocumentQueryFailed=15,
        DocumentRetrievalFailed=16,
        PasswordResetForPatient = 17,
        PasswordResetForProvider = 18,
        UserAccountActivated=19,
        RegenratedUserAccount = 20,
        UpgradeUserAccount=21,
        UpgradeUserAccountFailed = 22,
        EmergencyOverride=23,
        EmergencyOverrideFailed=24

    }


    /// <summary>
    /// Enumeration  of action that can be perform on Event
    /// </summary>
    internal enum ActionType
    { 

        Mail = 1,
        Audit = 2
    }

    
}
