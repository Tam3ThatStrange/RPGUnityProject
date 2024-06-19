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

        [SerializeField] PatrolPath patrolPath;   
        [SerializeField] float waypointTolerance = 1f;   
         Fighter fighter;
		GameObject player;
        Health health;
        Mover mover;
        Vector3 guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;

		void Start()
		{
			fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
			player = GameObject.FindGameObjectWithTag("Player");
            mover = GetComponent<Mover>();

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
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPostion = guardLocation;
            if(patrolPath != null)
            {
                if (AtWayypoint())
                {
                    CycleWaypoint();
                }
                nextPostion = GetCurrentWaypoint();
            }
            mover.StartMoveAction(nextPostion);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position , GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
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
