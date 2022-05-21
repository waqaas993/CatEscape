using UnityEngine;

namespace CatEscape.Game
{
    public enum SFXType
    {
        Start,
        Fail,
        Complete,
    }

    public class SoundManager : MonoBehaviour
    {
        public AudioSource SFX;

        //Start
        //Fail
        //Complete
        public AudioClip[] Clips;

        private void OnEnable()
        {
            Campaign.Begin += PlayStart;
            Campaign.Fail += PlayFail;
            Campaign.Complete += PlayComplete;
        }

        private void PlayStart()
        {
            Play(SFXType.Start);
        }

        private void PlayFail()
        {
            Play(SFXType.Fail);
        }

        private void PlayComplete()
        {
            Play(SFXType.Complete);
        }

        public void Play(SFXType sfxType)
        {
            switch (sfxType)
            {
                case SFXType.Start:
                    SFX.PlayOneShot(Clips[0]);
                    break;
                case SFXType.Fail:
                    SFX.PlayOneShot(Clips[1]);
                    break;
                case SFXType.Complete:
                    SFX.PlayOneShot(Clips[2]);
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            Campaign.Begin -= PlayStart;
            Campaign.Fail -= PlayFail;
            Campaign.Complete -= PlayComplete;
        }
    }
}