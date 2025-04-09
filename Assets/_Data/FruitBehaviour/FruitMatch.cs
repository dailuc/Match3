using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FruitMatch : GamePlayManagerCtrlAbstract
{
    [Header("FruitMatch")]
    [SerializeField] protected bool hasMatch = false;
    [SerializeField]

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(this.Checkfullboard), 2f);
    }
    public virtual List<Transform> ListDespawnMatch(Transform objStart, Transform objEnd)
    {

        Node nodeStart = GridManagerCtrl.GridSystem.GetNodeByWorldPos(objStart.position);
        Node nodeEnd = GridManagerCtrl.GridSystem.GetNodeByWorldPos(objEnd.position);

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
    protected virtual List<Transform> CheckMatch(Node nodeCheck)
    {
        List<List<Transform>> lists = new List<List<Transform>>
                                    {this.CheckVertical(nodeCheck),
                                    this.CheckHorizontal(nodeCheck)};
        lists.RemoveAll(list => list.Count < 3);
        List<Transform> mergedList = lists.SelectMany(list => list).ToList();

        return mergedList;
    }
    public virtual bool DespawnMatch(List<Transform> Objs)
    {
        if (Objs.Count == 0) return false;
        GridSystem gridSystem = GridManagerCtrl.GridSystem;
        foreach (Transform obj in Objs)
        {
            Node node = gridSystem.GetNodeByObject(obj);
            node.obj = null;
            FruitSpawner.Instance.Despawn(obj);
        }
        return true;
    }
    protected virtual List<Transform> CheckHorizontal(Node node)
    {
        List<Transform> horizontalObj = new List<Transform>();

        Transform obj = node.GetObject();
        horizontalObj.Add(obj);

        Node leftNode = node.left;
        horizontalObj.AddRange(this.CheckLeft(leftNode, obj));

        Node rightNode = node.right;
        horizontalObj.AddRange(this.CheckRight(rightNode, obj));

        return horizontalObj;
    }
    protected virtual List<Transform> CheckVertical(Node node)
    {

        List<Transform> verticalObj = new List<Transform>();


        Transform obj = node.GetObject();
        verticalObj.Add(obj);

        Node upNode = node.up;
        verticalObj.AddRange(this.CheckUp(upNode, obj));

        Node downNode = node.down;
        verticalObj.AddRange(this.CheckDown(downNode, obj));

        return verticalObj;
    }
    protected virtual List<Transform> CheckRight(Node rightNode, Transform obj)
    {
        List<Transform> horizontalObj = new List<Transform>();
        if (rightNode == null) return horizontalObj;

        while (rightNode != null)
        {
            Transform objRight = rightNode.GetObject();
            if (objRight == null) break;
            if (objRight.name != obj.name) break;

            horizontalObj.Add(objRight);
            rightNode = rightNode.right;
        }
        return horizontalObj;
    }
    protected virtual List<Transform> CheckLeft(Node leftNode, Transform obj)
    {
        List<Transform> horizontalObj = new List<Transform>();
        if (leftNode == null) return horizontalObj;

        while (leftNode != null)
        {
            Transform objLeft = leftNode.GetObject();
            if (objLeft == null) break;
            if (objLeft.name != obj.name) break;

            horizontalObj.Add(objLeft);
            leftNode = leftNode.left;
        }
        return horizontalObj;
    }
    protected virtual List<Transform> CheckUp(Node upNode, Transform obj)
    {
        List<Transform> verticalObj = new List<Transform>();
        if (upNode == null) return verticalObj;

        while (upNode != null)
        {
            Transform objUp = upNode.GetObject();
            if (objUp == null) break;
            if (objUp.name != obj.name) break;

            verticalObj.Add(objUp);
            upNode = upNode.up;
        }
        return verticalObj;
    }
    protected virtual List<Transform> CheckDown(Node downNode, Transform obj)
    {
        List<Transform> verticalObj = new List<Transform>();
        if (downNode == null) return verticalObj;

        while (downNode != null)
        {
            Transform objDown = downNode.GetObject();
            if (objDown == null) break;
            if (objDown.name != obj.name) break;

            verticalObj.Add(objDown);
            downNode = downNode.down;
        }
        return verticalObj;
    }
    public virtual List<Transform> Checkfullboard()
    {
        Node[,] nodes = GridManagerCtrl.GridSystem.GetNodes();

        int height = nodes.GetLength(0);
        int width = nodes.GetLength(1);

        List<Transform> verticalObj = new List<Transform>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node node = nodes[i, j];
                Transform obj = node.GetObject();

                if (verticalObj.Contains(obj)) continue;
                var upList = this.CheckUp(node, obj);
                if (upList.Count >= 3)
                {
                    verticalObj.AddRange(upList);
                }
            }
        }

        List<Transform> horizontalObj = new List<Transform>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Node node = nodes[i, j];
                Transform obj = node.GetObject();

                if (horizontalObj.Contains(obj)) continue;
                var rightList = this.CheckRight(node, obj);
                if (rightList.Count >= 3)
                {
                    horizontalObj.AddRange(rightList);
                }
            }
        }
        verticalObj.AddRange(horizontalObj);
        verticalObj = verticalObj.Distinct().ToList();
        return verticalObj;
    }

}
