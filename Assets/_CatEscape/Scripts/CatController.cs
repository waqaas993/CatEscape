using UnityEngine;
using CatEscape.Input;
using System;

namespace CatEscape.Game
{
    public class CatController : MonoBehaviour
    {
        public Animator Animator;
        [Header("Player Properties")]
        [Tooltip("Walking speed of Player")]
        public float Speed;
        [Tooltip("Health of Player")]
        public float Health;

        private void OnEnable()
        {
            NPC.Attacked += ReceiveDamage;
        }

        private void ReceiveDamage(GameObject attacker, GameObject victim, float damage)
        {
            if (gameObject.Equals(victim))
            {
                Health -= damage;
                if (Health <= 0)
                {
                    //TODO: death
                    Debug.Log("Died!");
                }
            }
        }

        private void FixedUpdate()
        {
            Vector3 inputVector = new Vector3(VirtualJoystick.InputVector.x, 0, VirtualJoystick.InputVector.y);

            Animator.SetFloat("input_magnitude", inputVector.magnitude * 2);
            Animator.SetFloat("walk_speed", inputVector.magnitude * Speed);

            transform.forward = !inputVector.Equals(Vector3.zero) ? inputVector : transform.forward;
            transform.Translate(inputVector * Speed * Time.fixedDeltaTime, Space.World);
        }

        private void OnDisable()
        {
            NPC.Attacked -= ReceiveDamage;
        }
    }
}
