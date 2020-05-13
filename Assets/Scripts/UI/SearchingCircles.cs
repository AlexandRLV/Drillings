using UnityEngine;
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
        [SerializeField] private float extraAnimationTime;
        [SerializeField] private AnimationCurve pulseCurve;

        [Header("Sprites")]
        [SerializeField] private Sprite outerCircleStart;
        [SerializeField] private Sprite innerCircleStart;
        [SerializeField] private Sprite outerCircleFinal;
        [SerializeField] private Sprite innerCircleFinal;
    
        private bool active;
        private bool isAnimating;
        private bool isExtraTime;
        private float timer;
        private float extraAnimationTimer;


        private void Start()
        {
            Play();
        }

        private void Update()
        {
            if (!active)
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
        
            if (!isExtraTime)
                return;

            if (extraAnimationTimer > 0)
                extraAnimationTimer -= Time.deltaTime;
            else
            {
                StopAnimation();
            }
        }


        public void Play()
        {
            Debug.Log("Playing");
            active = true;
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
        }

        public void Stop()
        {
            if (extraAnimationTime > 0)
            {
                isExtraTime = true;
                extraAnimationTimer = extraAnimationTime;
            }
            else
                StopAnimation();
        }

        public void ShowRestartButton()
        {
            loadingImage.SetActive(false);
            startButton.SetActive(false);
            continueButton.SetActive(false);
            restartButton.SetActive(true);
            blackout.SetActive(true);
            outerCircle.sprite = outerCircleFinal;
            innerCircle.sprite = innerCircleFinal;
        }

        public void ShowContinueButton()
        {
            loadingImage.SetActive(false);
            startButton.SetActive(false);
            restartButton.SetActive(false);
            blackout.SetActive(false);
            continueButton.SetActive(true);
            outerCircle.sprite = outerCircleFinal;
            innerCircle.sprite = innerCircleFinal;
        }
    

        public void Show()
        {
            uiManager.Show();
        }

        public void Restart()
        {
            restartButton.SetActive(false);
            blackout.SetActive(false);
            uiManager.Restart();
        }

        public void Continue()
        {
            continueButton.SetActive(false);
            uiManager.Continue();
        }


        private void StopAnimation()
        {
            active = false;
            isExtraTime = false;
            loadingImage.SetActive(false);
            startButton.SetActive(true);
            innerCircle.transform.localScale = Vector3.one;
            outerCircle.transform.localScale = Vector3.one;
            outerCircle.sprite = outerCircleFinal;
            innerCircle.sprite = innerCircleFinal;
        }
    }
}