using System;
using UnityEngine;

namespace CatEscape.Game
{
    public enum Type
    {
        Entered,
        Left
    }

    public class HitBox : MonoBehaviour
    {
        public static Action<GameObject, Type> Trigger;

        private void OnTriggerEnter(Collider other)
        {
            Trigger?.Invoke(other.gameObject, Type.Entered);
        }

        private void OnTriggerExit(Collider other)
        {
            Trigger?.Invoke(other.gameObject, Type.Left);
        }
    }
}
