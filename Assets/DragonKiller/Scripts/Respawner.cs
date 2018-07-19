using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class Respawner : MonoBehaviour {

        [Tooltip("Trigger for call the pooling object to spawn")]
        public GameObject TargetCollide;
        public InfinitySpawner TheSpawner;

        void Start () {

        }
        
        void Update () {
        
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == TargetCollide.tag)
            {
                TheSpawner.SpawnNext();
            }
        }
    }
}