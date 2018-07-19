using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
    public class EnemyAttack : MonoBehaviour {
        
        /*
            Methods in this script is called in animation.
            When the dragon ready to attack, this will start StartAttack.
            After a while, the dragon launch the attack and MoveAttack will started.
            If dragon is killed when getting ready to attack before it launch the attack, it will destroy it attack and cancel the attack
        */

        private GameObject Attack;
        private Transform AttackPoint;
        private GameObject AttackObject;

        void Start () {
        
        }
        
        void Update () {
        
        }
        
        public void SetAttack(GameObject Attack, Transform AttackPoint)
        {
            this.Attack = Attack;
            this.AttackPoint = AttackPoint;
        }

        void StartAttack()
        {
            AttackObject = Instantiate(Attack, AttackPoint.position, AttackPoint.rotation) as GameObject;
            AttackObject.transform.parent = AttackPoint;
        }

        void MoveAttack()
        {
            AttackObject.transform.parent = null;
            AttackObject.GetComponent<DragonAttack>().enabled = true;
            CameraFollow.Instance.Zoomout();
        }

        void DestroyAttack()
        {
            if(AttackObject != null)
                AttackObject.SetActive(false);
        }
    }
}