using System;
using UnityEngine;

namespace CatEscape.Game
{
    public enum State
    {
        Idle,
        Tracking,
        Following,
        Attacking,
        Waiting
    }
    public class Enemy : MonoBehaviour
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
        private State _CurrentState;
        private float CurrentChargeTime { get { return _CurrentChargeTime; } set { _CurrentChargeTime = value; } }
        public State CurrentState { get => _CurrentState; private set { _CurrentState = value; } }

        public static Action<GameObject, GameObject, float> Attacked;
        public static Action<State> StateChanged;

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
            Vector3 direction = Target.transform.position - transform.position;
            direction = direction.normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * Speed * 2);
            CurrentChargeTime += Time.fixedDeltaTime;
            if (CurrentChargeTime > ChargeTime)
            {
                CurrentState = State.Following;
                StateChanged?.Invoke(CurrentState);
            }
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
                {
                    CurrentState = State.Attacking;
                    StateChanged?.Invoke(CurrentState);
                }
            }
        }

        private void Attack()
        {
            Attacked?.Invoke(gameObject, Target, 100);
            Animator.SetTrigger("attack");
            CurrentState = State.Waiting;
            StateChanged?.Invoke(CurrentState);
        }

        private void SetTracking(GameObject candidate, Type trigger) 
        {
            if (candidate.CompareTag("Player"))
            {
                if (trigger.Equals(Type.Entered))
                {
                    CurrentState = State.Tracking;
                    Target = candidate;
                    StateChanged?.Invoke(CurrentState);
                }
                if (trigger.Equals(Type.Left))
                {
                    ResetToIdle();
                }
            }
        }

        /// <summary>
        /// This function is marked as an event in NPC attack animation
        /// </summary>
        public void ResetToIdle()
        {
            CurrentState = State.Idle;
            StateChanged?.Invoke(CurrentState);
        }

        private void OnDisable()
        {
            HitBox.Trigger -= SetTracking;
        }
    }
}