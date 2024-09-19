using PKPL.DiamondRush.Board;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PKPL.DiamondRush.UI
{
    public class PowerupController : MonoBehaviourWithGameService
    {
        [SerializeField] private Sprite[] powerupSprites;
        [SerializeField] private Image iconImage;

        private Button powerupButton;
        private PowerupType powerupType;
        private Action<PowerupType> onClickCB;

        private void Awake()
        {
            powerupButton = GetComponent<Button>();
        }

        public void InitialisePowerup(PowerupType type, Action<PowerupType> onClick)
        {
            powerupType = type;
            onClickCB = onClick;
            Debug.Log("Type " +  type.ToString() + " V " +(int)type );
            iconImage.sprite = powerupSprites[(int)type];
        }
        public void OnButtonClick()
        {
            if (GService.CanActivatePowerup(powerupType))
            {
                powerupButton.interactable = false;
                onClickCB?.Invoke(powerupType);
                Destroy(this.gameObject);
            }
        }

    }
}
