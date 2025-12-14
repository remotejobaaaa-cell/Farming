using System.Collections.Generic;
public class GameConstants
{
    public const string moreGamesURL = "https://play.google.com/store/apps/dev?id=5025055235544243089";
    public const string companyYoutube = "https://www.youtube.com/channel/UCNmZK8Ubq_3hJrY7bmq5T6w?sub_confirmation=1";
    public const string companyInstagaram = "https://www.instagram.com/mrsikandarali";
    public const string gameInstagaram = "https://www.instagram.com/mrsikandarali";
    public const string companyTwitter = "https://twitter.com/MrSikandarAli";
    public const string companyFacebook = "https://www.facebook.com/MrSikandarAli";
    public const string companyLinkedIn = "https://www.linkedin.com/in/sikandar-ali-2035b612a/";


    public static int currentlySelectedMap;
    public static int currentlySelectedLevel = 0;
    public static int adCounter = 0;
    public static string currentlySelectedLevelDescription = "Description";
    public static int currentlySelectedFarmer;
    public static int currentlySelectedWheel;
    public static int currentlySelectedSkin;
    public static int noAds=0;

    public static string currentTerminalsName;

    public static GameplayWeather gameplayWeather;
    public static Dictionary<int, float> foodToServe = new Dictionary<int, float>();

    public static float baseFare;
    public static float minFare;
    public static float maxFare;
    public static float farePercentToIncrease;
    public static float decidedFare;

    public static float currentSittingBooked;

    public static bool isRegionAnimationPlayed = false;

    public static bool isPlayerWin = false;
    public static bool openLevels = false;


}
