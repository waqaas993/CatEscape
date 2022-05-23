using UnityEngine;
using System.Collections;
using System;

namespace CatEscape.Game
{
    public enum SFXType
    {
        Start,
        Fail,
        Complete
    }

    public class SoundManager : MonoBehaviour
    {
        private bool IsConcluded;

        public AudioSource BackgroundMusic;
        public AudioSource SFX;

        //Start
        //Fail
        //Complete
        public AudioClip[] SFXClips;
        //Regular
        //Enemy Chase
        public AudioClip[] BackgroundClips;

        private void OnEnable()
        {
            Campaign.Begin += PlayStart;
            Campaign.Fail += PlayFail;
            Campaign.Complete += PlayComplete;
            Enemy.StateChanged += EnemyChangedState;
        }

        private void PlayStart()
        {
            IsConcluded = false;
            StartCoroutine(Play(SFXType.Start, 2));
        }

        private void PlayFail()
        {
            IsConcluded = true;
            StartCoroutine(Play(SFXType.Fail, 0));
        }

        private void PlayComplete()
        {
            IsConcluded = true;
            StartCoroutine(Play(SFXType.Complete, 0));
        }
        
        private void EnemyChangedState(State state)
        {
            if (!IsConcluded)
            {
                if ((state.Equals(State.Following) || state.Equals(State.Tracking)) && !BackgroundMusic.clip.Equals(BackgroundClips[1]))
                {
                    BackgroundMusic.clip = BackgroundClips[1];
                    BackgroundMusic.Play();
                }
                else if (!(state.Equals(State.Following) || state.Equals(State.Tracking) && !BackgroundMusic.clip.Equals(BackgroundClips[0])))
                {
                    BackgroundMusic.clip = BackgroundClips[0];
                    BackgroundMusic.Play();
                }
            }
        }

        public IEnumerator Play(SFXType sfxType, float delay)
        {
            yield return new WaitForSeconds(delay);
            switch (sfxType)
            {
                case SFXType.Start:
                    SFX.PlayOneShot(SFXClips[0]);
                    break;
                case SFXType.Fail:
                    BackgroundMusic.Stop();
                    SFX.PlayOneShot(SFXClips[1]);
                    break;
                case SFXType.Complete:
                    SFX.PlayOneShot(SFXClips[2]);
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
            Enemy.StateChanged -= EnemyChangedState;
        }
    }
}