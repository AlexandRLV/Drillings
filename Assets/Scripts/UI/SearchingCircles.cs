using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class SearchingCircles : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image outerCircle;
        [SerializeField] private Image innerCircle;
        [SerializeField] private GameObject loadingImage;
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject continueButton;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject blackout;
        [SerializeField] private UIManager uiManager;

        [Header("Settings")]
        [SerializeField] private float pulseTime;
        [SerializeField] private float waitTime;
        [SerializeField] private float scaleMultiplier;
        [SerializeField] private float loadingAnimationTime;
        [SerializeField] private AnimationCurve pulseCurve;

        [Header("Sprites")]
        [SerializeField] private Sprite outerCircleStart;
        [SerializeField] private Sprite innerCircleStart;
        [SerializeField] private Sprite outerCircleFinal;
        [SerializeField] private Sprite innerCircleFinal;
    
        private bool hasRunningAnimation;
        private bool isAnimating;
        private bool activeLoadingAnimation;
        private float timer;
        private float loadingAnimationTimer;


        private void Start()
        {
            Play();
        }

        private void Update()
        {
            if (!hasRunningAnimation)
                return;

            if (isAnimating)
            {
                float t = timer / pulseTime * 2 - 1;
                float scale = pulseCurve.Evaluate(1 - Mathf.Abs(t));
                scale *= scaleMultiplier;
                innerCircle.transform.localScale = Vector3.one * (scale + 1);
                outerCircle.transform.localScale = Vector3.one * (scale + 1);
            }

            
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                if (isAnimating)
                {
                    innerCircle.transform.localScale = Vector3.one;
                    outerCircle.transform.localScale = Vector3.one;
                    timer = waitTime;
                }
                else
                    timer = pulseTime;

                isAnimating = !isAnimating;
            }
        
            
            if (!activeLoadingAnimation)
                return;

            if (loadingAnimationTimer > 0)
                loadingAnimationTimer -= Time.deltaTime;
            else
            {
                StopLoadingAnimation();
            }
        }


        public void Play()
        {
            Debug.Log("Playing");
            hasRunningAnimation = true;
            outerCircle.sprite = outerCircleStart;
            innerCircle.sprite = innerCircleStart;
            loadingImage.SetActive(false);
            startButton.SetActive(false);
            blackout.SetActive(false);
            restartButton.SetActive(false);
            continueButton.SetActive(false);
        }
    
        public void ShowLoading()
        {
            Debug.Log("Show loading");
            loadingImage.SetActive(true);
            activeLoadingAnimation = true;
            loadingAnimationTimer = loadingAnimationTime;
        }
    

        public void Show()
        {
            uiManager.Show();
        }


        private void StopLoadingAnimation()
        {
            hasRunningAnimation = false;
            activeLoadingAnimation = false;
            loadingImage.SetActive(false);
            startButton.SetActive(true);
            innerCircle.transform.localScale = Vector3.one;
            outerCircle.transform.localScale = Vector3.one;
            outerCircle.sprite = outerCircleFinal;
            innerCircle.sprite = innerCircleFinal;
        }
    }
}