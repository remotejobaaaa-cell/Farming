using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GleyLocalization;

public class SettingsPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public GameObject changeControlSubPopup;
    public GameObject changeLanguageSubPopup;

    [Header("Toggles")]
    public Toggle bgmToggle;
    public Toggle soundFxToggle;
    public Toggle mirrorToggle;
    [Header("Controls")]
    public Image controlsTypeButton;
    public Sprite[] controlsTypeSprite;
    public Text controlsTypeText;
    public Text layoutIndexText;
    public GameObject nextLayoutBtn;
    public GameObject previousLayoutBtn;
    [Header("Buttons")]
    public Image buttonsControl;
    public Sprite[] buttonsControlSprite;
    public GameObject[] buttonControlLayouts;
    private int buttonControlLayoutIndex;
    [Header("Steering")]
    public Image steeringControl;
    public Sprite[] steeringControlSprite;
    public GameObject[] steeringControlLayouts;
    private int steeringControlLayoutIndex;
    [Header("Tilt")]
    public Image tiltControl;
    public Sprite[] tiltControlSprite;
    public GameObject[] tiltControlLayouts;
    private int tiltControlLayoutIndex;
    [Header("Localization")]
    public Text currentLocalizeLanguageText;
    public Image localizeFlagImg;
    public Sprite[] localizeFlagSprite;
    public LanguageFlagButton[] languageFlags;
    [Header("Quality")]
    public Text currentQualityText;
    public int currentQualityIndex;
    [Header("Game Volume")]
    public Slider gameVolumeSlider;
    [Header("Camera Sensitivity")]
    public Slider cameraSensitivitySlider;

    private void OnEnable()
    {
        Initialization();
    }

    private void Initialization()
    {
        bgmToggle.isOn = mainMenuManager.playerData.isBackgroundMusicOn;
        soundFxToggle.isOn = mainMenuManager.playerData.isSoundFxOn;
        mirrorToggle.isOn = mainMenuManager.playerData.isMirrorOn;
        InitilizeControls();
        InitilizeQuality();
        InitilizeLanguage();
        gameVolumeSlider.value = mainMenuManager.playerData.gameVolume;
        cameraSensitivitySlider.value = mainMenuManager.playerData.cameraSensitivity;
    }

    private void InitilizeLanguage()
    {
        currentLocalizeLanguageText.text = Manager.GetText(WordIDs.CurrentLanguageID);
        foreach(LanguageFlagButton flagButton in languageFlags)
        {
            flagButton.HighlightSelectedLanguage(false);
        }

        if (Manager.GetCurrentLanguage() == SupportedLanguages.English)
        {
            localizeFlagImg.sprite = localizeFlagSprite[0];
            languageFlags[0].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Chinese)
        {
            localizeFlagImg.sprite = localizeFlagSprite[1];
            languageFlags[1].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.French)
        {
            localizeFlagImg.sprite = localizeFlagSprite[2];
            languageFlags[2].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.German)
        {
            localizeFlagImg.sprite = localizeFlagSprite[3];
            languageFlags[3].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Hindi)
        {
            localizeFlagImg.sprite = localizeFlagSprite[4];
            languageFlags[4].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Indonesian)
        {
            localizeFlagImg.sprite = localizeFlagSprite[5];
            languageFlags[5].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Spanish)
        {
            localizeFlagImg.sprite = localizeFlagSprite[6];
            languageFlags[6].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Italian)
        {
            localizeFlagImg.sprite = localizeFlagSprite[7];
            languageFlags[7].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Japanese)
        {
            localizeFlagImg.sprite = localizeFlagSprite[8];
            languageFlags[8].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Korean)
        {
            localizeFlagImg.sprite = localizeFlagSprite[9];
            languageFlags[9].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Arabic)
        {
            localizeFlagImg.sprite = localizeFlagSprite[10];
            languageFlags[10].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Portuguese)
        {
            localizeFlagImg.sprite = localizeFlagSprite[11];
            languageFlags[11].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Russian)
        {
            localizeFlagImg.sprite = localizeFlagSprite[12];
            languageFlags[12].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Turkish)
        {
            localizeFlagImg.sprite = localizeFlagSprite[13];
            languageFlags[13].HighlightSelectedLanguage(true);
        }
        else if (Manager.GetCurrentLanguage() == SupportedLanguages.Ukrainian)
        {
            localizeFlagImg.sprite = localizeFlagSprite[14];
            languageFlags[14].HighlightSelectedLanguage(true);
        }
        InitilizeDynamicLocalization();
    }

    private void InitilizeDynamicLocalization()
    {
        if (mainMenuManager.playerData.controlsType == ControlsType.Buttons)
        {
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (buttonControlLayoutIndex + 1);
        }
        else if (mainMenuManager.playerData.controlsType == ControlsType.Steering)
        {
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (steeringControlLayoutIndex + 1);
        }
        else
        {
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (tiltControlLayoutIndex + 1);
        }

        if (mainMenuManager.playerData.quality == Quality.Low)
        {
            currentQualityText.text = Manager.GetText(WordIDs.LowID);
        }
        else if (mainMenuManager.playerData.quality == Quality.Medium)
        {
            currentQualityText.text = Manager.GetText(WordIDs.MediumID);
        }
        else
        {
            currentQualityText.text = Manager.GetText(WordIDs.HighID);
        }
    }

    private void InitilizeControls()
    {
        if (mainMenuManager.playerData.controlsType == ControlsType.Buttons)
        {
            //controlsTypeText.text = "Buttons";
            controlsTypeText.text = Manager.GetText(WordIDs.ButtonsID);
            buttonControlLayoutIndex = mainMenuManager.playerData.buttonControlLayoutIndex;
            LayoutToggle(buttonControlLayoutIndex, buttonControlLayouts);
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (buttonControlLayoutIndex + 1);
            nextLayoutBtn.SetActive(true);
            previousLayoutBtn.SetActive(true);
            controlsTypeButton.sprite = controlsTypeSprite[0];
            buttonsControl.sprite = buttonsControlSprite[1];
            steeringControl.sprite = steeringControlSprite[0];
            tiltControl.sprite = tiltControlSprite[0];
        }
        else if (mainMenuManager.playerData.controlsType == ControlsType.Steering)
        {
            //controlsTypeText.text = "Steering";
            controlsTypeText.text = Manager.GetText(WordIDs.SteeringID);
            steeringControlLayoutIndex = mainMenuManager.playerData.steeringControlLayoutIndex;
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (steeringControlLayoutIndex + 1);
            LayoutToggle(steeringControlLayoutIndex, steeringControlLayouts);
            nextLayoutBtn.SetActive(true);
            previousLayoutBtn.SetActive(true);
            controlsTypeButton.sprite = controlsTypeSprite[1];
            buttonsControl.sprite = buttonsControlSprite[0];
            steeringControl.sprite = steeringControlSprite[1];
            tiltControl.sprite = tiltControlSprite[0];
        }
        else
        {
            //controlsTypeText.text = "Tilt";
            controlsTypeText.text = Manager.GetText(WordIDs.TiltID);
            tiltControlLayoutIndex = mainMenuManager.playerData.tiltControlLayoutIndex;
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (tiltControlLayoutIndex + 1);
            LayoutToggle(tiltControlLayoutIndex, tiltControlLayouts);
            nextLayoutBtn.SetActive(false);
            previousLayoutBtn.SetActive(false);
            controlsTypeButton.sprite = controlsTypeSprite[2];
            buttonsControl.sprite = buttonsControlSprite[0];
            steeringControl.sprite = steeringControlSprite[0];
            tiltControl.sprite = tiltControlSprite[1];
        }
    }

    private void LayoutToggle(int index, GameObject[] layouts)
    {
        foreach (GameObject g in buttonControlLayouts)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in steeringControlLayouts)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in tiltControlLayouts)
        {
            g.SetActive(false);
        }
        layouts[index].SetActive(true);
    }

    public void OnClick_NextLayout()
    {
        if (mainMenuManager.playerData.controlsType == ControlsType.Buttons)
        {
            buttonControlLayoutIndex++;
            if (buttonControlLayoutIndex >= buttonControlLayouts.Length)
                buttonControlLayoutIndex = 0;
            mainMenuManager.playerData.buttonControlLayoutIndex = buttonControlLayoutIndex;
            //layoutIndexText.text = "Layout " + (buttonControlLayoutIndex + 1);
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (buttonControlLayoutIndex + 1);
            LayoutToggle(buttonControlLayoutIndex, buttonControlLayouts);
        }
        else if (mainMenuManager.playerData.controlsType == ControlsType.Steering)
        {
            steeringControlLayoutIndex++;
            if (steeringControlLayoutIndex >= steeringControlLayouts.Length)
                steeringControlLayoutIndex = 0;
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (steeringControlLayoutIndex + 1);
            LayoutToggle(steeringControlLayoutIndex, steeringControlLayouts);
            mainMenuManager.playerData.steeringControlLayoutIndex = steeringControlLayoutIndex;
        }
    }

    public void OnClick_PreviousLayout()
    {
        if (mainMenuManager.playerData.controlsType == ControlsType.Buttons)
        {
            buttonControlLayoutIndex--;
            if (buttonControlLayoutIndex < 0)
                buttonControlLayoutIndex = buttonControlLayouts.Length - 1;
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (buttonControlLayoutIndex + 1);
            LayoutToggle(buttonControlLayoutIndex, buttonControlLayouts);
            mainMenuManager.playerData.buttonControlLayoutIndex = buttonControlLayoutIndex;
        }
        else if (mainMenuManager.playerData.controlsType == ControlsType.Steering)
        {
            steeringControlLayoutIndex--;
            if (steeringControlLayoutIndex < 0)
                steeringControlLayoutIndex = steeringControlLayouts.Length - 1;
            layoutIndexText.text = Manager.GetText(WordIDs.LayoutID) + " " + (steeringControlLayoutIndex + 1);
            LayoutToggle(steeringControlLayoutIndex, steeringControlLayouts);
            mainMenuManager.playerData.steeringControlLayoutIndex = steeringControlLayoutIndex;
        }
    }

    public void OnClick_ChangeLocalization(LanguageFlagButton language)
    {
        Manager.SetCurrentLanguage(language.languages);
        InitilizeLanguage();
        InitilizeControls();
    }

    private void InitilizeQuality()
    {
        if (mainMenuManager.playerData.quality == Quality.Low)
        {
            //currentQualityText.text = "Low";
            currentQualityText.text = Manager.GetText(WordIDs.LowID);
        }
        else if (mainMenuManager.playerData.quality == Quality.Medium)
        {
            //currentQualityText.text = "Medium";
            currentQualityText.text = Manager.GetText(WordIDs.MediumID);
        }
        else
        {
            //currentQualityText.text = "High";
            currentQualityText.text = Manager.GetText(WordIDs.HighID);
        }
    }

    private void OnDisable()
    {
        changeControlSubPopup.SetActive(false);
        changeLanguageSubPopup.SetActive(false);
        SetSound();
        PlayerDataController.instance.Save();

    }
    public void SetSound()
    {
        mainMenuManager.playerData.isBackgroundMusicOn =bgmToggle.isOn;
        mainMenuManager.playerData.isSoundFxOn = soundFxToggle.isOn;
        mainMenuManager.playerData.isMirrorOn = mirrorToggle.isOn;
        mainMenuManager.playerData.gameVolume = gameVolumeSlider.value;
        if (AudioManager.instance)
        {
            AudioManager.instance.backgroundMusicAudioSource.volume = mainMenuManager.playerData.gameVolume;
        }
        mainMenuManager.playerData.cameraSensitivity = cameraSensitivitySlider.value;
    }
    public void OnClick_Close()
    {
        SetSound();
        mainMenuManager.OnClick_HandleBackMenu();
    }

    public void OnClick_ChangeControls(int type)
    {
        if (type == 0)
        {
            mainMenuManager.playerData.controlsType = ControlsType.Buttons;
        }
        else if (type == 1)
        {
            mainMenuManager.playerData.controlsType = ControlsType.Steering;
        }
        else
        {
            mainMenuManager.playerData.controlsType = ControlsType.Tilt;
        }
        InitilizeControls();
    }

    public void ChangeQuality(int quality)
    {
        switch (quality)
        {
            case 0:
                mainMenuManager.playerData.quality = Quality.Low;
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                mainMenuManager.playerData.quality = Quality.Medium;
                QualitySettings.SetQualityLevel(3);
                break;
            case 2:
                mainMenuManager.playerData.quality = Quality.High;
                QualitySettings.SetQualityLevel(5);
                break;
        }
        InitilizeQuality();
    }

    public void OnClick_NextQuality()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        currentQualityIndex++;
        if (currentQualityIndex >= 3)
        {
            currentQualityIndex = 0;
        }
        switch (currentQualityIndex)
        {
            case 0:
                mainMenuManager.playerData.quality = Quality.Low;
                QualitySettings.SetQualityLevel(0);
                currentQualityText.text = "Low";
                break;
            case 1:
                mainMenuManager.playerData.quality = Quality.Medium;
                QualitySettings.SetQualityLevel(3);
                currentQualityText.text = "Medium";
                break;
            case 2:
                mainMenuManager.playerData.quality = Quality.High;
                QualitySettings.SetQualityLevel(5);
                currentQualityText.text = "High";
                break;
        }
    }
    public void OnClick_PreviousQuality()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        currentQualityIndex--;
        if (currentQualityIndex < 0)
        {
            currentQualityIndex = 2;
        }
        switch (currentQualityIndex)
        {
            case 0:
                mainMenuManager.playerData.quality = Quality.Low;
                QualitySettings.SetQualityLevel(0);
                currentQualityText.text = "Low";
                break;
            case 1:
                mainMenuManager.playerData.quality = Quality.Medium;
                QualitySettings.SetQualityLevel(3);
                currentQualityText.text = "Medium";
                break;
            case 2:
                mainMenuManager.playerData.quality = Quality.High;
                QualitySettings.SetQualityLevel(5);
                currentQualityText.text = "High";
                break;
        }
    }

    public void ChangeBGM(bool value, Toggle toggle)
    {
        if (toggle == bgmToggle)
        {
            mainMenuManager.playerData.isBackgroundMusicOn = value;
            if (value == true)
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play_MenuBackgroundMusic();
                    AudioManager.instance.backgroundMusicAudioSource.volume = PlayerDataController.instance.playerData.gameVolume;
                }
            }
            else
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.backgroundMusicAudioSource.Stop();
                }
            }
        }
        else if (toggle == soundFxToggle)
        {
            mainMenuManager.playerData.isSoundFxOn = value;
        }
        else if (toggle == mirrorToggle)
        {
            mainMenuManager.playerData.isMirrorOn = value;
        }
    }

    public void OnClick_OpenChangeControlsSubPopup()
    {
        changeControlSubPopup.SetActive(true);
    }

    public void OnClick_ChangeLanguage()
    {
        changeLanguageSubPopup.SetActive(true);
    }
}