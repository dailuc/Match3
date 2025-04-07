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
    public Node up;
    public Node right;
    public Node down;
    public Node left;

    public Transform GetObjectAtPosition2D()
    {
        Vector2 worldPosition = new Vector2(this.worldPosX, this.worldPosY);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.transform;
        }

        return null;
    }
}
