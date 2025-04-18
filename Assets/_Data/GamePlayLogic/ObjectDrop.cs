using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectDrop : GamePlayManagerCtrlAbstract
{
    [Header("Object Drop")]
    [SerializeField] protected float speed = 20f;

    public virtual List<Node> FindObjectDrop()
    {
        Node[,] nodes = GamePlayManagerCtrl.GridSystem.GetNodes();
        List<Node> nodeObjectsFound = new List<Node>();
        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);
        Transform obj;
        for (int i = 0; i < width; i++)
        {
            List<Node> nodeObjectsNotFound = new List<Node>();
            for (int j = 0; j < height; j++)
            {
                Node node = nodes[i, j];
                if (node.IsObjectActive()) continue;
                bool hasObject = false;
                Node NodeUp = node.up;
                while (NodeUp != null)
                {
                    obj = NodeUp.GetObject();
                    if (NodeUp.IsObjectActive())
                    {
                        node.StealObject(NodeUp);
                        nodeObjectsFound.Add(node);
                        hasObject = true;
                        break;
                    }
                    NodeUp = NodeUp.up;
                }
                if (!hasObject)
                {
                    nodeObjectsNotFound.Add(node);
                }
            }
            this.SpawnObjectToNode(nodeObjectsNotFound);
            nodeObjectsFound.AddRange(nodeObjectsNotFound);
        }
        return nodeObjectsFound;
    }
    protected virtual void SpawnObjectToNode(List<Node> nodeObjectsNotFound)
    {
        for (int index = 0; index < nodeObjectsNotFound.Count; index++)
        {
            Node node = nodeObjectsNotFound[index];
            Vector3 pos = GamePlayManagerCtrl.GridSystem.PositionSpawn(node, index);
            Transform obj = FruitSpawner.Instance.Spawn(FruitSpawner.Instance.RandomPrefabs()
                                                    , pos,Quaternion.identity);

            node.SetObject(obj);
            obj.gameObject.SetActive(true);
        }
    }
    public virtual IEnumerator MoveObjsToItsNode(List<Node> nodes)
    {
        int movingCount = nodes.Count;

        foreach (Node node in nodes)
        {
            StartCoroutine(MoveSingleObj(node, () => movingCount--));
        }
        while (movingCount > 0)
        {
            yield return null;
        }
    }

    private IEnumerator MoveSingleObj(Node node, Action onComplete)
    {
        Transform obj = node.GetObject();
        Vector3 targetPos = node.GetWorldPos();

        while (Vector3.Distance(obj.position, targetPos) > 0.01f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPos, speed * Time.fixedDeltaTime);
            yield return null;
        }

        obj.position = targetPos;
        onComplete?.Invoke();
    }
}
