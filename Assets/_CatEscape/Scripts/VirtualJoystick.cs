using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CatEscape.Input
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {

        public Image ImageBackground;
        public Image JoystickImage;
        public bool IsPressed = false;

        private Vector2 DefaultAnchoredPosition;

        private static Vector2 _InputVector;

        public static Vector2 InputVector
        {
            get { return _InputVector; }
            private set { _InputVector = value; }
        }


        public void OnPointerDown(PointerEventData e)
        {
            DefaultAnchoredPosition = ImageBackground.rectTransform.anchoredPosition;
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ImageBackground.rectTransform,
                                                                e.position,
                                                                e.pressEventCamera,
                                                                out pos))
            {
                ImageBackground.rectTransform.anchoredPosition = new Vector2(ImageBackground.rectTransform.anchoredPosition.x + (pos.x / ImageBackground.rectTransform.sizeDelta.x) * 249, ImageBackground.rectTransform.anchoredPosition.y);
            }
            OnDrag(e);
        }

        public void OnDrag(PointerEventData e)
        {
            IsPressed = true;
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ImageBackground.rectTransform,
                                                                        e.position,
                                                                        e.pressEventCamera,
                                                                        out pos))
            {
                pos.x = (pos.x / ImageBackground.rectTransform.sizeDelta.x);
                pos.y = (pos.y / ImageBackground.rectTransform.sizeDelta.y);
                InputVector = new Vector2(pos.x, pos.y) * 2f;
                InputVector = (InputVector.magnitude > 1) ? InputVector.normalized : InputVector;
                JoystickImage.rectTransform.anchoredPosition = new Vector2(InputVector.x * (ImageBackground.rectTransform.sizeDelta.x * .5f),
                                                                         InputVector.y * (ImageBackground.rectTransform.sizeDelta.y * .5f));
            }
        }

        public void ResetJoystick()
        {
            IsPressed = false;
            InputVector = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData e)
        {
            IsPressed = false;
            InputVector = Vector2.zero;
            JoystickImage.rectTransform.anchoredPosition = Vector2.zero;
            ImageBackground.rectTransform.anchoredPosition = DefaultAnchoredPosition;
        }
    }
}