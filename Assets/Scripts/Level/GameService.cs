using PKPL.DiamondRush.Board;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PKPL.DiamondRush.Level
{
    public class GameService : BaseMonoSingletonGeneric<GameService>, IGameService
    {
        public event Action OnStartGame;
        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnPowerupComplete;
        public BoardManager boardManager;

        private int currentScore;

        public bool IsTouchAvailable { get; private set; }

        public int CurrentScore => currentScore;

        public bool IsClickablePowerupActivated { get ; private set ; }
        public PowerupType PowerupType { get ; set ; }

        public int GetAndResetMovesCount => boardManager.GetAndResetMovesCount();

        public bool IsTwoxActive  {get ; private set;}


        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            IsClickablePowerupActivated = false;
            PowerupType = PowerupType.None;
        }

        public void TriggerOnStartGame()
        {
            SetTouchAvailable(true);
            OnStartGame?.Invoke();
        }

        public void TriggerGameOver()
        {
            SetTouchAvailable(false);
            OnGameOver?.Invoke();
        }

        public void IncreaseScoreForOneBlock()
        {
            var amount = IsTwoxActive ? GameConstants.SCORE_FOR_ONE_ITEM * 2 :
                GameConstants.SCORE_FOR_ONE_ITEM;
            this.currentScore += amount;
            OnScoreChanged?.Invoke(currentScore);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetTouchAvailable(bool value = false)
        {
            IsTouchAvailable = value;
        }

        public bool CanActivatePowerup(PowerupType type)
        {
            if (type == PowerupType.TwoxScore && IsTwoxActive)
                return false;
            else 
                return !IsClickablePowerupActivated;
        }

        public void SetClickablePowerup(bool value, PowerupType type = PowerupType.None)
        {
            IsClickablePowerupActivated = value;
            PowerupType = type;
        }

        public void TriggerOnPowerupComplete()
        {
            SetClickablePowerup(false);
            OnPowerupComplete?.Invoke();
        }

        public void SetTwoxStatus(bool value)
        {
            IsTwoxActive = value;
        }
    }
}
