using PKPL.DiamondRush.Board;
using UnityEngine;
using UnityEngine.UI;

namespace PKPL.DiamondRush.UI
{
    public class PowerupManager : MonoBehaviourWithGameService
    {
        [SerializeField] private Slider powerupSlider;
        [SerializeField] private Button powerupButton;
        [SerializeField] private Sprite[] powerupSprites;
        [SerializeField] private Image powerupBtnImage;
        private int prevScore =0;
        private int currentPowerupType;
        public void Init()
        {
            base.Start();
            powerupButton.interactable = false;
            powerupSlider.maxValue = GameConstants.POWERUP_REQUIREMENT;
            powerupSlider.value = 0;
            SetPowerup();
            GService.OnScoreChanged += UpdateSlider;
            GService.OnPowerupComplete += OnPowerupComplete;
        }
        private void SetPowerup()
        {
            currentPowerupType = Random.Range(1,powerupSprites.Length+1);
            powerupBtnImage.sprite = powerupSprites[currentPowerupType - 1];
        }
        private void UpdateSlider(int currentScore)
        {
            powerupSlider.value += currentScore - prevScore;
            if (powerupSlider.value >= GameConstants.POWERUP_REQUIREMENT)
            {
                ActivatePowerupButton();
                powerupSlider.value = powerupSlider.maxValue;
            }
            prevScore = currentScore;

        }

        private void ActivatePowerupButton()
        {
            powerupButton.interactable = true;
        }

        public void OnPowerupButtonClicked()
        {
            powerupButton.interactable = false;
            GService.ActivatePowerup(true, (AbilityType)currentPowerupType);
        }

        private void OnDisable()
        {
            if (!IsGSNull)
            {
                GService.OnScoreChanged -= UpdateSlider;
                GService.OnPowerupComplete -= OnPowerupComplete;
            }
        }

        private void OnPowerupComplete()
        {
            powerupSlider.value = 0;
            SetPowerup();
        }
    }
}
