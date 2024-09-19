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
        private bool isTwoxActive = false;

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
            var amount = isTwoxActive ? GameConstants.SCORE_FOR_ONE_ITEM * 2 :
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

        public void ActivateClickablePowerup(bool value, PowerupType type = PowerupType.None)
        {
            IsClickablePowerupActivated = value;
            PowerupType = type;
        }

        public void TriggerOnPowerupComplete()
        {
            ActivateClickablePowerup(false);
            OnPowerupComplete?.Invoke();
        }

        public void SetTwoxStatus(bool value)
        {
            isTwoxActive = value;
        }
    }
}
