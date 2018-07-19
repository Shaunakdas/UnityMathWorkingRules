using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class CameraFollow : Singleton<CameraFollow> {

        [Tooltip("Default target to follow. Usually player's character")]
        public Transform Target;
        [Tooltip("Camera offset from the target")]
        public Vector3 Offset;
        [Tooltip("Camera offset when zooming (dragon attacking player).")]
        public Vector3 ZoominOffset;
        [Tooltip("Camera FOV when zooming (dragon attacking player).")]
        public float ZoominFOV = 30.0f;
        
        private bool isFollowing = true;     // is camera following target ?
        private Transform _Default;         // Default target reference
        private Vector3 _DefaultOffset;     // Default offset reference
        private float _DefaultFOV;          // Default FOV reference
        private Camera _Camera;

        // Make an instance of singleton
        void Awake ()
        {
            instance = this;
        }

        void Start () {
            // Find game object with Player tag if target is not defined
            if(Target == null)
            {
                Target = GameObject.FindGameObjectWithTag("Player").transform;
                if(Target == null)
                {
                    Debug.LogError("Player not found");
                }
            }
            
            // Store value for use after changes
            _Default = Target;
            _DefaultOffset = Offset;
            _Camera = GetComponent<Camera>();
            _DefaultFOV = _Camera.fieldOfView;
        }
        
        void Update () {
            // Follow Target
            if(isFollowing && Target != null)
            {
                transform.position = new Vector3(Target.position.x + Offset.x, Target.position.y + Offset.y, transform.position.z + Offset.z);
            }
        }

        public void ChangeTarget(Transform Target)
        {
            this.Target = Target;
        }

        public void ChangeDefault(Transform Default)
        {
            this._Default = Default;
        }

        public void ChangeOffset(Vector3 Offset)
        {
            this.Offset = Offset;
        }

        void ChangePOV(float pov)
        {
            _Camera.fieldOfView = pov;
        }

        public void Zoomin(Transform Target)
        {
            ChangeTarget(Target);
            ChangeOffset(ZoominOffset);
            ChangePOV(ZoominFOV);
        }

        public void Zoomout()
        {
            ChangeTarget(_Default);
            ChangeOffset(_DefaultOffset);
            ChangePOV(_DefaultFOV);
        }

        public void SetFollowing(bool set)
        {
            isFollowing = set;
        }
    }
}