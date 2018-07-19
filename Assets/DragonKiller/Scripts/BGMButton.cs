using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class BGMButton : MonoBehaviour {

        [Tooltip("true for BGM and false for SFX")]
        public bool isBGM = true;   // true = BGM, false = SFX
        private int bgm;
        private Image theImage;

        void Start () {
            theImage = GetComponent<Image>();
            // Set the button if its on/off
            if(isBGM)
            {
                bgm = PlayerPrefs.GetInt("BGMMute", 1);

                if (bgm == 1)
                {
                    theImage.sprite = AudioManager.Instance.BGMIcon[1];
                }
                else
                {
                    theImage.sprite = AudioManager.Instance.BGMIcon[0];
                }
            }
            else
            {
                bgm = PlayerPrefs.GetInt("SFXMute", 1);

                if (bgm == 1)
                {
                    theImage.sprite = AudioManager.Instance.SFXIcon[1];
                }
                else
                {
                    theImage.sprite = AudioManager.Instance.SFXIcon[0];
                }
            }
        }
        
        void Update () {
        
        }
    }
}