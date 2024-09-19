using PKPL.DiamondRush.Board;
using System.Collections;
using TMPro;
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
        [SerializeField] GameObject powerupTimerUi;
        [SerializeField] TextMeshProUGUI powerupTimerTxt;
        private int prevScore =0;
        private PowerupType currentPowerupType;
        private int powerupCount = 0;
        public void Init()
        {
            base.Start();
            powerupButton.interactable = false;
            powerupTimerUi.SetActive(false);
            powerupSlider.maxValue = GameConstants.POWERUP_REQUIREMENT;
            powerupSlider.value = 0;
            SetPowerup();
            GService.OnScoreChanged += UpdateSlider;
        }

        private void SetPowerup()
        {
            powerupCount++;
            if (powerupCount == GameConstants.TWOX_THRESHOLD)
            {
                currentPowerupType = PowerupType.TwoxScore;
                powerupCount = 0;
            }
            else
            {
                int moves = GService.GetAndResetMovesCount;
                if (moves < 10)
                    currentPowerupType = PowerupType.ColorDestroy;
                else if (moves > 10)
                    currentPowerupType = PowerupType.Bomb;
            }
            powerupBtnImage.sprite = powerupSprites[(int)currentPowerupType];
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
            switch (currentPowerupType)
            {
                case PowerupType.Bomb:
                case PowerupType.ColorDestroy:
                    GService.ActivateClickablePowerup(true,currentPowerupType);
                    break;
                case PowerupType.TwoxScore:
                    StartCoroutine(TwoxCountDown(GameConstants.TWOX_TIME));
                    break;
            }

            powerupSlider.value = 0;
            SetPowerup();
        }

        private void OnDisable()
        {
            if (!IsGSNull)
            {
                GService.OnScoreChanged -= UpdateSlider;
            }
        }

        private IEnumerator TwoxCountDown(int seconds)
        {
            powerupTimerUi.SetActive(true);
            powerupTimerTxt.text = seconds.ToString();
            GService.SetTwoxStatus(true);
            while (seconds > 0)
            {
                powerupTimerTxt.text = seconds.ToString();
                seconds--;
                yield return new WaitForSeconds(1f);
            }
            powerupTimerUi.SetActive(false);
            GService.SetTwoxStatus(false);
        }
    }
}
