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
        [SerializeField] private GameObject[] powerupHolders;
        [SerializeField] GameObject powerupTimerUi;
        [SerializeField] TextMeshProUGUI powerupTimerTxt;
        [SerializeField] private PowerupController powerupPrefab;

        private int prevScore =0;
        private int powerupCount = 0;
        private int powerupHolderIndex = 0;
        public void Init()
        {
            base.Start();
            powerupTimerUi.SetActive(false);
            powerupSlider.maxValue = GameConstants.POWERUP_REQUIREMENT;
            powerupSlider.value = 0;
            GService.OnScoreChanged += UpdateSlider;
        }

        private void SetPowerup()
        {
            if(powerupHolderIndex < 0)
                powerupHolderIndex = 0;
            if (powerupHolderIndex == powerupHolders.Length)
                return;

            powerupCount++;
            powerupSlider.value = 0;
            var powerupType = PowerupType.Bomb;
            if (powerupCount == GameConstants.TWOX_THRESHOLD)
            {
                powerupType = PowerupType.TwoxScore;
                powerupCount = 0;
            }
            else
            {
                int moves = GService.GetAndResetMovesCount;
                if (moves < 10)
                    powerupType = PowerupType.ColorDestroy;
                else if (moves > 10)
                    powerupType = PowerupType.Bomb;
            }
            var instance = Instantiate(powerupPrefab,powerupHolders[powerupHolderIndex].transform);
            powerupHolderIndex++;    
            instance.InitialisePowerup(powerupType,OnPowerupButtonClicked);

        }

        private void UpdateSlider(int currentScore)
        {
            powerupSlider.value += currentScore - prevScore;
            if (powerupSlider.value >= GameConstants.POWERUP_REQUIREMENT)
            {
                powerupSlider.value = powerupSlider.maxValue;
                SetPowerup();
            }
            prevScore = currentScore;
        }

        public void OnPowerupButtonClicked(PowerupType type)
        {
            powerupHolderIndex--;
            switch (type)
            {
                case PowerupType.Bomb:
                case PowerupType.ColorDestroy:
                    GService.SetClickablePowerup(true,type);
                    break;
                case PowerupType.TwoxScore:
                    StartCoroutine(TwoxCountDown(GameConstants.TWOX_TIME));
                    break;
            };
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
