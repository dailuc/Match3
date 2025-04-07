using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridSystem : GridManagerCtrlAbstract
{
    [Header("GridSystem")]
    [SerializeField] protected int width;
    public int Width => width;

    [SerializeField] protected int height;
    public int Height => height;

    [SerializeField] protected float offsetX;
    [SerializeField] protected float offsetY;
    [SerializeField] protected Transform prefab;
    [SerializeField] protected Node[,] nodes;
    protected override void Awake()
    {
        this.InitGridSystem();
        this.FindNodesNeighbors();
    }
    protected override void Start()
    {
        base.Start();
        this.SpawnObj();
    }
    protected virtual void InitGridSystem()
    {
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = new Node
                {
                    x = x,
                    y = y,
                    worldPosX = x * (1 + this.offsetX),
                    worldPosY = y * (1 + this.offsetY)
                };

                nodes[x, y] = node;
            }
        }
    }
    public virtual Node[,] GetNodes()
    {
        return nodes;
    }
    public virtual Vector2Int GetXYByWorldPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / (1 + this.offsetX));
        int y = Mathf.RoundToInt(worldPos.y / (1 + this.offsetY));
        return new Vector2Int(x, y);
    }
    public virtual Vector3 GetWorldPosByXY(int x, int y)
    {
        float worldPosX = x * (1 + this.offsetX);
        float worldPosY = y * (1 + this.offsetY);
        return new Vector3(worldPosX, worldPosY);
    }
    public virtual Node GetNodeByXY(int x, int y)
    {
        if (x < 0 || x >= this.width) return null;
        if (y < 0 || y >= this.height) return null;

        return this.nodes[x, y];
    }
    public virtual Node GetNodeByWorldPos(Vector3 worldPos)
    {
        Vector2Int XY = this.GetXYByWorldPos(worldPos);
        return this.GetNodeByXY(XY.x, XY.y);
    }
    protected virtual void FindNodesNeighbors()
    {
        int x, y;
        foreach (Node node in this.nodes)
        {
            x = node.x;
            y = node.y;
            node.up = this.GetNodeByXY(x, y + 1);
            node.right = this.GetNodeByXY(x + 1, y);
            node.down = this.GetNodeByXY(x, y - 1);
            node.left = this.GetNodeByXY(x - 1, y);
        }
    }
    protected virtual void SpawnObj()
    {
        Vector3 pos = Vector3.zero;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = nodes[x, y];

                pos.x = node.worldPosX;
                pos.y = node.worldPosY;

                List<string> notValidPrefabs = this.NotValidPrefabs(node);
                Transform obj = FruitSpawner.Instance.Spawn(FruitSpawner.Instance.RandomPrefabs(notValidPrefabs), pos, Quaternion.identity);
                node.occupied = true;
                obj.gameObject.SetActive(true);
            }
        }
    }
    protected virtual List<string> NotValidPrefabs(Node node)
    {
        Transform obj1;
        Transform obj2;
        List<string> notValidPrefabs = new List<string>();

        Node[] directions = { node.up, node.down, node.left, node.right };

        foreach (Node direction in directions)
        {
            obj1 = direction?.GetObjectAtPosition2D();

            if (direction == null || obj1 == null) continue;

            Node nextDirection = null;
            if (direction == node.up) nextDirection = direction.up;
            else if (direction == node.down) nextDirection = direction.down;
            else if (direction == node.left) nextDirection = direction.left;
            else if (direction == node.right) nextDirection = direction.right;

            obj2 = nextDirection?.GetObjectAtPosition2D();

            if (nextDirection == null || obj2 == null) continue;

            if (obj1.name == obj2.name)
            {
                notValidPrefabs.Add(obj1.name);
            }
        }
        return notValidPrefabs;
    }
}


