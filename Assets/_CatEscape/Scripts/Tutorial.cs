using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using CatEscape.Input;
using System.Collections;

namespace CatEscape.Game
{
    public class Tutorial : MonoBehaviour
    {
        private Coroutine Coroutine;
        public Text[] Instructions;

        private void OnEnable()
        {
            Campaign.Begin += CampaignBegan;
            Enemy.StateChanged += EnemyChangedState;
        }

        private void Awake()
        {
            DisableInstructions();
            string text = Instructions[0].text;
            Instructions[0].text = "";
            Instructions[0].gameObject.SetActive(true);
            Instructions[0].DOText(text, 2);
        }

        private void CampaignBegan()
        {
            DisableInstructions();
            Instructions[1].gameObject.SetActive(true);
        }

        private void Update()
        {
            if (Instructions[1].gameObject.activeSelf)
            {
                Vector3 inputVector = new Vector3(VirtualJoystick.InputVector.x, 0, VirtualJoystick.InputVector.y);
                if (inputVector.magnitude > 0.5f)
                {
                    Instructions[1].gameObject.SetActive(false);
                }
            }
        }

        private void EnemyChangedState(State state)
        {
            
            if (state.Equals(State.Following) || state.Equals(State.Tracking))
            {
                if (Coroutine == null)
                {
                    Instructions[2].gameObject.SetActive(true);
                    Coroutine = StartCoroutine(SimulateHurry());
                }
            }
            else if (!(state.Equals(State.Following) || state.Equals(State.Tracking)))
            {
                if (Coroutine != null)
                {
                    StopCoroutine(Coroutine);
                    Coroutine = null;
                }
                DOTween.Kill(Instructions[2].transform);
                DisableInstructions();
            }
        }

        private IEnumerator SimulateHurry()
        {
            do
            {
                Instructions[2].transform.DOScale(Vector3.one * 1.5f, 0.5f);
                yield return new WaitForSeconds(0.5f);
                Instructions[2].transform.DOScale(Vector3.one, 0.5f);
                yield return new WaitForSeconds(0.5f);
            } while (true);
        }

        private void DisableInstructions()
        {
            foreach (Text instruction in Instructions)
            {
                instruction.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            Campaign.Begin += CampaignBegan;
            Enemy.StateChanged -= EnemyChangedState;
        }
    }
}