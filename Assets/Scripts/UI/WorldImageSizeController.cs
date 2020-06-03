using System;
using AssetVariables;
using UnityEngine;

namespace UI
{
    public class WorldImageSizeController : MonoBehaviour
    {
        public float HeightMultiplier { get; set; }
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
            float height = width * HeightMultiplier;
            
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            rectTransform.anchoredPosition = targetRect.anchoredPosition + Vector2.up * (yOffset +
                (height * rectTransform.localScale.x + targetRect.rect.height * targetRect.localScale.x) / 2);
        }
    }
}
