using UnityEngine;

namespace PKPL.DiamondRush.Board
{
    public class BoardInputManager : MonoBehaviourWithGameService
    {
        private BoardManager boardManager;
        private Node selectedNode;
        private bool isTouchAavailable => GService.IsTouchAvailable;

        private void Awake()
        {
            boardManager = GetComponent<BoardManager>();
        }
        private void Update()
        {
            if(!isTouchAavailable)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Input.touchCount > 0)
                {
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                }

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Node hitObject = hit.collider.gameObject.GetComponent<Node>() ;
                    if (hitObject.CompareTag(GameConstants.NODE_TAG))
                    {
                        selectedNode = hitObject;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (selectedNode != null)
                {
                    boardManager.CheckForMatches(selectedNode);
                    selectedNode = null;
                }
            }
        }

       
    }
}
