using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour {

        [Tooltip("Enemy Movement Speed")]
        [Range(0.1f, 5.0f)]
        public float Speed = 2.0f;
        [Tooltip("Prefabs for Enemy Attack")]
        public GameObject EnemyAttack;
        [Tooltip("Enemy attack point where EnemyAttack prefabs instantiated")]
        public Transform AttackPoint;
        [Tooltip("Exploison point for physic exploison origin. This should be in the lower left enemy so they will move to the right when die")]
        public Transform ExploisonPoint;
        [Tooltip("Particles that instantiated when enemy die")]
        public GameObject DeadFX;
        [Tooltip("Soundclip that will played when enemy die")]
        public AudioClip DeadSFX;

        private Animator _Anim;
        private Rigidbody2D _Rb2d;
        private bool _IsDead = false;
        private bool _IsAttack = false;
        private AudioSource audioS;

        private Character character;
        private EnemyAttack enemyAttack;

		private bool isReaching = false;
		private GameObject targetGO;

        void Start() {
            audioS = GetComponent<AudioSource>();
            _Anim = GetComponentInChildren<Animator>();
            _Rb2d = GetComponent<Rigidbody2D>();
            enemyAttack = GetComponentInChildren<EnemyAttack>();
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            
            if(Player != null)
            {
                character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            }
            
            if(enemyAttack != null)
            {
                enemyAttack.SetAttack(EnemyAttack, AttackPoint);
            }
            else
            {
                Debug.LogError("Enemy Attack script not found in child. Please add enemy attack script in graphic animation object and call it from animation");
            }
            
            if(_Anim == null)
            {
                Debug.LogError("Animator not found in child");
            }
            StartRunning();
        }

        void Update() {
			if (isReaching) {
				Vector3 targetPos = MathTrigger.Instance.convertNGUIToArrow (targetGO.transform.position);
				if (Vector3.Distance (transform.position, targetPos) > 0.1f) {
					gameObject.transform.position = Vector3.MoveTowards (transform.position, targetPos, 3.0f * Time.deltaTime);
				} else {
//					isReaching = false;
				}
			}
        }

        void Dead()
        {
            //Save some daily quest data
            DailyQuest.Instance.DragonKilled();
            DailyQuest.Instance.ChainKill();
            
            // Instantiate particles and play soundclip
            Instantiate(DeadFX, transform.position, transform.rotation);
            PlaySFX(DeadSFX);
        
            // Set some parameters
            _IsDead = true;
            GetComponentInChildren<SpriteRenderer>().color = Color.gray;
            StopRunning();
            AddExplosionForce(_Rb2d, 300.0f, ExploisonPoint.position, 5.0f);
            _Rb2d.gravityScale = 1.0f;
            
            // Make player can shoot again
            character.Reload();
            character.Running(true);
            character.CanAim(true);
            
            // Zooming out camera if enemy dead when in zoomed in mode (ready to attack player)
            StartCoroutine(WaitZoomout(1.0f));

            // Destroy unused child objects
            foreach(Transform child in transform)
            {
                if(child.tag != "Untagged")
                {
                    Destroy(child.gameObject);
                }
            }

        }

        public void WeakShooted()
        {
            if (!_IsDead && character != null)
            {
                // Save some daily quest data
                DailyQuest.Instance.WeakpointShoot();
                
                // Set dead animation
                _Anim.SetTrigger("Dead");
                
                // Add coins and points
                GamePoints.Instance.AddPoints(2);
                GameCoins.Instance.SpawnCoin(transform, "Normal");
                
                // Display info
                GUIController.Instance.DisplayHitInfo("weak");
                
                // Run another method
                character.Headshot();
                Dead();
            }
        }

        public void NormalShooted()
        {
            if (!_IsDead && character != null)
            {
                // Set dead animation
                _Anim.SetTrigger("Dead");
                
                // Add coins and points
                GamePoints.Instance.AddPoints(1);
                GameCoins.Instance.SpawnCoin(transform, "Normal");
                
                // Display info
                GUIController.Instance.DisplayHitInfo("hit");
                
                // Run another method
                character.Bodyshot();
                Dead();
            }
        }

        public void UltimateShooted()
        {
            if (!_IsDead)
            {
                // Set dead animation
                _Anim.SetTrigger("Dead");
                
                // Add more coins and points
                GamePoints.Instance.AddPoints(3);
                GameCoins.Instance.SpawnCoin(transform, "Ultimate");

                // Display info
                GUIController.Instance.DisplayHitInfo("ultimateshoot");

                // Run another method
                character.Bodyshot();
                Dead();
            }
        }

        void Attack()
        {
            if(!character.IsDead())
            {
                // Zoomin camera
                CameraFollow.Instance.Zoomin(transform);
                
                // Start attacking
                _IsAttack = true;
                StopRunning();
                _Anim.SetTrigger("Attack");
                
                // Set player so they can't attack anymore
                character.Running(false);
                character.CanAim(false);
                character.CancelAim();
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (col.name == "Attack Area" && !_IsDead && !_IsAttack && character != null)
            {
                Attack();
            }
        }

        void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius)
        {
            var dir = (body.transform.position - expPosition);
            float calc = 1 - (dir.magnitude / expRadius);
            if (calc <= 0)
            {
                calc = 0;
            }
            body.AddForce(dir.normalized * expForce * calc);
        }

        void PlaySFX(AudioClip clip)
        {
            audioS.clip = clip;
            audioS.Play();
        }

        public void StartRunning()
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-Speed, 0);
        }

        public void StopRunning()
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        IEnumerator WaitZoomout(float dur)
        {
            yield return new WaitForSeconds(dur);
            CameraFollow.Instance.Zoomout();
        }

		public void ReachByRunning(GameObject target)
		{
			targetGO = target;
			isReaching = true;
		}
    }
}