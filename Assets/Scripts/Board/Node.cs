using UnityEngine;

namespace PKPL.DiamondRush.Board
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private NodeType nodeType;
        private NodeIndex index;

        public NodeType Type => nodeType;
        public NodeIndex Index
        {
            get { return index; }
            set { index = value; }
        }

        public void InitNode(NodeType type,NodeIndex pos, Sprite sprite)
        {
            nodeType = type;
            spriteRenderer.sprite = sprite;
            index = pos;
        }
    }
    public class NodeIndex
    {
        public int row;
        public int column;

        public NodeIndex(int r, int c)
        {
            row = r;
            column = c;
        }
    }
    public enum NodeType
    {
        Red,
        Yellow,
        Blue,
        Violet,
        Green
    }
}
