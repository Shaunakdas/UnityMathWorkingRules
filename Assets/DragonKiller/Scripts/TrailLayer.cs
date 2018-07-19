using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailLayer : MonoBehaviour {

        private TrailRenderer trail;

        void Start () {
            trail = GetComponent<TrailRenderer>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            trail.sortingLayerID = spriteRenderer.sortingLayerID;
            trail.sortingOrder = spriteRenderer.sortingOrder;
        }
        
        void Update () {
        
        }
    }
}