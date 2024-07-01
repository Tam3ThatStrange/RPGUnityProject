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
        [SerializeField] float waypointDwellTime = 3.0f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

         Fighter fighter;
		GameObject player;
        Health health;
        Mover mover;
        Vector3 guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArivedAtWaypoint = Mathf.Infinity;
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

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPostion = guardLocation;
            if (patrolPath != null)
            {
                if (AtWayypoint())
                {
                    timeSinceArivedAtWaypoint = 0.0f;
                    CycleWaypoint();
                }
                nextPostion = GetCurrentWaypoint();
            }


            if (timeSinceArivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPostion, patrolSpeedFraction);
            }
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
            timeSinceLastSawPlayer = 0.0f;
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
