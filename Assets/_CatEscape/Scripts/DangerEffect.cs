using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CatEscape.Game
{
    public class DangerEffect : MonoBehaviour
    {
        private Coroutine Coroutine;
        public Image Background;

        private void OnEnable()
        {
            Enemy.StateChanged += EnemyChangedState;
        }

        private void EnemyChangedState(State state)
        {
            if ((state.Equals(State.Following) || state.Equals(State.Tracking)))
            {
                if (Coroutine == null)
                {
                    Coroutine = StartCoroutine(SimulateEffect());
                }
            }
            else if (!(state.Equals(State.Following) || state.Equals(State.Tracking)))
            {
                if (Coroutine != null)
                {
                    StopCoroutine(Coroutine);
                    Coroutine = null;
                }
                DOTween.Kill(Background);
                Background.DOFade(0, 0.5f);
            }
        }

        private IEnumerator SimulateEffect()
        {
            do
            {
                Background.DOFade(0.1f, 0.5f);
                yield return new WaitForSeconds(0.5f);
                Background.DOFade(0, 0.5f);
                yield return new WaitForSeconds(0.5f);
            } while (true);
        }

        private void OnDisable()
        {
            Enemy.StateChanged -= EnemyChangedState;
        }
    }
}