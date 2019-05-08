using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class Character : MonoBehaviour {

        [System.Serializable]
        public class AimingObject
        {
            public GameObject Object;
            public float MaxRotation;
        }
        [Tooltip("Player weapon object (will not active when reloading)")]
        public GameObject Weapon;
        [Tooltip("Normal attack prefab for character")]
        public GameObject NormalAttack;
        [Tooltip("Ultimate attack prefab for character")]
        public GameObject UltimateAttack;
        [Tooltip("Ultimate indicator object (will active when ultimate ready)")]
        public GameObject UltimateIndicator;
        [Tooltip("Spawn point where attack spawned")]
        public Transform SpawnPoint;
        [Tooltip("Object that will rotate when aiming")]
        public GameObject Aimer;
        [Tooltip("SFX when attacking")]
        public AudioClip ReleaseSFX;
        [Tooltip("Particles when player respawned")]
        public GameObject PlayerReviveFX;
        [Tooltip("SFX when reload finished")]
        public AudioClip ReloadSFX;
        [Tooltip("Delay each shoot")]
        public float Delay;

        [Tooltip("Object will rotate when aiming. So body is a littl bit rotate up when aiming")]
        public AimingObject[] Aimings;
        private Vector3[] AimingsBaseRotation;

		private bool canShoot = true;
		private bool isAiming = false;
		private bool isAimingEnemy = false;
		private Enemy enemy;
		public LocationFollower locationFollower;
        private bool isUltimate = false;
        private int UltimateCount = 0;
        private AimerDuplicator aimer;
        private bool canAim = true;
        private AudioSource audioS;

        private Quaternion ultimateRot;

        public float Speed = 100.0f;
        public GameObject Ragdoll;
        public float PauseTime = 2.0f;
        public Sprite CutIn;

        private Animator _anim;

        private bool isDead = false;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            aimer = GetComponentInChildren<AimerDuplicator>();
            audioS = GetComponent<AudioSource>();
        }

        void Start () {
            // Reset aimer to default
            Aimer.SetActive(false);
            AimingsBaseRotation = new Vector3[Aimings.Length];
            for(int i = 0; i < Aimings.Length; i++)
            {
                AimingsBaseRotation[i] = Aimings[i].Object.transform.rotation.eulerAngles;
            }
			if (GetComponent<LocationFollower> ()) {
				locationFollower = GetComponent<LocationFollower> ();
				locationFollower.initiateFields (GUIType.uGUIDragon);
			}
        }

        void Update () {
            // Cheats for ultimate attack
            if (Input.GetKeyDown(KeyCode.F1))
                UltimateReady();

            if(canShoot)
                ShowWeapon();

            // Rotate aiming objects when aiming
            if(isAiming)
            {
                foreach (AimingObject obj in Aimings)
                {
                    if(obj.Object.transform.rotation.eulerAngles.z <= obj.MaxRotation)
                        obj.Object.transform.Rotate(new Vector3(0, 0, obj.MaxRotation / 1.5f * Time.deltaTime));
                }
            }
			// Rotate aiming objects when aiming
			if(isAimingEnemy)
			{
				foreach (AimingObject obj in Aimings)
				{	
					//Setting max rotation of right hand to aim at enemy
					if (obj.Object.gameObject.name == "Right Hand") {
						Vector3 lookPos = enemy.transform.position -  obj.Object.transform.position;
						float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
						obj.MaxRotation = angle;
					}
					if (obj.Object.transform.rotation.eulerAngles.z <= obj.MaxRotation)
						obj.Object.transform.Rotate (new Vector3 (0, 0, obj.MaxRotation / 1.5f * Time.deltaTime));
				}
			}
        }

		public void StartAim()
		{
			if(canShoot && canAim)
			{      
				isAiming = true;
				Aimer.SetActive(true);
				aimer.StartDuplicating();
			}
		}

		public void StartAimEnemy(Enemy _enemy)
		{
			if(canShoot && canAim)
			{      
				isAimingEnemy = true;
				Aimer.SetActive(true);
				aimer.StartDuplicating();
				enemy = _enemy;
			}
		}

        public void ReleaseAim()
        {
            if(isAiming)
            {
                aimer.StopDuplicating();
                Shoot();
            }
        }

		public void keepAim(){
			if(isAiming)
			{
				aimer.StopDuplicating();
			}
		}

        public void CancelAim()
        {
            aimer.StopDuplicating();
            isAiming = false;
            ResetAimers();
            StartCoroutine(Reloading());
        }

        public void Reload()
        {
            canShoot = true;
            PlaySFX(ReloadSFX);
            GUIController.Instance.ReloadingDone();
        }

        public void CanAim(bool can)
        {
            canAim = can;
        }

        public void Headshot()
        {
            UltimateCount++;
            if(UltimateCount == 3)
            {
                UltimateReady();
            }
        }

        public void Bodyshot()
        {
            UltimateCount = 0;
        }

        public void UltiShoot()
        { 
            GameObject attack = Instantiate(UltimateAttack, SpawnPoint.position, ultimateRot) as GameObject;
            if(attack.GetComponent<ProjectileMovement>() != null)
            {
                attack.GetComponent<ProjectileMovement>().setBaseRot(ultimateRot);
                attack.GetComponent<ProjectileMovement>().StartMove();
            } 
        }

        public void Dead()
        {
            DailyQuest.Instance.ResetChain();
            Die();
            isDead = true;
        }

        public void Running(bool run)
        {
            if (run)
            {
                StartRunning();
            }
            else
            {
                StopRunning();
            }
        }

        public void SetDead(bool isdead)
        {
            isDead = isdead;
        }

        public bool IsDead()
        {
            return isDead;
        }

        void Shoot()
        {
            if(isUltimate)
            {
                ultimateRot = SpawnPoint.rotation;
                SkillManager.Instance.UsingSkill(CutIn, PauseTime);
                isUltimate = false;
                UltimateIndicator.SetActive(false);
            }
            else
            {
                GameObject attack = Instantiate(NormalAttack, SpawnPoint.position, SpawnPoint.rotation) as GameObject;
                if (attack.GetComponent<ProjectileMovement>() != null)
                {
                    attack.GetComponent<ProjectileMovement>().StartMove();
                }
            }
            //isAiming = false;
            //ResetAimers();
            //StopAllCoroutines();
            //StartCoroutine(Reloading());
            //HideWeapon();
            //PlaySFX(ReleaseSFX);
        }

        IEnumerator Reloading()
        {
            canShoot = false;
            GUIController.Instance.Reloading(Delay);
            yield return new WaitForSeconds(Delay);
            Reload();
        }

        public void ResetAimers()
        {
            for(int i = 0; i < Aimings.Length; i++)
            {
                Aimings[i].Object.transform.eulerAngles = AimingsBaseRotation[i];
            }
            Aimer.SetActive(false);
            AimerDuplicator[] aimers = GetComponentsInChildren<AimerDuplicator>();
            foreach(AimerDuplicator aimr in aimers)
            {
                aimr.gameObject.SetActive(false);
            }
        }

        public void Revive()
        {
            GameObject ReviveFX = Instantiate(PlayerReviveFX, transform.position - new Vector3(0, 0.65f, 0), transform.rotation) as GameObject;
            ReviveFX.transform.parent = transform;
        }

        void UltimateReady()
        {
            isUltimate = true;
            UltimateCount = 0;
            UltimateIndicator.SetActive(true);
            GUIController.Instance.DisplayHitInfo("ultimate");
        }

        void ShowWeapon()
        {
            Weapon.SetActive(true);
        }

        void HideWeapon()
        {
            Weapon.SetActive(false);
        }

        void PlaySFX(AudioClip clip)
        {
            audioS.clip = clip;
            audioS.Play();
        }

        void StartRunning()
        {
            _anim.SetBool("isRunning", true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(Speed, 0);
        }

        public void StopRunning()
        {
            _anim.SetBool("isRunning", false);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        public void ReachByRunning(GameObject _targetGO, GUIType _gUIType,EventDelegate _nextEvent)
		{
			_anim.SetBool("isRunning", false);
            EventDelegate postDelegate = new EventDelegate(delegate () { _nextEvent.Execute(); Shoot(); locationFollower.startResetingLocation(); });
            locationFollower.setReachingGameobject (_targetGO, _gUIType, true, postDelegate);
			locationFollower.startReachingGameobject ();
		}

        void Die()
        {
            // Show interstitial ads if ready and rate us
            AdsManager.Instance.ShowAdsGameOver();
            RateUsManager.Instance.CheckIfPromptRateDialogue();

            // Disable player and spawn ragdoll when die
            GameController.Instance.ResumeGame();
            isAiming = false;
            Instantiate(Ragdoll, transform.position, transform.rotation);
            GUIController.Instance.OpenLosePanel();
            ResetAimers();
        }
    }
}