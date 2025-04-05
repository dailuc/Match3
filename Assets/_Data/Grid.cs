using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected float cellSize;
    [SerializeField] protected Vector3 originPosition;

    public Grid(int width, int height, float cellSize, Transform[,] gridObjects)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        Debug.Log(this.originPosition);
        Debug.Log("width: "+width+" "+"height"+height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridObjects[i,j].position = GetWorldPosition(i,j);
            }
        }
    }
    public virtual Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x,y) * this.cellSize + originPosition;
    }
}
