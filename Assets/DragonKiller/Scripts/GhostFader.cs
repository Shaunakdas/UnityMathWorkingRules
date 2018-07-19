using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class GhostFader : MonoBehaviour {

        // Fading objects for ghost-effect
        public float StartAlpha = 1.0f;
        private float a;

        void Start () {
            a = StartAlpha;
        }
        
        void Update () {
        
        }

        public void Fade(float val)
        {
            a = StartAlpha;
            StartCoroutine(StartFading(val));
        }

        IEnumerator StartFading(float val)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.1f);
            a -= val;
            StartCoroutine(StartFading(val));
        }
    }
}