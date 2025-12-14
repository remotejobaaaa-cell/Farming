using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{
    public Image img;
    public RectTransform rectTrans;

    //SettingsMenu reference
    public SettingsMenu settingsMenu;

    //item button
    Button button;

    //index of the item in the hierarchy
    int index;

    void Awake()
    {
        //-1 to ignore the main button
        index = rectTrans.GetSiblingIndex() - 1;

        //add click listener
        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }

    void OnItemClick()
    {
        settingsMenu.OnItemClick(index);
    }

    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        button.onClick.RemoveListener(OnItemClick);
    }
}
