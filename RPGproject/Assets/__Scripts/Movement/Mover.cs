
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.core;

namespace RPG.Movement
{
    public class  Mover : MonoBehaviour , IAction
    {
        [SerializeField]
        public Transform target;

        NavMeshAgent navAgent;
        // Ray lastRay;



        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();

        }

        void Update()
        {

            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);

        }

        public void MoveTo(Vector3 destination)
        {
            navAgent.destination = destination;
            navAgent.isStopped = false;
        }

        public void Stop()
        {
            navAgent.isStopped = true;
        }
        public void Cancel()
        { }
        private void UpdateAnimator()
        {
            Vector3 velocity = navAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

    }
}
