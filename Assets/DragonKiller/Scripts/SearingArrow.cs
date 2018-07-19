using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class SearingArrow : MonoBehaviour {

        // Scripts for archer ultimate that spawn a lot of arrow between interval
        public GameObject[] Arrows;
        public float Interval;

        private int i = 0;

        void Start () {
            StartCoroutine(Searing());
        }
        
        void Update () {
        
        }

        IEnumerator Searing()
        {
            if(i == Arrows.Length - 1)
            {
                StopAllCoroutines();
            }
            Arrows[i].SetActive(true);
    
            yield return new WaitForSeconds(Interval);
            i++;
            StartCoroutine(Searing());
        }
    }
}