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

        void Awake()
        {
            instance = this;
            // we want to know how much variants of environment and randomized it
            InfinitySpawner spwnr = (InfinitySpawner)FindObjectOfType(typeof(InfinitySpawner));
            iLevelVariation = Random.Range(0, spwnr.Sprites.Length);
        }

        void Start () {
            // We need some stored value for used later
            CamFollow = Camera.main.GetComponent<CameraFollow>();
            Spawner = GetComponents<RandomSpawner>();
        }
        
        void Update () {

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
    }
}