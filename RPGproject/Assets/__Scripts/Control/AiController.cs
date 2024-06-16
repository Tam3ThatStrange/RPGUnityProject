using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Control
{
	public class AiController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float suspicionTime = 3.0f;

        Fighter fighter;
		GameObject player;
        Health health;
        Mover mover;
        Vector3 guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;

		void Start()
		{
			fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
			player = GameObject.FindGameObjectWithTag("Player");

            guardLocation = gameObject.transform.position;
		}

		private void Update()
        {
            if (health.IsDead()) return;


            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0.0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardLocation);
        }

        private void SuspicionBehaviour()
        {

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
