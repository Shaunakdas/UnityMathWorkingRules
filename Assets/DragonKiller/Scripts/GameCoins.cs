using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rezero;

namespace Rezero
{
    public class GameCoins : Singleton<GameCoins> {

        [Tooltip("You can use more variance of coins")]
        public GameObject[] coins;
        public Text CoinText;
        private int CurrentCoins;

        void Awake ()
        {
            instance = this;
        }

        void Start () {
            CurrentCoins = PlayerPrefs.GetInt("Coins", 0);
            UpdateCoinText();
        }

        void Update () {
        
        }

        public int GetCoin()
        {
            return CurrentCoins;
        }

        public void AddCoin(int value)
        {
            CurrentCoins += value;
            PlayerPrefs.SetInt("Coins", CurrentCoins);
            UpdateCoinText();
        }

        public void DecreaseCoin(int value)
        {
            CurrentCoins -= value;
            PlayerPrefs.SetInt("Coins", CurrentCoins);
            UpdateCoinText();
        }

        public void SpawnCoin(Transform pos, string type)
        {
            if(type == "Normal")
            {
                Instantiate(coins[Random.Range(0, coins.Length)], pos.position, pos.rotation);
            }
        }

        void UpdateCoinText()
        {
            CoinText.text = CurrentCoins.ToString();
        }
    }
}