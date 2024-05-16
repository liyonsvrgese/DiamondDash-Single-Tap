using System;

namespace PKPL.DiamondRush.Level
{
    public interface IGameService 
    {
        int CurrentScore { get; }

        event Action OnStartGame;

        event Action<int> OnScoreChanged;

        event Action OnGameOver;

        void TriggerOnStartGame();

        void TriggerGameOver();

        void TriggerOnScoreChanged(int amount);

        void RestartScene();
    }
}
