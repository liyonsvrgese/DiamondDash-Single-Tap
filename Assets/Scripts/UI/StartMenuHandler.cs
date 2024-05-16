using PKPL.DiamondRush.Level;
using UnityEngine;

namespace PKPL.DiamondRush.UI
{
    public class StartMenuHandler : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;

        private void Start()
        {
            startMenu.SetActive(true);
        }
        public void OnPlayBtnClicked()
        {
            var gameService = GameService.Instance;
            if(gameService != null)
            {
                gameService.TriggerOnStartGame();
            }
            startMenu.SetActive(false);
        }
    }
}

