using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class GameController : Singleton<GameController> {

        // This script is used to control game as game manager

        private GameObject Player;
        private RandomSpawner[] Spawner;
        private CameraFollow CamFollow;

        private Character character;
        private float startTime;
        private int iLevelVariation;

		public bool enemyMeeting;

        void Awake()
        {
            instance = this;
            // we want to know how much variants of environment and randomized it
            InfinitySpawner spwnr = (InfinitySpawner)FindObjectOfType(typeof(InfinitySpawner));
            iLevelVariation = Random.Range(0, spwnr.Sprites.Length);
        }

        void Start () {
            // We need some stored value for used later
			StartPlayer();
			enemyMeeting = false;
        }
        
        void Update () {
			if (!enemyMeeting) {
				CheckMeeting ();
			}
        }

		public void StartPlayer(){
			if (GameObject.Find ("Dragon/Main Camera")) {
				CamFollow = GameObject.Find ("Dragon/Main Camera").GetComponent<CameraFollow> ();
			} else {
				CamFollow = GameObject.Find ("Main Camera").GetComponent<CameraFollow> ();
			}
			Spawner = GetComponents<RandomSpawner>();
		}

        public void UpdatePlayer()
        {
            // Find Player and enable object pooling spawner after that
            Player = GameObject.FindGameObjectWithTag("Player");
            character = Player.GetComponent<Character>();
            CamFollow.enabled = true;
            foreach(RandomSpawner spwn in Spawner)
            {
                spwn.enabled = false;
            }
        }

        public void StartGame()
        {
            // Show Banner
            AdsManager.Instance.ShowBanner();

            PlayerPrefs.SetInt("Playing", 0);

            // Set timescale to 1 just in case level loaded after pause
            Time.timeScale = 1.0f;

            // Start player and camera follow
            CamFollow.SetFollowing(true);
            character.Running(true);
            foreach (RandomSpawner spwn in Spawner)
            {
                spwn.enabled = true;
            }

            // Disable some UI after game start
            GUIController.instance.GameStartGUI();
        }

        // Stop Spawning objects
        public void StopSpawning()
        {
            foreach (RandomSpawner spwn in Spawner)
            {
                spwn.StopAllCoroutines();
            }
        }

        // We want to clear enemies and projectiles after player resume game
        public void ClearGame()
        {
            GameObject[] TempStuff = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject stuff in TempStuff)
            {
                Destroy(stuff);
            }

            GameObject[] Projectiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach(GameObject projectile in Projectiles)
            {
                Destroy(projectile);
            }
        }

        // Make enemies run after paused
        public void ResumeGame()
        {
            GameObject[] TempStuff = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject stuff in TempStuff)
            {
                stuff.GetComponent<Enemy>().StartRunning();
            }
        }

        public void RevivePlayer()
        {
            Player.SetActive(true);
            character.Revive();
            character.CanAim(true);
            character.Reload();
            character.SetDead(false);
            foreach (RandomSpawner spwn in Spawner)
            {
                spwn.enabled = false;
                spwn.ResetInverval();
            }
        }

        public void PlayerAim()
        {
            character.StartAim();
        }

        public void PlayerAttack()
        {
            character.ReleaseAim();
        }

        public int GetLevelVariationIndex()
        {
            return iLevelVariation;
        }

		public void CheckMeeting(){
			if (Spawner [0].GameObjectList.Count> 0 ) {
				if ((Spawner [0].GameObjectList.ToArray () [0].transform.position.x - character.gameObject.transform.position.x) < 8.0f) {
					enemyMeeting = true;
					enemyMeeted ();
				}
			}
		}

		public void enemyMeeted(){
			Spawner [0].GameObjectList.ToArray () [0].GetComponent<Enemy> ().StopRunning ();
			character.StopRunning ();
			PlayerAim ();
			StartCoroutine(WaitAndAttack());
		}

		// Spawn randomly
		IEnumerator WaitAndAttack()
		{
			yield return new WaitForSeconds(0.5f);
			character.keepAim ();
			MathTrigger.Instance.NextPara();
		}

		public void characterReachTarget(Vector3 target){
			CamFollow.enabled = false;
			character.ReachByRunning (target);
		}
		public void enemyReachTarget(GameObject _targetGO){
			Spawner [0].GameObjectList.ToArray () [0].GetComponent<Enemy> ().ReachByRunning (_targetGO);
		}
    }
}