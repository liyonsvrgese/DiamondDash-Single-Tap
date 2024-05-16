using UnityEngine;

namespace PKPL.DiamondRush.Board
{
    public class BoardInputManager : MonoBehaviour
    {
        private BoardManager boardManager;
        private Node selectedNode;
        private Vector2Int dragDirection;

        private void Awake()
        {
            boardManager = GetComponent<BoardManager>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Input.touchCount > 0)
                {
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                }

                if (Physics.Raycast(ray, out hit))
                {
                    Node hitObject = hit.collider.gameObject.GetComponent<Node>() ;
                    if (hitObject.CompareTag(GameConstants.NODE_TAG))
                    {
                        selectedNode = hitObject;
                    }
                }
            }

            if (selectedNode != null && (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)))
            {
                Vector2 dragDelta = (Vector2)Input.mousePosition - (Vector2)Camera.main.WorldToScreenPoint(selectedNode.transform.position);
                dragDirection = GetDragDirection(dragDelta);
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (selectedNode != null)
                {
                    boardManager.TrySwapNodes(selectedNode, dragDirection);
                    selectedNode = null;
                    dragDirection = Vector2Int.zero;
                }
            }
        }

        private Vector2Int GetDragDirection(Vector2 dragDelta)
        {
            Vector2Int direction;

            if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
            {
                direction = (dragDelta.x > 0f) ? Vector2Int.right : Vector2Int.left;
            }
            else
            {
                direction = (dragDelta.y > 0f) ? Vector2Int.up : Vector2Int.down;
            }

            return direction;
        }
    }
}
