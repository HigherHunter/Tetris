using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class TouchController : MonoBehaviour
    {
        public delegate void TouchEventHandler(Vector2 swipe);

        public static event TouchEventHandler TapEvent;
        public static event TouchEventHandler SwipeEvent;
        public static event TouchEventHandler SwipeEndEvent;

        private Vector2 _touchMovement;

        private const int MinSwipeDistance = 20;

        // Update is called once per frame
        private void Update()
        {
            if (Input.touchCount <= 0) return;

            if (IsPointerOverUIObject()) return;

            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchMovement = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    {
                        _touchMovement += touch.deltaPosition;

                        if (_touchMovement.magnitude > MinSwipeDistance)
                            OnSwipe();
                        else
                            OnTap();
                        break;
                    }
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTap();
                    OnSwipeEnd();
                    break;
            }
        }

        private static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void OnTap()
        {
            TapEvent?.Invoke(_touchMovement);
        }

        private void OnSwipe()
        {
            SwipeEvent?.Invoke(_touchMovement);
        }

        private void OnSwipeEnd()
        {
            SwipeEndEvent?.Invoke(_touchMovement);
        }
    }
}