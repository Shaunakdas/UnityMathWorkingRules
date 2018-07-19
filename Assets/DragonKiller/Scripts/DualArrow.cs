using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class DualArrow : MonoBehaviour {

        void Start () {
            foreach(Transform child in GetComponentsInChildren<Transform>())
            {
                if(child.name == "Arrow")
                    child.parent = null;
            }
            Destroy(gameObject);
        }
        
        void Update () {
        
        }
    }
}