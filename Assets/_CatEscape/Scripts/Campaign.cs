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
            NPC.MetTarget += ObjectiveCompleted;
        }
        
        private void Start()
        {
            Invoke("StartCampaign", 4);
        }

        private void StartCampaign()
        {
            Begin?.Invoke();
        }

        private void PlayerDied()
        {
            Fail?.Invoke();
        }

        private void ObjectiveCompleted()
        {
            Complete?.Invoke();
        }

        private void OnDisable()
        {
            Player.Died -= PlayerDied;
            NPC.MetTarget -= ObjectiveCompleted;
        }

    }
}
