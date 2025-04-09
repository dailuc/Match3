using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node 
{
    public int x = 0;
    public int y = 0;
    public float worldPosX = 0;
    public float worldPosY = 0;
    public int weight = 1;
    public bool occupied = false;
    public Transform obj;
    public Node up;
    public Node right;
    public Node down;
    public Node left;

    public Transform GetObject()
    { 
        return obj;
    }
    public void SetObject(Transform obj)
    {
        this.obj = obj;
    }
    public void SwapObject(Node node)
    {
        Transform tempObj = this.obj;
        this.obj = node.GetObject();
        node.SetObject(tempObj);
    }
    public void StealObject(Node node)
    {
        this.obj = node.obj;
        node.obj = null;
    }
    public Vector3 GetWorldPos()
    {
        return new Vector3(worldPosX, worldPosY);
    }
    public bool IsObjectActive()
    {
        if (this.obj == null)
        {
            return false;
        }
        return this.obj.gameObject.activeSelf;
    }

}
