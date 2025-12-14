using UnityEngine;
using UnityEngine.UI;

public class LevelUIButton : MonoBehaviour
{
    public LevelSelectionMenuManager levelSelection;

    public int id;

    public Image levelImage;
    public Sprite levelSprite;
    [Header("Selected Level")]
    public Image selectedlevelImage;

    [Space(0)]
    public Text levelCountText;
    public GameObject lockedIcon;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (levelSelection.mainMenuManager.playerData.maps[GameConstants.currentlySelectedMap].routes[id].isLocked)
        {
            levelCountText.text = "Locked";
            lockedIcon.SetActive(true);
        }
        else
        {
            lockedIcon.SetActive(false);
            if (id < 9)
            {
                levelCountText.text = "Level 0" + (id + 1);
            }
            else
            {
                levelCountText.text = "Level " + (id + 1);
            }
        }
    }

    public void OnClick_Level(LevelUIButton levelUI)
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        levelSelection.SelectLevel(levelUI);
        levelSelection.LeanArray();
    }
}
