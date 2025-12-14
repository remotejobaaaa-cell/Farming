using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump.Api;

public class Consent_Form : MonoBehaviour
{
    private bool consentFormShown = false;  // Track if the form has been shown

    void Start()
    {
        Invoke(nameof(InitiliazeConsent), 0.2f);
    }
    public void InitiliazeConsent()
    {
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                // put your test device id
                ""
            }
        };

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = debugSettings,
            };

            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }
    }
    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            if (AdsManager.instance)
            {

#if UNITY_ANDROID



#endif
            }
            Debug.LogError("Consent update error: " + consentError.Message);
            return;
        }

        if (ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required &&
            ConsentInformation.ConsentStatus != ConsentStatus.Obtained && !consentFormShown)
        {
            consentFormShown = true;  // Ensure the form shows only once
            Debug.Log("Showing Consent Form...");
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                if (formError != null)
                {
                    Debug.LogError("Form error: " + formError.Message);
                    return;
                }

                if (ConsentInformation.CanRequestAds())
                {
                    if (AdsManager.instance)
                    {

#if UNITY_ANDROID

#endif
                    }
                    Debug.Log("Consent obtained, ads can be requested.");
                    // Initialize ads here if needed
                }
            });
        }
        else
        {
            if (AdsManager.instance)
            {

#if UNITY_ANDROID



#endif
            }
            Debug.Log("Consent is not required or already obtained.");
        }
    }
}
