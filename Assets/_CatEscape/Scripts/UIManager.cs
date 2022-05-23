using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace CatEscape.Game
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Game screen, End Card Fail, End Card Success 
        /// </summary>
        public GameObject[] Screens;

        public float TransitionTime = 0.25f;

        private void OnEnable()
        {
            Campaign.Begin += ShowGameScreen;
            Campaign.Fail += ShowEndCardFailScreen;
            Campaign.Complete += ShowEndCardSuccessScreen;
        }

        private void ShowGameScreen()
        {
            StartCoroutine(ShowScreen(Screens[0], 0));
        }

        private void ShowEndCardFailScreen()
        {
            StartCoroutine(ShowScreen(Screens[1], TransitionTime));
        }

        private void ShowEndCardSuccessScreen()
        {
            StartCoroutine(ShowScreen(Screens[2], TransitionTime));
        }

        private IEnumerator ShowScreen(GameObject screenToShow, float delay)
        {
            yield return new WaitForSeconds(delay);
            foreach (GameObject screen in Screens)
            {
                if (screen != screenToShow)
                {
                    screen.GetComponent<CanvasGroup>().DOFade(0, TransitionTime);
                    yield return new WaitForSeconds(TransitionTime);
                    screen.SetActive(false);
                }
            }
            screenToShow.GetComponent<CanvasGroup>().alpha = 0;
            screenToShow.SetActive(true);
            screenToShow.GetComponent<CanvasGroup>().DOFade(1, TransitionTime);
        }

        public void OnClickPlay(string url)
        {
            Application.OpenURL(url);
        }

        private void OnDisable()
        {
            Campaign.Begin -= ShowGameScreen;
            Campaign.Fail -= ShowEndCardFailScreen;
            Campaign.Complete -= ShowEndCardSuccessScreen;
        }
    }
}