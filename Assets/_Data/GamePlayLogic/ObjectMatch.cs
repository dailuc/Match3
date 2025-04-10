using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ObjectMatch : GamePlayManagerCtrlAbstract
{
    [Header("Object Match")]
    [SerializeField] protected bool hasMatch = false;
    [SerializeField]

    //protected override void Start()
    //{
    //    base.Start();
    //    Invoke(nameof(this.CheckFullBoard), 2f);
    //}
    public virtual List<Transform> ListDespawnMatch(Transform objStart, Transform objEnd)
    {

        Node nodeStart = GamePlayManagerCtrl.GridSystem.GetNodeByWorldPos(objStart.position);
        Node nodeEnd = GamePlayManagerCtrl.GridSystem.GetNodeByWorldPos(objEnd.position);

        Node[] nodeCheck = new Node[] { nodeStart, nodeEnd };
        List<Transform> mergedList = new List<Transform>();

        for (int i = 0; i < nodeCheck.Length; i++)
        {
            List<Transform> list = this.CheckMatch(nodeCheck[i]);
            mergedList.AddRange(list);
        }
        mergedList = mergedList.Distinct().ToList();

        return mergedList;
    }
   
    public virtual bool DespawnMatch(List<Transform> Objs)
    {
        if (Objs.Count == 0) return false;

        GridSystem gridSystem = GamePlayManagerCtrl.GridSystem;

        //string despawnedObjects = "Despawned objects: ";

        foreach (Transform obj in Objs)
        {
            Node node = gridSystem.GetNodeByObject(obj);
            node.obj = null;

           // despawnedObjects += obj.name + ", "; 

            FruitSpawner.Instance.Despawn(obj);
        }

      //  Debug.Log(despawnedObjects); 

        return true;
    }
    protected virtual List<Transform> CheckDirection(Node startNode, Transform obj, Func<Node, Node> nextNodeFunc)
    {
        List<Transform> result = new List<Transform>();
        if (startNode == null) return result;

        string targetName = obj.name;

        Node currentNode = startNode;
        while (currentNode != null)
        {
            Transform currentObj = currentNode.GetObject();
            if (currentObj == null) break;
            if (!currentObj.name.Contains(targetName)) break;

            result.Add(currentObj);
            currentNode = nextNodeFunc(currentNode);
        }

        return result;
    }

    protected virtual List<Transform> CheckHorizontal(Node node)
    {
        List<Transform> result = new List<Transform>();
        Transform obj = node.GetObject();
        result.Add(obj);

        result.AddRange(CheckDirection(node.left, obj, currentNode => currentNode.left));
        result.AddRange(CheckDirection(node.right, obj, currentNode => currentNode.right));

        return result;
    }


    protected virtual List<Transform> CheckVertical(Node node)
    {
        List<Transform> result = new List<Transform>();
        Transform obj = node.GetObject();
        result.Add(obj);

        result.AddRange(CheckDirection(node.up, obj, currentNode => currentNode.up));
        result.AddRange(CheckDirection(node.down, obj, currentNode => currentNode.down));

        return result;
    }


    protected virtual List<Transform> CheckMatch(Node nodeCheck)
    {
        List<List<Transform>> lists = new List<List<Transform>>
    {
        CheckVertical(nodeCheck),
        CheckHorizontal(nodeCheck)
    };

        lists.RemoveAll(list => list.Count < 3);
        return lists.SelectMany(list => list).ToList();
    }

    public virtual List<Transform> CheckFullBoard()
    {
        Node[,] nodes = GamePlayManagerCtrl.GridSystem.GetNodes();
        int height = nodes.GetLength(0);
        int width = nodes.GetLength(1);

        HashSet<Transform> matchedObjects = new HashSet<Transform>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node node = nodes[i, j];
                Transform obj = node.GetObject();
                if (obj == null || matchedObjects.Contains(obj)) continue;

                List<Transform> matchList = CheckMatch(node);
                foreach (Transform matchedObj in matchList)
                {
                    matchedObjects.Add(matchedObj);
                }
            }
        }

        return matchedObjects.ToList();
    }

}
