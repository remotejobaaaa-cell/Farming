using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashMenuManager : MonoBehaviour
{
    public float delayInLoading;

    [Header("Loading")]
    [Space(2)]
    public GameObject loadingMenu;
    public Slider loadingSlider;
    public Text loadingProgressText;
    public string menuLevelName = "Menu";
    private AsyncOperation ao;
	public GameObject tapToStartPanel;
	public GameObject adLogo;
	void Awake()
    {
        Time.timeScale = 1f;
		if (AudioManager.instance)
        {
            AudioManager.instance.Play_MenuBackgroundMusic();
        }
	}

	private void OnEnable()
	{

#if HAVE_ADS
		if (adLogo)
		{
			if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isAdlimit)
				adLogo.SetActive(true);
			else
				adLogo.SetActive(false);
		}
#else
    if (adLogo)
    {
        adLogo.SetActive(false); // Hide ad logo if ads are not enabled
    }
#endif
	}


	private void Start()
	{
		StartCoroutine(WaitForSdkInitialization());
	}

	IEnumerator WaitForSdkInitialization()
{
		yield return new WaitForSeconds(delayInLoading);// Wait for the next frame and check again

		StartCoroutine(LoadingGameAssets());
}
	public float totalFakeLoadTime; // 20 seconds fake load time

	IEnumerator LoadingGameAssets()
{
	loadingSlider.value = 0f;
	loadingProgressText.text = "";
		loadingMenu.SetActive(true);

	float fakeProgress = 0f;

	while (fakeProgress < totalFakeLoadTime)
	{
		fakeProgress += Time.deltaTime;
		float progress = Mathf.Clamp01(fakeProgress / totalFakeLoadTime);
			loadingSlider.value = progress;
			loadingProgressText.text = " " + (int)(progress * 100) + "%";
		yield return null;
	}
		tapToStartPanel.SetActive(true);
}
	public void LoadMainMenu()
	{
#if HAVE_ADS
		//if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isRemoveAd() && !AdsManager.instance.isAdlimit)
  //      {
		//	AdsManager.instance.ShowInterstitialAd(MethodToCallCorotinue);
		//}
  //      else
  //      {
			MethodToCallCorotinue();
	//	}
#else
			Debug.Log("444444444444");
			MethodToCallCorotinue();
			Debug.Log("555555555555");
#endif
	}

	void MethodToCallCorotinue()
    {
		StartCoroutine(LoadTheLevel());
#if HAVE_ADS
		if (AdsManager.instance)
			AdsManager.instance.LogAnalyticsEvent("tap_to_start");
#endif
	}
		IEnumerator LoadTheLevel()
{
		tapToStartPanel.SetActive(false);
		loadingSlider.value = 0f;
		loadingProgressText.text = "";
		loadingMenu.SetActive(true);

		float fakeProgress = 0f;

		while (fakeProgress < 3)
		{
			fakeProgress += Time.deltaTime;
			float progress = Mathf.Clamp01(fakeProgress / 3);
			loadingSlider.value = progress;
			loadingProgressText.text = " " + (int)(progress * 100) + "%";
			yield return null;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
