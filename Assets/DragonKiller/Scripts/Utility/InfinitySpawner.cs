using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rezero;

namespace Rezero
{
    public class InfinitySpawner : MonoBehaviour {

        [Tooltip("Initial object where we spawn next objects next to this initial object.")]
        public GameObject InitialObject;
        [Tooltip("Variant sprites that will randomly used when starting the game")]
        public Sprite[] Sprites;
        [Tooltip("Max pooled object. use the minimum count (the lowest that doesnt cut in the game) for higher perfomance")]
        [Range(1, 5)]
        public int MaxObject = 3;

        private GameObject _CurrentObject;
        private List<GameObject> _ObjectPools = new List<GameObject>();
        private int _PoolIndex = 0;

        void Start () {
            // Initiated pooling
            _CurrentObject = InitialObject;
            AssignRandomSprites(_CurrentObject);
            _ObjectPools.Add(_CurrentObject);

            // Initiate max object
            for (int i = 0; i < MaxObject - 1; i++)
            {
                SpawnNext();
            }
        }

        // Spawn the next object after the last one
        public void SpawnNext()
        {
            Vector3 nextPost = new Vector3(_CurrentObject.transform.position.x + _CurrentObject.GetComponent<BoxCollider2D>().size.x, _CurrentObject.transform.position.y, _CurrentObject.transform.position.z);
            if(_ObjectPools.Count == MaxObject)
            {
                _CurrentObject = _ObjectPools[_PoolIndex];
                _CurrentObject.transform.position = nextPost;
                NextPool();
            }
            else
            {
                _CurrentObject = Instantiate(InitialObject, nextPost, InitialObject.transform.rotation) as GameObject;
                _CurrentObject.transform.parent = InitialObject.transform.parent;
                _ObjectPools.Add(_CurrentObject);
            }

            AssignRandomSprites(_CurrentObject);
        }

        // Pooling
        void NextPool()
        {
            _PoolIndex++;
            if(_PoolIndex >= MaxObject)
            {
                _PoolIndex = 0;
            }
        }

        void AssignRandomSprites(GameObject CurrentObject)
        {
            CurrentObject.GetComponent<SpriteRenderer>().sprite = Sprites[GameController.Instance.GetLevelVariationIndex()];
        }
    }
}