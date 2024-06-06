using RPG.Combat;
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

		void Start()
		{
			fighter = GetComponent<Fighter>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		private void Update()
        {



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
    }
}
