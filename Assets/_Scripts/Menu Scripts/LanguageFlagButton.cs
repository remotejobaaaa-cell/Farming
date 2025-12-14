using UnityEngine;

public class LanguageFlagButton : MonoBehaviour
{
    public GameObject selectedImg;
    public SupportedLanguages languages;

    public void HighlightSelectedLanguage(bool value)
    {
        selectedImg.SetActive(value);
    }
}
