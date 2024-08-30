using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PKPL.DiamondRush.UI
{
    public class IngameUiHandler : MonoBehaviourWithGameService
    {
        [SerializeField] private GameObject uiPanel;
        [SerializeField] private Slider timeSlider;
        [SerializeField] private Image timeSliderFillImage;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private PowerupManager powerupManager;

        private float timer =0f;
        private bool isGameRunning = false;

        protected override void Start()
        {
            base.Start();
            uiPanel.SetActive(false);
            timeSlider.gameObject.SetActive(false);
            powerupManager.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            scoreTxt.gameObject.SetActive(false);
            GService.OnScoreChanged += UpdateScoreText;
            GService.OnStartGame += () =>
            {
                isGameRunning = true;
                uiPanel.SetActive(true);
                timeSlider.gameObject.SetActive(true);
                powerupManager.gameObject.SetActive(true);
                powerupManager.Init();
                timeText.gameObject.SetActive(true);
                scoreTxt.gameObject.SetActive(true);
                SetSlider();
                UpdateScoreText(0);

            };
            GService.OnGameOver += () =>
            {
                isGameRunning = false;
                uiPanel.SetActive(false);
            };
            
        }
        private void Update()
        {
            if (isGameRunning)
            {
                UpdateTimer();
            }
        }

        public void UpdateScoreText(int currentScore)
        {
            scoreTxt.text = "Score: " + currentScore.ToString();
        }

        private void OnDisable()
        {
            if (!IsGSNull)
            {
                GService.OnScoreChanged -= UpdateScoreText;
            }
        } 
        private void SetSlider()
        {
            timeSlider.maxValue = GameConstants.MAX_GAME_TIME;
            timeSliderFillImage.color = Color.green;
            timeSlider.value = timeSlider.maxValue;
            timeText.text = timeSlider.value.ToString();
        }

        private void UpdateSlider()
        {
            timeSlider.value -= 1;
            timeSlider.value = Mathf.Clamp(timeSlider.value, 0, timeSlider.maxValue + 1);
            timeText.text = timeSlider.value.ToString();

            if (timeSlider.value == 0)
            {
                if (!IsGSNull)
                {
                    GService.TriggerGameOver();
                }
                else
                {
                    TryGetGS();
                    if (IsGSNull)
                    {
                        Debug.Log("IngameUiHandler- UpdateSlider - GameService is null");
                    }
                }
                return;
            }
           
            float normalizedValue = timeSlider.value / timeSlider.maxValue;

            Color color = normalizedValue >= 0.5f ? Color.Lerp(Color.yellow, Color.green, (normalizedValue - 0.5f) * 2f)
                : Color.Lerp(Color.red, Color.yellow, normalizedValue * 2f);

            timeSliderFillImage.color = color;
        }

        private void UpdateTimer()
        {
            timer += Time.deltaTime;
            if(Mathf.FloorToInt(timer) >=1)
            {
                timer = 0f;
                UpdateSlider();
            }
        }
    }
}
