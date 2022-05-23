using System;
using UnityEngine;
using CatEscape.Input;

namespace CatEscape.Game
{
    public class Player : MonoBehaviour
    {
        private bool CanPlay;

        public Animator Animator;
        [Header("Player Properties")]
        [Tooltip("Walking speed of Player")]
        public float Speed;
        [Tooltip("Health of Player")]
        public float Health;

        public static Action Died;

        private void OnEnable()
        {
            Enemy.Attacked += ReceiveDamage;
            Campaign.Begin += CampaignBegan;
            Campaign.Fail += CampaignConcluded;
            Campaign.Complete += CampaignConcluded;
        }

        private void CampaignBegan()
        {
            CanPlay = true;
        }

        private void CampaignConcluded()
        {
            CanPlay = false;
        }

        private void ReceiveDamage(GameObject attacker, GameObject victim, float damage)
        {
            if (gameObject.Equals(victim))
            {
                Health -= damage;
                if (Health <= 0)
                {
                    Died?.Invoke();
                }
            }
        }

        private void FixedUpdate()
        {
            if (CanPlay)
            {
                Control();
            }
        }

        private void Control()
        {
            Vector3 inputVector = new Vector3(VirtualJoystick.InputVector.x, 0, VirtualJoystick.InputVector.y);

            Animator.SetFloat("input_magnitude", inputVector.magnitude * 2);
            Animator.SetFloat("walk_speed", inputVector.magnitude * Speed);

            transform.forward = !inputVector.Equals(Vector3.zero) ? inputVector : transform.forward;
            transform.Translate(inputVector * Speed * Time.fixedDeltaTime, Space.World);
        }

        private void OnDisable()
        {
            Enemy.Attacked -= ReceiveDamage;
            Campaign.Begin += CampaignBegan;
            Campaign.Fail += CampaignConcluded;
            Campaign.Complete += CampaignConcluded;
        }
    }
}
