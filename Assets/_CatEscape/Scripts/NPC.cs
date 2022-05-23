using System;
using UnityEngine;

namespace CatEscape.Game
{
    public class NPC : MonoBehaviour
    {
        private bool Met;

        public Transform Target;
        public float TargetDistance = 0.5f;

        public static Action MetTarget;

        private void FixedUpdate()
        {
            if (Vector3.Distance(Target.position, transform.position) < TargetDistance && !Met)
            {
                Met = true;
                MetTarget?.Invoke();
            }
        }
    }
}