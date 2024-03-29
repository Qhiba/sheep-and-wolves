using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
            }
            return instance;
        }
    }
    
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tile walkableTile;

    private NodeData[,] nodes;

    private void Awake()
    {
        RealocateNodeToGrid();
        SearchAndSetNodeNeighbor();
    }

    public Tilemap GetTilemap()
    {
        return groundTilemap;
    }

    private void RealocateNodeToGrid()
    {
        BoundsInt cellBound = groundTilemap.cellBounds;
        Vector3Int offset = new Vector3Int(cellBound.min.x, cellBound.min.y, 0);

        nodes = new NodeData[cellBound.size.x, cellBound.size.y];

        for (int x = 0; x < cellBound.size.x; x++)
        {
            for (int y = 0; y < cellBound.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0) + offset;
                TileBase tile = groundTilemap.GetTile(cellPos);

                if (tile == null)
                {
                    Debug.Log("Tile is " + tile);
                    continue;
                }

                NodeData node = new NodeData(groundTilemap, new Vector3Int(x, y, 0), cellPos);
                node.isWalkable = (tile == walkableTile ? true : false);

                if(tile == walkableTile)
                {
                    node.isWalkable = true;
                }


                nodes[x, y] = node;
            }
        }

        Debug.Log(string.Format("Tile Size : {0} -- Node Size : {1}, {2}", cellBound.size, nodes.GetLength(0), nodes.GetLength(1)));
    }

    public Vector3Int GetGridSize()
    {
        return new Vector3Int(nodes.GetLength(0), nodes.GetLength(1), 0);
    }

    public Vector3 GetCellSize()
    {
        return groundTilemap.cellSize;
    }

    public NodeData[,] GetAllNodeData()
    {
        return nodes;
    }

    public NodeData GetNode(int x, int y)
    {
        return nodes[x, y];
    }

    public NodeData GetNode(Vector3 worldPosition)
    {
        Vector3Int offset = new Vector3Int(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.min.y, 0);
        Vector3Int nodeCell = groundTilemap.WorldToCell(worldPosition);
        Vector3Int gridPos = nodeCell - offset;

        if (gridPos.x >= nodes.GetLength(0) || gridPos.x < 0 ||
            gridPos.y >= nodes.GetLength(1) || gridPos.y < 0)
        {
            return null;
        }

        return GetNode(gridPos.x, gridPos.y);
    }

    private void SearchAndSetNodeNeighbor()
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                SearchAndSetNodeNeighbor(nodes[x, y]);
            }
        }
    }

    private void SearchAndSetNodeNeighbor(NodeData node)
    {
        Vector3Int nodeGridPos = node.GetGridPos();

        if (nodeGridPos.x - 1 >= 0)
        {
            // Left Down Position
            if (nodeGridPos.y - 1 >= 0)
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y - 1]);
            }

            // Left Up Position
            if (nodeGridPos.y + 1 < nodes.GetLength(1))
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y + 1]);
            }

            // Left Position
            node.SetNeighborNodes(nodes[nodeGridPos.x - 1, nodeGridPos.y]);
        }

        if (nodeGridPos.x + 1 < nodes.GetLength(0))
        {
            // Right Down Position
            if (nodeGridPos.y - 1 >= 0)
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y - 1]);
            }

            // Right Up Position
            if (nodeGridPos.y + 1 < nodes.GetLength(1))
            {
                node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y + 1]);
            }

            // Right Position
            node.SetNeighborNodes(nodes[nodeGridPos.x + 1, nodeGridPos.y]);
        }

        // Down Position
        if (nodeGridPos.y - 1 >= 0)
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x, nodeGridPos.y - 1]);
        }

        // Up Position
        if (nodeGridPos.y + 1 < nodes.GetLength(1))
        {
            node.SetNeighborNodes(nodes[nodeGridPos.x, nodeGridPos.y + 1]);
        }
    }
}