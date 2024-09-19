using PKPL.DiamondRush.Board;
using System;

namespace PKPL.DiamondRush.Level
{
    public interface IGameService 
    {
        bool IsClickablePowerupActivated { get; }

        bool IsTwoxActive { get; }
        PowerupType PowerupType { get; set; }
        bool IsTouchAvailable { get; }
        int CurrentScore { get; }

        event Action OnPowerupComplete;

        event Action OnStartGame;

        event Action<int> OnScoreChanged;

        event Action OnGameOver;

        int GetAndResetMovesCount { get; }
        void TriggerOnStartGame();

        void TriggerGameOver();

        bool CanActivatePowerup(PowerupType type);

        void SetTwoxStatus(bool value);
        void IncreaseScoreForOneBlock();

        void TriggerOnPowerupComplete();

        void RestartScene();
        void SetTouchAvailable(bool value);
        void SetClickablePowerup(bool value, PowerupType type = PowerupType.None);

    }
}
