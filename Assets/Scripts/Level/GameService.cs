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

        private int currentScore;

        public bool IsTouchAvailable { get; private set; }

        public int CurrentScore => currentScore;

        public bool IsPowerupActivated { get ; private set ; }
        public AbilityType PowerupType { get ; set ; }

        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            IsPowerupActivated = false;
            PowerupType = AbilityType.None;
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

        public void TriggerOnScoreChanged(int amount)
        {
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

        public void ActivatePowerup(bool value, AbilityType type = AbilityType.None)
        {
            IsPowerupActivated = value;
            PowerupType = type;
        }

        public void TriggerOnPowerupComplete()
        {
            ActivatePowerup(false);
            OnPowerupComplete?.Invoke();
        }
    }
}
