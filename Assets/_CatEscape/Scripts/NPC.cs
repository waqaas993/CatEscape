using System;
using UnityEngine;

namespace CatEscape.Game
{
    enum State
    {
        Idle,
        Tracking,
        Following,
        Attacking,
        Waiting
    }
    public class NPC : MonoBehaviour
    {
        public Animator Animator;
        [Header("NPC Properties")]
        [Tooltip("Walking speed of NPC")]
        public float Speed;
        [Tooltip("Time before NPC starts to follow Target")]
        public float ChargeTime;
        [Tooltip("Distance NPC can attack from")]
        public float AttackDistance;

        private GameObject Target;

        private float _CurrentChargeTime;
        private State CurrentState;
        private float CurrentChargeTime { get { return _CurrentChargeTime; } set { _CurrentChargeTime = value; } }

        public static Action<GameObject, GameObject, float> Attacked;

        private void OnEnable()
        {
            HitBox.Trigger += SetTracking;
        }

        private void FixedUpdate()
        {
            TakeAction();
        }

        private void TakeAction()
        {
            if (CurrentState.Equals(State.Idle))
                SitIdle();
            else if (CurrentState.Equals(State.Tracking))
                TrackTarget();
            else if (CurrentState.Equals(State.Following))
                FollowTarget();
            else if (CurrentState.Equals(State.Attacking))
                Attack();
        }

        private void SitIdle()
        {
            CurrentChargeTime = 0;
            Target = null;
            Animator.SetTrigger("idle");
        }
        
        private void TrackTarget()
        {
            CurrentChargeTime += Time.fixedDeltaTime;
            if (CurrentChargeTime > ChargeTime)
                CurrentState = State.Following;
        }

        private void FollowTarget()
        {
            Vector3 direction = Target.transform.position - transform.position;
            direction = direction.normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * Speed * 2);
            transform.Translate(transform.forward * Speed * Time.fixedDeltaTime, Space.World);
            Animator.SetTrigger("follow");

            //Checks if the target is in range to launch an attack
            if (Target)
            {
                if (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance)
                    CurrentState = State.Attacking;
            }
        }

        private void Attack()
        {
            Attacked?.Invoke(gameObject, Target, 100);
            Animator.SetTrigger("attack");
            CurrentState = State.Waiting;
        }

        private void SetTracking(GameObject candidate, Type trigger) 
        {
            if (candidate.CompareTag("Player"))
            {
                if (trigger.Equals(Type.Entered))
                {
                    CurrentState = State.Tracking;
                    Target = candidate;
                }
                if (trigger.Equals(Type.Left))
                {
                    ResetToIdle();
                }
            }

        }


        /// <summary>
        /// This function is specified as an event in NPC attack animation
        /// </summary>
        public void ResetToIdle()
        {
            CurrentState = State.Idle;
        }

        private void OnDisable()
        {
            HitBox.Trigger -= SetTracking;
        }
    }
}