using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class ShopManager : MonoBehaviour {

        [System.Serializable]
        public class Hero
        {
            public string HeroName;
            public GameObject Preview;
            public GameObject Player;
            public int Cost;
            // public bool hidden = false;
            // public string code; // Code if exclusive
        }

        [Tooltip("Heroes available in the game")]
        public Hero[] Heroes;
        [Tooltip("Heroes button in the shop. Use the same order with heroes")]
        public Image[] HeroButtons;
        public Transform CharPreview;
        public Transform HeroPos;
        public Text ButtonText;
        public GameObject Button;
        public GameObject ButtonCoin;

        private GameObject[] HeroPlayers;
        private GameObject[] Previews;
        private int CurrentHero;
        private int RealHero;

        void Start () {
            Previews = new GameObject[Heroes.Length];
            HeroPlayers = new GameObject[Heroes.Length];
            RealHero = PlayerPrefs.GetInt("Hero", 0);
            PlayerPrefs.SetInt(Heroes[0].HeroName, 1);
            CurrentHero = RealHero;
            InitiatePreviews();
            ChooseCharacter(CurrentHero);
            ChooseHero();
            PlayerPrefs.SetInt(Heroes[0].HeroName, 1);
            
			//DRAGON CHANGES#1 commenting out Starting Game
//            if(PlayerPrefs.GetInt("Playing", 0) == 1)
//            {
//                GameController.Instance.StartGame();
//            }
			GameController.Instance.StartGame();
			//DRAGON CHANGES#1 end
        }
        
        void Update () {
        
        }

        // Show heroes that already owned, silhoutte heroes thats not owned
        void InitiatePreviews()
        {
            for(int i = 0; i < Heroes.Length; i++)
            {
                HeroPlayers[i] = Instantiate(Heroes[i].Player, HeroPos.position, HeroPos.rotation) as GameObject;
                Previews[i] = Instantiate(Heroes[i].Preview, CharPreview.position, CharPreview.rotation) as GameObject;
                if(PlayerPrefs.GetInt(Heroes[i].HeroName, 0) == 1)
                {
                    ShowPreview(Previews[i]);
                }
                else
                {
                    HidePreview(Previews[i]);
                }
            }
        }

        public void ChooseCharacter(int i)
        {
            CurrentHero = i;

            foreach (GameObject p in Previews)
            {
                p.SetActive(false);
            }

            Previews[i].SetActive(true);
            UpdateButton();
        }

        // Use hero if owned, purchase if money is more than the cost
        public void SelectCharacter()
        {
            if(isBought())
            {
                ChooseHero();
                GUIController.Instance.CloseShopPanel();
            }
            else
            {
                if(GameCoins.Instance.GetCoin() >= Heroes[CurrentHero].Cost)
                {
                    GameCoins.Instance.DecreaseCoin(Heroes[CurrentHero].Cost);
                    PlayerPrefs.SetInt(Heroes[CurrentHero].HeroName, 1);
                    ShowPreview(Previews[CurrentHero]);
                }
            }

            UpdateButton();
        }

        // Set the new hero that currently selected
        void ChooseHero()
        {
            PlayerPrefs.SetInt("Hero", CurrentHero);
            RealHero = CurrentHero;

            foreach (GameObject p in HeroPlayers)
            {
                p.SetActive(false);
            }
            HeroPlayers[CurrentHero].SetActive(true);
            CameraFollow.Instance.ChangeTarget(HeroPlayers[CurrentHero].transform);
            CameraFollow.Instance.ChangeDefault(HeroPlayers[CurrentHero].transform);
			GameController.Instance.StartPlayer ();
            GameController.Instance.UpdatePlayer();
        }

        bool isBought()
        {
            if(PlayerPrefs.GetInt(Heroes[CurrentHero].HeroName, 0) == 1)
                return true;
            return false;
        }

        // bool isExclusive()
        // {
        //     if (Heroes[CurrentHero].hidden)
        //         return true;
        //     return false;
        // }

        bool isUsed()
        {
            if (RealHero == CurrentHero)
                return true;
            return false;
        }

        // Other heroes button is slightly darker than the one currently chosen
        void UpdateButton()
        {
            foreach(Image img in HeroButtons)
            {
                img.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            HeroButtons[CurrentHero].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            ButtonCoin.SetActive(false);
            if (isUsed())
            {
                ButtonText.text = "Used";
            }
            else if(isBought())
            {
                ButtonText.text = "Select";
            }
            // else if (isExclusive())
            // {
            //     ButtonText.text = "Exclusive";
            // }
            else
            {
                ButtonText.text = Heroes[CurrentHero].Cost.ToString();
                ButtonCoin.SetActive(true);
            }
        }

        // Reveal hero preview
        void ShowPreview(GameObject preview)
        {
            foreach(SpriteRenderer sr in preview.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = Color.white;
            }
        }

        // Silhoutte hero preview
        void HidePreview(GameObject preview)
        {
            foreach (SpriteRenderer sr in preview.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = Color.black;
            }
        }
    }
}