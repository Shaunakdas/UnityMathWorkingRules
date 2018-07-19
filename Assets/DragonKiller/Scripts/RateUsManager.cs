using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class RateUsManager : Singleton<RateUsManager>
    {

        public GameObject RateUsUI;
        public int NumberOfLevelPlayedToShowRateUs = 10;
        public string AndroidURL = "http://Rezero.com";

        void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void Yes()
        {
#if UNITY_ANDROID
            Application.OpenURL(AndroidURL);
#endif

            PlayerPrefs.SetInt("NumberOfLevelPlayedToShowRateUs", -1);
            PlayerPrefs.Save();
            HideRateUs();
        }

        public void Later()
        {
            PlayerPrefs.SetInt("NumberOfLevelPlayedToShowRateUs", 0);
            PlayerPrefs.Save();
            HideRateUs();
        }

        public void Never()
        {
            PlayerPrefs.SetInt("NumberOfLevelPlayedToShowRateUs", -1);
            PlayerPrefs.Save();
            HideRateUs();
        }

        public void CheckIfPromptRateDialogue()
        {
            int count = PlayerPrefs.GetInt("NumberOfLevelPlayedToShowRateUs", 0);

            if (count > NumberOfLevelPlayedToShowRateUs)
            {
                ShowRateUs();
            }
            else if(count != -1)
            {
                count++;
                PlayerPrefs.SetInt("NumberOfLevelPlayedToShowRateUs", count);
            }
        }

        public void ShowRateUs()
        {
            RateUsUI.SetActive(true);
        }

        void HideRateUs()
        {
            RateUsUI.SetActive(false);
        }
    }
}