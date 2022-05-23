using System;
using UnityEngine;

namespace CatEscape.Game
{
    public class Campaign : MonoBehaviour
    {
        public static Action Begin;
        public static Action Fail;
        public static Action Complete;

        private void OnEnable()
        {
            Player.Died += PlayerDied;
        }
        
        private void Start()
        {
            Begin?.Invoke();
        }

        private void PlayerDied()
        {
            Fail?.Invoke();
        }

        private void OnDisable()
        {
            Player.Died -= PlayerDied;
        }

    }
}
