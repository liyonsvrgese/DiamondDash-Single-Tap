using System;

namespace PKPL.DiamondRush.Level
{
    public interface IGameService 
    {
        bool IsTouchAvailable { get; }
        int CurrentScore { get; }

        event Action OnStartGame;

        event Action<int> OnScoreChanged;

        event Action OnGameOver;

        void TriggerOnStartGame();

        void TriggerGameOver();

        void TriggerOnScoreChanged(int amount);

        void RestartScene();
        void SetTouchAvailable(bool value);
    }
}
