using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileMovement : MonoBehaviour {

        [Tooltip("The power of projectile")]
        [Range(500, 5000)]
        public float Power;
        [Tooltip("Particles when projectile hits")]
        public GameObject Impact;
        [Tooltip("True if you want the projectile not destroyed after hits")]
        public bool DontDestroy = false;
        [Tooltip("Link projectile with others. ex: dual arrow that not miss if one of them hit")]
        public bool Linked = false;
        public GameObject LinkedGO;

        private Rigidbody2D rb2d;
        private Quaternion baseRot;
        private bool isLinkedHit = false;

        void Awake ()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        void Start () {
            StartMove();
        }

        public void setBaseRot(Quaternion rot)
        {
            baseRot = rot;
            transform.rotation = baseRot;
        }
        
        void Update () {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.transform.tag == "Enemy" || col.transform.tag == "Weakpoint" || col.transform.tag == "Normalpoint" && !DontDestroy)
            {
                if(Linked && LinkedGO != null)
                    LinkedGO.GetComponent<ProjectileMovement>().SetLinkedHit(true);
                if (!DontDestroy)
                    Destroy(gameObject);
                if (Impact != null)
                {
                    Instantiate(Impact, transform.position, transform.rotation);
                }
            }
            else if (col.transform.tag == "Destroyer" && !DontDestroy)
            {
                if(!Linked)
                {
                    DailyQuest.Instance.ResetChain();
                    GUIController.Instance.DisplayHitInfo("miss");
                }
                else
                {
                    if(!isLinkedHit)
                    {
                        DailyQuest.Instance.ResetChain();
                        GUIController.Instance.DisplayHitInfo("miss");
                    }
                }
                Destroy(gameObject);
            }
        }

        public void StartMove()
        {
            rb2d.AddForce(transform.right * Power);
        }

        public void SetLinkedHit(bool b)
        {
            isLinkedHit = b;
        }
    }
}