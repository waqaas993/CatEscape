using UnityEngine;
using UnityEngine.UI;

namespace CatEscape.Input
{
    public class VirtualJoystick : MonoBehaviour
    {
        public Image ImageBackground;
        public Image JoystickImage;

        private static Vector2 _InputVector;

        public static Vector2 InputVector
        {
            get { return _InputVector; }
            private set { _InputVector = value; }
        }

        private void Update()
        {
            InputEditor();
            InputMobile();
        }

        private void InputEditor()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                transform.position = UnityEngine.Input.mousePosition;
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                Vector2 pos = UnityEngine.Input.mousePosition - transform.position;
                pos.x = (pos.x / ImageBackground.rectTransform.sizeDelta.x);
                pos.y = (pos.y / ImageBackground.rectTransform.sizeDelta.y);
                InputVector = new Vector2(pos.x, pos.y) * 2f;
                if (InputVector.magnitude > 1)
                {
                    transform.position = Vector3.Lerp(transform.position, UnityEngine.Input.mousePosition, Time.deltaTime * 3);
                    InputVector = InputVector.normalized;
                }
                JoystickImage.rectTransform.anchoredPosition = new Vector2(InputVector.x * (ImageBackground.rectTransform.sizeDelta.x * .5f),
                                                                         InputVector.y * (ImageBackground.rectTransform.sizeDelta.y * .5f));
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                InputVector = Vector2.zero;
                JoystickImage.rectTransform.anchoredPosition = Vector2.zero;
            }
        }

        private void InputMobile()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        transform.position = touch.position;
                        break;
                    case TouchPhase.Moved:
                        Vector2 pos = touch.position - (Vector2)transform.position;
                        pos.x = (pos.x / ImageBackground.rectTransform.sizeDelta.x);
                        pos.y = (pos.y / ImageBackground.rectTransform.sizeDelta.y);
                        InputVector = new Vector2(pos.x, pos.y) * 2f;
                        if (InputVector.magnitude > 1)
                        {
                            transform.position = Vector3.Lerp(transform.position, touch.position, Time.deltaTime * 3);
                            InputVector = InputVector.normalized;
                        }
                        JoystickImage.rectTransform.anchoredPosition = new Vector2(InputVector.x * (ImageBackground.rectTransform.sizeDelta.x * .5f),
                                                                                 InputVector.y * (ImageBackground.rectTransform.sizeDelta.y * .5f));
                        break;
                    case TouchPhase.Ended:

                        break;
                }
            }
        }
    }
}