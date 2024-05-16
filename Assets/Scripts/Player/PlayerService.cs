using System;
using UnityEngine;

namespace PKPL.DiamondRush.Player
{
    public class PlayerService : BaseMonoSingletonGeneric<PlayerService>, IPlayerService
    {
        public event Action OnStartGame;

        public void TriggerOnStartGame()
        {
            OnStartGame?.Invoke();
        }        
    }
}

