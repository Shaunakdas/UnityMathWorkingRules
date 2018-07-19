using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class GUIController : Singleton<GUIController> {

        public GameObject Points;
        public GameObject MainMenuPanel;
        public GameObject ShopPanel;
        public GameObject LosePanel;
        public GameObject PauseButton;
        public GameObject PausePanel;
        public GameObject ReviveButton;
        public GameObject Reload;
        public Image ReloadIndicator;
        public Image PauseImage;
        public GameObject HowToImage;
        public Sprite[] PauseIcon;
        public GameObject HitInfo;
        public Transform ShopCamera;
        public Text BestScoreText;
        public AudioSource BGM;
        public AudioClip PlayBGM;

        public Text questName;
        public Text questDesc;
        public GameObject questReward;
        public GameObject questComplete;
        public GameObject dailyQuestPopup;

        private Vector3 MainCameraPosition;
        private bool isPaused = false;
		private bool isGameOver = false;
		private bool isGameStarted = false;
        private IEnumerator ReloadCo;

        void Awake()
        {
            instance = this;
        }

        void Start () {
            // Set and update everything before game start
            Points.SetActive(false);
            MainCameraPosition = Camera.main.transform.position;
            UpdateBestScore();
            UpdateQuest ();
            if (PlayerPrefs.GetInt("OpenShop", 0) == 1)
            {
                OpenShopPanel();
            }
        }
        
        void Update () {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (LosePanel.activeInHierarchy) {
					GoHome ();
				} else if (ShopPanel.activeInHierarchy) {
					CloseShopPanel ();
				} else if (HowToImage.activeInHierarchy) {
					CloseHowToPlay ();
				} else if (isGameStarted) {
					Pause ();
				} else {
					Application.Quit ();
				}
			}
        }

        public void GameStartGUI()
        {
			isGameStarted = true;
            MainMenuPanel.SetActive(false);
            Points.SetActive(true);
            PauseButton.SetActive(true);
            BGM.clip = PlayBGM;
            BGM.Play();
        }

        public void OpenPanel(GameObject panel)
        {
            panel.SetActive(true);
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
        void OpenPausePanel()
        {
            PausePanel.SetActive(true);
        }

        void ClosePausePanel()
        {
            PausePanel.SetActive(false);
        }

        public void OpenShopPanel()
        {
            // Hide banner
            AdsManager.Instance.Hide_Banner();

            Camera.main.GetComponent<CameraFollow> ().SetFollowing(false);
            Camera.main.transform.position = ShopCamera.position;
            ShopPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }

        public void CloseShopPanel()
        {
            Camera.main.GetComponent<CameraFollow> ().enabled = true;
            PlayerPrefs.SetInt("OpenShop", 0);
            Camera.main.transform.position = MainCameraPosition;
            ShopPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }

        public void OpenLosePanel()
        {
			isGameOver = true;
            LosePanel.SetActive(true);
        }

        public void CloseLosePanel()
        {
			isGameOver = false;
            LosePanel.SetActive(false);
        }

        public void GoHome()
        {
            // Hide Banner
            AdsManager.Instance.Hide_Banner();

            Application.LoadLevel(Application.loadedLevel);
        }

        public void GoShop()
        {
            PlayerPrefs.SetInt("OpenShop", 1);
            Application.LoadLevel(Application.loadedLevel);
        }

        public void OpenHowToPlay()
        {
            HowToImage.SetActive(true);
        }

        public void CloseHowToPlay()
        {
            HowToImage.SetActive(false);
        }

        public void Revive()
        {
            AdsManager.Instance.ShowRewardedAd();
        }

        public void RevivePlayer()
        {
            ReviveButton.SetActive(false);
            GameController.instance.ClearGame();
            GameController.instance.RevivePlayer();
            GameController.instance.StartGame();
            CloseLosePanel();
        }

        public void Retry()
        {
            PlayerPrefs.SetInt("Playing", 1);
            GoHome();
        }

        public void UpdateBestScore()
        {
            BestScoreText.text = "Best " + GamePoints.instance.GetBestScore().ToString();
        }

        public void DisplayHitInfo(string hit)
        {
            HitInfo.GetComponent<Animator>().SetTrigger("Display");
            Text hitText = HitInfo.GetComponentInChildren<Text>();
            switch (hit)
            {
                case "miss":
                    hitText.text = "MISS";
                    break;
                case "hit":
                    hitText.text = "HIT +1 Point";
                    break;
                case "weak":
                    hitText.text = "WEAK POINT +2 Points";
                    break;
                case "ultimateshoot":
                    hitText.text = "ULTIMATE +3 Points";
                    break;
                case "ultimate":
                    hitText.text = "ULTIMATE READY !!!";
                    break;
            }
        }

        public void Pause()
        {
            if (isPaused)
            {
                Time.timeScale = 1.0f;
                PauseImage.sprite = PauseIcon[0];
                ClosePausePanel();
            }
			else if(!isPaused && !isGameOver)
            {
                Time.timeScale = 0f;
                PauseImage.sprite = PauseIcon[1];
                OpenPausePanel();
            }
            isPaused = !isPaused;
        }

        public void Reloading(float dur)
        {
            Reload.SetActive(true);
            ReloadCo = ReloadTimer(dur);
            StartCoroutine(ReloadCo);
        }

        public void ReloadingDone()
        {
            Reload.SetActive(false);
            StopCoroutine(ReloadCo);
        }

        IEnumerator ReloadTimer(float dur)
        {
            // Set UI fill amount when reloading
            float timeRemainin = dur;
            while(timeRemainin > 0)
            {
                timeRemainin -= Time.deltaTime;
                ReloadIndicator.fillAmount = Mathf.Lerp(0, 1.0f, Mathf.InverseLerp(dur, 0, timeRemainin));
                yield return null;
            }

            ReloadIndicator.fillAmount = 1.0f;
        }

        public void UpdateQuest()
        {
            // Update Quest UI
            questName.text = DailyQuest.Instance.GetQuestName();
            questDesc.text = DailyQuest.instance.GetQuestDescription();
            questReward.SetActive (false);
            questComplete.SetActive (false);
            // Show reward if quest completed
            if (DailyQuest.Instance.IsCompleted() && PlayerPrefs.GetInt("GetReward", 0) == 0)
            {
                questReward.SetActive(true);
            }
            else if(DailyQuest.instance.IsCompleted())
            {
                questComplete.SetActive(true);
            }
        }

        public void GetQuestReward()
        {
            GameCoins.instance.AddCoin(DailyQuest.instance.GetReward());
            PlayerPrefs.SetInt("GetReward", 1);
            questReward.SetActive(false);
            questComplete.SetActive(true);
        }

        public void ShowQuestPopup()
        {
            dailyQuestPopup.GetComponent<Animator>().enabled = true;
        }
    }
}