using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class SkillManager : Singleton<SkillManager> {

        public GameObject SkillPanel;
        public GameObject CutInImage;

        private bool usingSkill = false;

        void Awake ()
        {
            instance = this;
        }

        void Start () {

        }
        
        void Update () {
        
        }

        public void UsingSkill(Sprite CutIn, float PauseTime)
        {
            DailyQuest.instance.Ultimate();
            SkillPanel.SetActive(true);
            SkillPanel.GetComponent<Animator>().SetTrigger("Skill");
            CutInImage.GetComponent<Image>().sprite = CutIn;
            usingSkill = true;
            Time.timeScale = 0f;
            usingSkill = false;
            
        }

        public bool getUsingSkill()
        {
            return usingSkill;
        }
    }
}