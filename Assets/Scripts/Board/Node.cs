using UnityEngine;

namespace PKPL.DiamondRush.Board
{
    public class Node : MonoBehaviour
    {
        private NodeType nodeType;
        private NodeIndex index;

        public NodeType Type => nodeType;
        public NodeIndex Index => index;

        public void SetIndex(int row, int col)
        {
            index ??= new NodeIndex(row, col);
            index.row = row;
            index.column = col;
        }

        public void InitNode(NodeType type, int row, int col)
        {
            nodeType = type;
            SetIndex(row, col);
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
        Green,
        Yellow,
        Red,
    }
}
