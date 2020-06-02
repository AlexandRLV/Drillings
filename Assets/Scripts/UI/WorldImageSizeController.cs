using System;
using AssetVariables;
using UnityEngine;

namespace UI
{
    public class WorldImageSizeController : MonoBehaviour
    {
        [SerializeField] private FloatVariable heightMultiplier;
        [SerializeField] private FloatVariable yOffset;
        
        [SerializeField] private RectTransform targetRect;

        private RectTransform rectTransform;


        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Set size and position relative to target rect
            float width = targetRect.rect.width;
            float height = targetRect.rect.width * heightMultiplier;
            
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            rectTransform.anchoredPosition = targetRect.anchoredPosition + Vector2.up * (yOffset + height * rectTransform.localScale.x / 2);
        }
    }
}
