using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rezero;

namespace Rezero
{
    public class GamePoints : Singleton<GamePoints> {

        // Used to store current score and save/load best score
        public Text PointsText;
        private int CurrentPoints;
        private int BestScore;

        void Awake()
        {
            instance = this;
            BestScore = PlayerPrefs.GetInt("BestScore", 0);
        }

        void Start () {
            CurrentPoints = 0;
            UpdatePointText();
        }

        public int GetPoints()
        {
            return CurrentPoints;
        }

        public void ClearPoints()
        {
            CurrentPoints = 0;
            UpdatePointText();
        }

        public void AddPoints(int value)
        {
            CurrentPoints += value;
            if(CurrentPoints > BestScore)
            {
                BestScore = CurrentPoints;
                PlayerPrefs.SetInt("BestScore", BestScore);
                GUIController.instance.UpdateBestScore();
            }
            UpdatePointText();
        }

        void UpdatePointText()
        {
            PointsText.text = CurrentPoints.ToString();
        }

        public int GetBestScore()
        {
            return BestScore;
        }
    }
}