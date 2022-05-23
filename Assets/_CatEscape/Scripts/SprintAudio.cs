using UnityEngine;

namespace CatEscape.Game
{
    public class SprintAudio : MonoBehaviour
    {
        public Enemy Enemy;
        public AudioSource AudioSource;

        private void Update()
        {
            if (Enemy.CurrentState.Equals(State.Following) && !AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
            else if (!Enemy.CurrentState.Equals(State.Following) && AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
        }
    }
}
