using UnityEngine;

namespace UI
{
    public class SwipeManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float swipeThreshold;
    
        [Header("References")]
        [SerializeField] private UIManager uiManager;

        private Vector2 deltaPos;
    
    
    
        private void Update()
        {
            if (Input.touchCount == 0)
                return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                deltaPos = Vector2.zero;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                deltaPos += touch.deltaPosition;
                Swipe();
            }
            else
            {
                deltaPos += touch.deltaPosition;
            }
        }



        private void Swipe()
        {
            float diff = deltaPos.x;
            if (diff < -swipeThreshold)
            {
                Debug.Log("Swiped to next unit");
                //uiManager.NextUnit();
            }

            if (diff > swipeThreshold)
            {
                Debug.Log("Swiped to prev unit");
                //uiManager.PrevUnit();
            }
        }
    }
}