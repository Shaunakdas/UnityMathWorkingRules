using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rezero;

namespace Rezero
{
    public class AimerDuplicator : MonoBehaviour {

        [Tooltip("How fast ghost-effect aimer spawned after the last one.")]
        [Range(0.01f, 1.0f)]
        public float Interval = 0.075f;
        public int MaxGhost = 10;
        private List<GameObject> Aimers = new List<GameObject>();

        private int i = 0;

        void Start () {
            
        }

        void Update () {
        
        }

        public void StartDuplicating()
        {
            StartCoroutine(Duplicating());
        }

        public void StopDuplicating()
        {
            StopAllCoroutines();
        }

        // Give ghost effect for aimer
        IEnumerator Duplicating()
        {
            GameObject newObj;
            if(Aimers.Count < MaxGhost)
            {
                newObj = Instantiate(this.gameObject);
            }
            else
            {
                newObj = Aimers[i];
            }
            newObj.transform.rotation = transform.rotation;
            newObj.transform.position = transform.position;
            newObj.transform.parent = transform.parent.transform.parent;
            newObj.SetActive(true);
            newObj.GetComponent<GhostFader>().Fade(0.2f);
            Aimers.Add(newObj);

            i++;
            if(i == MaxGhost)
            {
                i = 0;
            }
            yield return new WaitForSeconds(Interval);
            StartCoroutine(Duplicating());
        }
    }
}