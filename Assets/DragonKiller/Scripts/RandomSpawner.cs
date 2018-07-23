﻿using UnityEngine;
using System.Collections;
using Rezero;
using System.Collections.Generic;

namespace Rezero
{
    public class RandomSpawner : MonoBehaviour {

        [Tooltip("Object variants that will spawned")]
        public GameObject[] Object;
        [Tooltip("Spawn point where object spawned")]
        public Transform SpawnPoint;
        [Tooltip("Interval each spawn")]
        public float Interval;
        [Tooltip("Y offset minimum (bottom) for the object that spawned")]
        public Vector2 OffsetMin;
        [Tooltip("Y offset maximum (up) for the object that spawned")]
        public Vector2 OffsetMax;

		public List<GameObject> GameObjectList;

        private float BaseInterval;

		void Start () {
			GameObjectList = new List<GameObject> ();
            BaseInterval = Interval;
            StartSpawn();
        }
        
        void Update () {
        
        }

        public void StartSpawn()
        {
            StartCoroutine(Spawning());
        }

        public void ResetInverval()
        {
            Interval = BaseInterval;
        }

        // Spawn randomly
        IEnumerator Spawning()
        {
            float xPos = Random.Range(OffsetMin.x, OffsetMax.x);
            float yPos = Random.Range(OffsetMin.y, OffsetMax.y);
            Vector3 nextSpawn = SpawnPoint.position + new Vector3(xPos, yPos);
			GameObjectList.Add(Instantiate(Object[Random.Range(0, Object.Length)], nextSpawn, SpawnPoint.rotation));
            yield return new WaitForSeconds(Interval);
            if(Interval > 1.0f)
            {
                Interval -= 0.1f;
            }
            StartCoroutine(Spawning());
        }
    }
}