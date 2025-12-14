using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    public SettingsPopup settingsPopup;

    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    Button mainButton;
    public SettingsMenuItem[] menuItems;

    //is menu opened or not
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    void Awake()
    {
        //add all the items to the menuItems array
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            // +1 to ignore the main button
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        //SetAsLastSibling () to make sure that the main button will be always at the top layer
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;

        //set all menu items position to mainButtonPosition
        ResetPositions();
    }

    private void OnEnable()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
            //Fade to alpha=0
            menuItems[i].img.color = new Color(menuItems[i].img.color.r, menuItems[i].img.color.g, menuItems[i].img.color.b, 0);
            var textMesh = menuItems[i].transform.GetChild(0).GetComponent<Text>();
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
        }
        isExpanded = false;
    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
            //Fade to alpha=0
            menuItems[i].img.DOFade(0f, collapseFadeDuration);
            menuItems[i].transform.GetChild(0).GetComponent<Text>().DOFade(0f, collapseFadeDuration);
        }
    }

    void ToggleMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
                menuItems[i].transform.GetChild(0).GetComponent<Text>().DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        else
        {
            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                //Fade to alpha=0
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
                menuItems[i].transform.GetChild(0).GetComponent<Text>().DOFade(0f, collapseFadeDuration);
            }
        }

        //rotate main button arround Z axis by 180 degree starting from 0
        mainButton.transform
              .DORotate(Vector3.forward * 180f, rotationDuration)
              .From(Vector3.zero)
              .SetEase(rotationEase);
    }

    public void OnItemClick(int index)
    {
        settingsPopup.ChangeQuality(index);
        ToggleMenu();
    }

    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        mainButton.onClick.RemoveListener(ToggleMenu);
    }
}
