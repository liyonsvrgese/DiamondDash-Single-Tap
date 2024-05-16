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

        private int currentScore;

        public bool IsTouchAvailable { get; private set; }

        public int CurrentScore => currentScore;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }

        public void TriggerOnStartGame()
        {
            IsTouchAvailable = true;
            OnStartGame?.Invoke();
        }

        public void TriggerGameOver()
        {
            IsTouchAvailable = false;
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
    }
}
