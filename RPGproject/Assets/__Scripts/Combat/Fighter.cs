
using UnityEngine;
using RPG.Movement;
using Unity.VisualScripting;
using RPG.Core;
using RPG.core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 10f;
        [SerializeField] float timeBetweemAttacks = 1.0f;

        [SerializeField] float weaponDamage = 5f;

        Transform target;
        float timeSinceLastAttack = 0.0f;
       
       private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
                AttackBehaviour();
            }



        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweemAttacks)
            {
                // This will Trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0.0f;
              
            }
        }
        // Animation Event
        void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }


        public void Attack(CombatTarget combatTarget)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
           
        }
        public void Cancel()
        { 
            target = null; 
        }


     

    }
    
}
