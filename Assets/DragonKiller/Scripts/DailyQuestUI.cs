using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class DailyQuestUI : MonoBehaviour {

        public Text questText;
        public GameObject questReward;

        void Start () {
            // Show the reward button if quest completed
            questText.text = DailyQuest.Instance.GetQuestName();
            if(DailyQuest.Instance.IsCompleted())
            {
                questReward.SetActive(true);
            }
        }
        
        void Update () {
        
        }
    }
}