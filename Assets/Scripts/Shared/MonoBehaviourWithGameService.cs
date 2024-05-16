using UnityEngine;
using PKPL.DiamondRush.Level;

namespace PKPL.DiamondRush
{
    public class MonoBehaviourWithGameService : MonoBehaviour
    {
        private IGameService gameService;

        public IGameService GService => gameService;

        public bool IsGSNull => gameService == null;

        protected virtual void Start()
        {
            gameService = GameService.Instance;
            if(gameService == null)
            {
                Debug.Log("MonoBehaviourWithGameService- Start- GameService is null");
            }
        }

        protected void TryGetGS()
        {
            gameService = GameService.Instance;
        }
    }
}
