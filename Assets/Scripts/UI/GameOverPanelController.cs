using UnityEngine;
using TMPro;
using PKPL.DiamondRush.Level;

namespace PKPL.DiamondRush
{
    public class GameOverPanelController : MonoBehaviourWithGameService
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI scoreText;

        protected override void Start()
        {
            base.Start();
            gameOverPanel.SetActive(false); 
            GService.OnGameOver += InitPanel;
        }

        private void InitPanel()
        {
            gameOverPanel.SetActive(true);
            SetScore();
        }
        public void SetScore()
        {
            if (IsGSNull)
            {
                TryGetGS();
                if (IsGSNull)
                {
                    Debug.Log("GameOverPanelController- SetScore- GameService is null");
                }
                return;
            }
            var score = GService.CurrentScore.ToString();
            scoreText.text = score;
        }

        public void OnRestartBtnClicked()
        {
            if (IsGSNull)
            {
                TryGetGS();
                if (IsGSNull)
                {
                    Debug.Log("GameOverPanelController- OnRestartBtnClicked- GameService is null");
                }
                return;
            }
            GService.RestartScene();
        }

        private void OnDisable()
        {
            if(IsGSNull)
            {
                GService.OnGameOver -= InitPanel;
            }
        }
    }
}
