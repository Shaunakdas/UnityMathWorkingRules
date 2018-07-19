using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Rezero;

namespace Rezero
{
    public class DailyQuest : Singleton<DailyQuest> {

        public enum QuestType { Kill, Chain, Weakpoint, Ultimate };

        [System.Serializable]
        public class Quest
        {
            public string questName;
            public string questDescription;
            public QuestType questType;
            public int count;
            public int reward;
        }

        [Tooltip("List of available quest that will randomly assigned to daily quest")]
        public Quest[] questList;
        private int iQuest = 0;
        private bool isCompleted = false;

        private int dragonsKilled = 0;
        private int chainKills = 0;
        private int weakpoint = 0;
        private int ultimate = 0;
        private int chainKillTmp = 0;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                DailyQuest.instance.QuestCheck();
                DailyQuest.instance.UpdateQuest();
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this);

            // Get saved status for daily quest
            dragonsKilled = PlayerPrefs.GetInt("DragonKilled", 0);
            chainKills = PlayerPrefs.GetInt("ChainKill", 0);
            weakpoint = PlayerPrefs.GetInt("WeakpointShoot", 0);
            ultimate = PlayerPrefs.GetInt("Ultimate", 0);
        }

        void Start () {
            chainKillTmp = 0;
            QuestCheck();
            UpdateQuest(); 
        }
        
        void Update () {
        
        }

        public void UpdateQuest()
        {
            // Check last time user play the game and compare it with today
            // Refresh Quest if today is more than yesterday
            DateTime now = DateTime.Now;
            string LastDate = PlayerPrefs.GetString("LastDate", now.Date.AddDays(-1).ToString("d"));
            int comp = DateTime.Compare(DateTime.Parse(now.Date.ToString("d")), DateTime.Parse(LastDate));

            if (comp > 0)
            {
                isCompleted = false;
                PlayerPrefs.SetString("LastDate", now.Date.ToString("d"));
                iQuest = UnityEngine.Random.Range(0, questList.Length);
                PlayerPrefs.SetInt("DailyQuest", iQuest);
                RefreshQuest();
            }
            GUIController.instance.UpdateQuest();
        }

        void RefreshQuest()
        {
            Debug.Log("Quest Refreshed");
            dragonsKilled = 0;
            chainKills = 0;
            weakpoint = 0;
            ultimate = 0;
            PlayerPrefs.SetInt("DragonKilled", dragonsKilled);
            PlayerPrefs.SetInt("ChainKill", chainKills);
            PlayerPrefs.SetInt("WeakpointShoot", weakpoint);
            PlayerPrefs.SetInt("Ultimate", ultimate);
            PlayerPrefs.SetInt("GetReward", 0);
            PlayerPrefs.SetInt("QuestPopup", 0);
        }

        public void DragonKilled()
        {
            dragonsKilled++;
            PlayerPrefs.SetInt("DragonKilled", dragonsKilled);
            QuestCheck();
        }

        public void ChainKill()
        {
            chainKillTmp++;
            if(chainKillTmp > chainKills)
            {
                chainKills = chainKillTmp;
            }
            PlayerPrefs.SetInt("ChainKill", chainKills);
            QuestCheck();
        }

        public void ResetChain()
        {
            chainKillTmp = 0;
        }

        public void WeakpointShoot()
        {
            weakpoint++;
            PlayerPrefs.SetInt("WeakpointShoot", weakpoint);
            QuestCheck();
        }

        public void Ultimate()
        {
            ultimate++;
            PlayerPrefs.SetInt("Ultimate", ultimate);
            QuestCheck();
        }

        void QuestCheck()
        {
            iQuest = PlayerPrefs.GetInt("DailyQuest", 0);
            if (dragonsKilled >= questList[iQuest].count && questList[iQuest].questType == QuestType.Kill)
            {
                isCompleted = true;
            }

            if(chainKills >= questList[iQuest].count && questList[iQuest].questType == QuestType.Chain)
            {
                isCompleted = true;
            }

            if(weakpoint >= questList[iQuest].count && questList[iQuest].questType == QuestType.Weakpoint)
            {
                isCompleted = true;
            }

            if(ultimate >= questList[iQuest].count && questList[iQuest].questType == QuestType.Ultimate)
            {
                isCompleted = true;
            }

            if(PlayerPrefs.GetInt("QuestPopup", 0) == 0 && isCompleted)
            {
                GUIController.instance.ShowQuestPopup();
                PlayerPrefs.SetInt("QuestPopup", 1);
            }
        }

        public string GetQuestName()
        {
            return questList[iQuest].questName;
        }

        public string GetQuestDescription()
        {
            string questText = "";
            int currentCount = 0;

            if (questList[iQuest].questType == QuestType.Kill)
                currentCount = dragonsKilled;
            else if (questList[iQuest].questType == QuestType.Chain)
                currentCount = chainKills;
            else if (questList[iQuest].questType == QuestType.Weakpoint)
                currentCount = weakpoint;
            else if (questList[iQuest].questType == QuestType.Ultimate)
                currentCount = ultimate;

            questText = questList[iQuest].questDescription + " (" + currentCount + "/" + questList[iQuest].count + ")";

            return questText;
        }

        public bool IsCompleted()
        {
            return isCompleted;
        }

        public int GetReward()
        {
            return questList[iQuest].reward;
        }
    }
}