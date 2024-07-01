
using RPG.Core;
using RPG.Movement;
using RPGCharacterAnims.Lookups;
using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 10f;
        [SerializeField] float timeBetweemAttacks = 1.0f;

        [SerializeField] float weaponDamage = 5f;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;

        Health target;
        float timeSinceLastAttack = 0.0f;

        private void Start()
        {
            SpawnWeapon();
            
           
        }

      

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Stop();
                AttackBehaviour();
            }



        }
        private void SpawnWeapon()
        {
            print(weaponPrefab);
            Instantiate(weaponPrefab, handTransform);
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweemAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0.0f;
            }
          
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // This will Trigger the Hit() event
            GetComponent<Animator>().SetTrigger("attack");
    

        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {

            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
           
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");

            GetComponent<Animator>().SetTrigger("stopAttack");
        }



    }
    
}
