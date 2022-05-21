using UnityEngine;
using CatEscape.Input;

namespace CatEscape.Game
{
    public class CatController : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public Animator Animator;
        public float Speed;

        private void FixedUpdate()
        {
            Vector3 inputVector = new Vector3(VirtualJoystick.InputVector.x, 0, VirtualJoystick.InputVector.y);

            Animator.SetFloat("input_magnitude", inputVector.magnitude * 2);
            Animator.SetFloat("walk_speed", inputVector.magnitude * Speed);

            transform.forward = !inputVector.Equals(Vector3.zero) ? inputVector : transform.forward;
            transform.Translate(inputVector * Speed * Time.fixedDeltaTime, Space.World);
        }
    }
}
