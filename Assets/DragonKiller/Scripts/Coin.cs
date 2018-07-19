using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class Coin : MonoBehaviour {

        [Tooltip("Coin value")]
        public int CoinValue;
        [Tooltip("SFX when coin drop to the ground")]
        public AudioClip CoinSFX;
        [Tooltip("Shows when coin destroyed after a while")]
        public GameObject CoinFX;
        [Tooltip("Delay after coin hit ground")]
        public float DestroyDelay;

        private AudioSource audioS;
        private bool isAdded = false;
        private Rigidbody2D _Rb2d;

        void Start () {
            audioS = GetComponent<AudioSource>();
            _Rb2d = GetComponent<Rigidbody2D>();
            audioS.clip = CoinSFX;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1.0f, 0) * 200.0f);
        }

        void Update () {
        
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if(col.transform.tag == "Ground" && !isAdded)
            {
                audioS.Play();
                isAdded = true;
                GameCoins.Instance.AddCoin(CoinValue);
                _Rb2d.gravityScale = -1.0f;
                StartCoroutine(DelayDestroy());
            }
        }

        IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(DestroyDelay);
            Destroy(gameObject);
            Instantiate(CoinFX, transform.position, transform.rotation);
        }
    }
}