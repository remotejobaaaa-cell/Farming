using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchToggle : MonoBehaviour
{
    public SettingsPopup settingsPopup;
    [Header("Either")]
    public GamePauseMenuManager gamePauseMenuManager;

    [SerializeField] RectTransform uiHandleRectTransform;

    public Toggle toggle;

    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private Image handleImage;
    [SerializeField]
    private Sprite[] handleSprites;


    Vector2 handlePosition;

    void Start()
    {
        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
            OnSwitch(true);
    }

    public void OnSwitch(bool on)
    {
        Tweener tweener = uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        tweener.SetUpdate(true);
        if (on)
        {
            backgroundImage.sprite = backgroundSprites[1];
            handleImage.sprite = handleSprites[1];
        }
        else
        {
            backgroundImage.sprite = backgroundSprites[0];
            handleImage.sprite = handleSprites[0];
        }
        if (settingsPopup == null)
        {
            gamePauseMenuManager.ChangeBGM(on, toggle);
        }
        else 
        if (gamePauseMenuManager == null)
        {
            settingsPopup.ChangeBGM(on, toggle);

        }

    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
