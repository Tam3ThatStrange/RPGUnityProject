using RPG.Combat;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
	public class AiController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5.0f;

		Fighter fighter;
		GameObject player;
        Health health;

		void Start()
		{
			fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		private void Update()
        {
            if (health.IsDead()) return;


            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {

                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }


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
