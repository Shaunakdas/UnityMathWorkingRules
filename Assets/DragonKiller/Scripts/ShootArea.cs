using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class ShootArea : MonoBehaviour {

        // Collision for enemies

        [Tooltip("true if this collider is enemy weak point")]
        public bool isWeakPoint = false;

        void Start () {
        
        }
        
        void Update () {
        
        }

        void OnTriggerEnter2D (Collider2D col)
        {
            Enemy enemy = GetComponentInParent<Enemy>();
            
            if (col.transform.tag == "Projectile")
            {
                if (isWeakPoint)
                {
                    enemy.NormalShooted();
                }
                else
                {
                    enemy.WeakShooted();
                }
            }
            else if(col.transform.tag == "Ultimate Projectile")
            {
                enemy.UltimateShooted();
            }
        }
    }
}