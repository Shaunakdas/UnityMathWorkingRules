using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class SelfDestruct : MonoBehaviour {

        [Tooltip("Object will be destroyed for this seconds")]
        public float Lifetime = 1.0f;

        void Start () {
            Destroy(this.gameObject, Lifetime);
        }
    }
}