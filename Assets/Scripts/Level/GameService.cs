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

        public int CurrentScore => currentScore;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        public void TriggerOnStartGame()
        {
            OnStartGame?.Invoke();
        }

        public void TriggerGameOver()
        {
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
    }
}
