using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class DragonAttack : MonoBehaviour {

        [Tooltip("The speed of fireball")]
        [Range(5f, 20f)]
        public float Speed = 10f;
        [Tooltip("Particles that show when fireball hit player")]
        public GameObject HitFX;

        private Transform _Target;
        private Character character;

        void Start () {
            _Target = GameObject.FindGameObjectWithTag("Player").transform;
            character = _Target.GetComponent<Character>();
        }
        
        void Update () {
            transform.position = Vector3.MoveTowards(transform.position, _Target.position, Speed * Time.deltaTime);
        }

        void OnTriggerEnter2D (Collider2D col)
        {
            if(col.transform.tag == "Player")
            {
                character.Dead();
                col.gameObject.SetActive(false);
                Instantiate(HitFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}