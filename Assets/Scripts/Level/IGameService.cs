using PKPL.DiamondRush.Board;
using System;

namespace PKPL.DiamondRush.Level
{
    public interface IGameService 
    {
        bool IsPowerupActivated { get; }
        AbilityType PowerupType { get; set; }
        bool IsTouchAvailable { get; }
        int CurrentScore { get; }

        event Action OnPowerupComplete;

        event Action OnStartGame;

        event Action<int> OnScoreChanged;

        event Action OnGameOver;

        void TriggerOnStartGame();

        void TriggerGameOver();

        void TriggerOnScoreChanged(int amount);

        void TriggerOnPowerupComplete();

        void RestartScene();
        void SetTouchAvailable(bool value);
        void ActivatePowerup(bool value, AbilityType type = AbilityType.None);

    }
}
