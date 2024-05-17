using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace PKPL.DiamondRush.Board
{
    public class BoardManager : MonoBehaviourWithGameService
    {
        [SerializeField] private NodeBGManager tileBasePrefab;
        [SerializeField] private int noOfRows;
        [SerializeField] private int noOfColumns;
        [SerializeField] private Node nodePrefab;
        [SerializeField] private NodeSprites[] nodeSprites;
        private const float tileWidth = 0.7f;
        private const float tileHeight = 0.7f;


        private NodeBGManager[,] bgBoard;
        private Node[,] board;
        private Dictionary<NodeType, Sprite> nodeSpritesDict = new();
        private Vector2 startPos;
        private Coroutine clearCoroutine;
        private List<NodeIndex> deletedNodes;

        protected override void Start()
        {
            base.Start();
            InitCollections();
            startPos = new Vector2(transform.position.x - (noOfColumns - 1) *tileWidth / 2f,
                transform.position.y + (noOfRows - 1) * tileHeight / 2f);

            GService.OnStartGame += SpawnBoard;
            GService.OnGameOver += () =>
              {
                  GetComponent<BoardInputManager>().enabled = false;
              };
            AddNodeSpritesToDict();
        }

        private void InitCollections()
        {
            bgBoard = new NodeBGManager[noOfRows, noOfColumns];
            nodeSpritesDict = new();
            board = new Node[noOfRows, noOfColumns];
            deletedNodes = new();
        }
        private void AddNodeSpritesToDict()
        {
            for (var i = 0; i < nodeSprites.Length; i++)
            {
                if (!nodeSpritesDict.ContainsKey(nodeSprites[i].nodeType))
                {
                    nodeSpritesDict.Add(nodeSprites[i].nodeType, nodeSprites[i].nodeSprite);
                }
            }
        }

        private void SpawnBoard()
        {
            var index = 1;
            for (var i = 0; i < noOfRows; i++)
            {
                index = index == noOfRows - 1 ? 2 : 1;
                for (var j = 0; j < noOfColumns; j++)
                {
                    var nodeBG = Instantiate(tileBasePrefab, new Vector2(startPos.x + j * tileWidth, startPos.y - i * tileHeight), Quaternion.identity);
                    nodeBG.transform.SetParent(this.transform);
                    nodeBG.gameObject.name = "NodeBG-" + i + "-" + j;
                    bgBoard[i, j] = nodeBG;
                    index++;
                }
            }
            SpawnNodes();
        }

        private void SpawnNodes()
        {
            for (int row = 0; row < noOfRows; row++)
            {
                for (int col = 0; col < noOfColumns; col++)
                {
                    NodeType randomNodeType = GetRandomNodeType();
                    Sprite nodeSprite = nodeSpritesDict[randomNodeType]; 
                    var node = Instantiate(nodePrefab, new Vector2(startPos.x + col * tileWidth, 
                        startPos.y - row * tileHeight), Quaternion.identity);
                    node.transform.SetParent(transform);
                    node.gameObject.name = "Node-" + row + "-" + col;
                    node.InitNode(randomNodeType, new NodeIndex(row, col), nodeSprite);
                    board[row, col] = node;
                }
            }
        }
         private NodeType GetRandomNodeType()
        {
            var index = Random.Range(0, nodeSprites.Length);
            return nodeSprites[index].nodeType;
        }

        public void CheckForMatches(Node node)
        {
            GService.SetTouchAvailable(false);

            if(GService.IsPowerupActivated)
            {
                ProcessPowerup(node);
                GService.TriggerOnPowerupComplete();
                GService.SetTouchAvailable(true);
                return;
            }
            List<Node> matches = new List<Node>();
            matches.Add(node);

            FindMatchesRecursively(node, matches);

            if (matches.Count >= 2)
            {
                ClearMatchesAndRefill(matches);
            }
            else
            {
                GService.SetTouchAvailable(true);
            }
        }

        private void FindMatchesRecursively(Node node, List<Node> matches)
        {
            CheckMatchesInDirection(node, Direction.up, matches);
            CheckMatchesInDirection(node, Direction.down, matches);
            CheckMatchesInDirection(node, Direction.left, matches);
            CheckMatchesInDirection(node, Direction.right, matches);
        }

        private void CheckMatchesInDirection(Node node, Direction direction, List<Node> matches)
        {
            Node neighbor = GetNeighbor(node, direction);

            if (neighbor != null && neighbor.Type == node.Type && !matches.Contains(neighbor))
            {
                matches.Add(neighbor);
                FindMatchesRecursively(neighbor, matches);
            }
        }
        private Node GetNeighbor(Node node, Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    {
                        if (node.Index.column == 0)
                        {
                            return null;
                        }
                        return board[node.Index.row, node.Index.column - 1];
                    }

                case Direction.right:
                    {
                        if (node.Index.column == noOfColumns - 1)
                        {
                            return null;
                        }
                        return board[node.Index.row, node.Index.column + 1];
                    }
                case Direction.up:
                    {
                        if (node.Index.row == 0)
                        {
                            return null;
                        }
                        return board[node.Index.row - 1, node.Index.column];
                    }
                case Direction.down:
                    {
                        if (node.Index.row == noOfRows - 1)
                        {
                            return null;
                        }
                        return board[node.Index.row + 1, node.Index.column];
                    }
                default:
                    return null;
            }
        }
        private void ProcessPowerup(Node node)
        {
            List<Node> matches = new() ;
            switch(GService.PowerupType)
            {
                case AbilityType.None:
                    break ;
                case AbilityType.Bomb:
                    matches =GetBombNodes(node);
                    break;
                case AbilityType.ColorDestroy:
                    matches = GetColorDestroyNodes(node);
                    break;
            }
            ClearMatchesAndRefill(matches);     
        }

        private List<Node> GetBombNodes(Node node)
        {
            List<Node> neighbors = new();
            neighbors.Add(node);

            int[] rowOffsets = { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
            int[] colOffsets = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };

            for (int i = 0; i < rowOffsets.Length; i++)
            {
                int newRow = node.Index.row + rowOffsets[i];
                int newCol = node.Index.column + colOffsets[i];

                if (IsValidPosition(newRow, newCol) && !(newRow == node.Index.row && newCol == node.Index.column))
                {
                    neighbors.Add(board[newRow, newCol]);
                }
            }
            return neighbors;
        }

        private void ClearMatchesAndRefill(List<Node> matches)
        {
            if (matches != null && matches.Count > 0)
            {
                if (clearCoroutine != null)
                {
                    StopCoroutine(clearCoroutine);
                }
                clearCoroutine = StartCoroutine(DestroyMatches(matches));
            }
            else
            {
                GService.SetTouchAvailable(true);
            }
        }
        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < noOfRows && col >= 0 && col < noOfColumns;
        }

        private List<Node> GetColorDestroyNodes(Node node)
        {
            var nodeType = node.Type;
            List<Node> nodesOfType = new List<Node>();

            for (int row = 0; row < noOfRows; row++)
            {
                for (int col = 0; col < noOfColumns; col++)
                {
                    if (board[row, col] != null && board[row, col].Type == nodeType)
                    {
                        nodesOfType.Add(board[row, col]);
                    }
                }
            }

            return nodesOfType;
        }
        private void OnDisable()
        {
            GService.OnStartGame -= SpawnBoard;
            clearCoroutine = null;
        }
        private IEnumerator DestroyMatches(List<Node> matches)
        {
            if (matches == null || matches.Count == 0)
            {
                GService.SetTouchAvailable(true);
                yield return null ;
            }

            yield return new WaitForSeconds(0.2f);
            foreach (var node in matches)
            {
                deletedNodes.Add(node.Index);
                GService.TriggerOnScoreChanged(GameConstants.SCORE_FOR_ONE_ITEM);
                Destroy(node.gameObject);
                board[node.Index.row, node.Index.column] = null;
            }

            yield return new WaitForSeconds(0.2f);
            ApplyGravityAndRefillBoard();
        }
        private void ApplyGravityAndRefillBoard()
        {
            HashSet<int> affectedColumns = new HashSet<int>();
            foreach (var nodeIndex in deletedNodes)
            {
                if (!affectedColumns.Contains(nodeIndex.column))
                {
                    affectedColumns.Add(nodeIndex.column);
                }
            }

            Queue<Node> availableItems = new();
            foreach (var col in affectedColumns)
            {
                int firstHoleRow = -1 ;
                
                for (int row = noOfRows-1; row >=0; row--)
                {
                    if(board[row,col] == null)
                    {
                        if(firstHoleRow ==-1)
                        {
                            firstHoleRow = row;
                        }
                        
                    }
                    else if(firstHoleRow >-1 && board[row, col] != null)
                    {
                        availableItems.Enqueue(board[row, col]);
                    }
                }

                for (int row = firstHoleRow; row >= 0; row--)
                {
                    if (board[row, col] == null)
                    {
                        if (availableItems.Count == 0)
                        {
                            SpawnNodeAtRowColumn(row, col);
                        }
                        else
                        {
                            var node = availableItems.Dequeue();
                            SetNodeAtRowColumn(node, row, col);
                        }
                    }
                }
                availableItems.Clear();
            }
            deletedNodes.Clear();
            GService.SetTouchAvailable(true);
        }

        private void SpawnNodeAtRowColumn(int row, int col)
        {
            NodeType randomNodeType = GetRandomNodeType();
            Sprite nodeSprite = nodeSpritesDict[randomNodeType];

            var node = Instantiate(nodePrefab, new Vector2(startPos.x + col * tileWidth,
                        startPos.y - row * tileHeight), Quaternion.identity);
            node.transform.SetParent(transform);
            node.gameObject.name = "Node-" + row + "-" + col;
            node.InitNode(randomNodeType, new NodeIndex(row, col), nodeSprite);
            board[row, col] = node;
        }

        private void SetNodeAtRowColumn(Node node,int row, int column)
        {
            board[node.Index.row, node.Index.column] = null;
            node.transform.position = new Vector2(startPos.x + column * tileWidth,
                        startPos.y - row * tileHeight);
            node.Index = new NodeIndex(row, column);
            node.gameObject.name = "Node-" + row + "-" + column;
            board[row, column] = node;
        }

    }
    [System.Serializable]
    public struct NodeSprites
    {
        public Sprite nodeSprite;
        public NodeType nodeType;
    }

    public enum Direction
    {
        left,
        right,
        up,
        down,
        invalid
    }

    public enum AbilityType
    {
        None,
        Bomb,
        ColorDestroy
    }

}
