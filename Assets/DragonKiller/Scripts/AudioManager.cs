using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class AudioManager : Singleton<AudioManager> {

        // Used for handling bgm & sfx toggle
        // Refer the value from the Audio Mixer
        public AudioMixer mixer;
        public float MuteValue;
        public float BGMValue;
        public float SFXValue;
        [Tooltip("0 for off and 1 for on sprite")]
        public Sprite[] BGMIcon;
        [Tooltip("0 for off and 1 for on sprite")]
        public Sprite[] SFXIcon;

        private int bgm;
        private int sfx;

        void Awake()
        {
            instance = this;
        }

        void Start () {
            bgm = PlayerPrefs.GetInt("BGMMute", 1);
            sfx = PlayerPrefs.GetInt("SFXMute", 1);
            if(bgm == 1)
            {
                SetVolume("BGMvol", BGMValue);
            }
            else
            {
                SetVolume("BGMvol", MuteValue);
            }

            if(sfx == 1)
            {
                SetVolume("SFXvol", SFXValue);
            }
            else
            {
                SetVolume("SFXvol", MuteValue);
            }
        }
        
        void Update () {
        
        }

        public void ToggleBGM(Image theImage)
        {
            if(bgm == 1)
            {
                SetVolume("BGMvol", MuteValue);
                bgm = 0;
                theImage.sprite = BGMIcon[0];
                PlayerPrefs.SetInt("BGMMute", 0);
            }
            else
            {
                SetVolume("BGMvol", BGMValue);
                bgm = 1;
                theImage.sprite = BGMIcon[1];
                PlayerPrefs.SetInt("BGMMute", 1);
            }
        }

        public void ToggleSFX(Image theImage)
        {
            if(sfx == 1)
            {
                SetVolume("SFXvol", MuteValue);
                sfx = 0;
                theImage.sprite = SFXIcon[0];
                PlayerPrefs.SetInt("SFXMute", 0);
            }
            else
            {
                SetVolume("SFXvol", SFXValue);
                sfx = 1;
                theImage.sprite = SFXIcon[1];
                PlayerPrefs.SetInt("SFXMute", 1);
            }
        }

        void SetVolume(string name, float vol)
        {
            mixer.SetFloat(name, vol);
        }
    }
}