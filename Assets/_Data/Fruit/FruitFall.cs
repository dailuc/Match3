using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFall : GridManagerCtrlAbstract
{
    [Header("Fruit Fall")]
    [SerializeField] protected List<Transform> objsMove = new List<Transform>();

    //protected override void Start()
    //{
    //    base.Start();
    //    Invoke(nameof(this.FindFruitDrop), 10f);
    //}
    
    public virtual void FindFruitDrop()
    {
        Node[,] nodes = GridManagerCtrl.GridSystem.GetNodes();
        Transform obj;
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Node node = nodes[i, j];
                if(node == null) continue;
                if (!node.occupied) 
                {
                    Node currentNode = node.up;
                    while (currentNode != null) 
                    {
                       obj = currentNode?.GetObjectAtPosition2D();
                        if (obj == null && objsMove.Contains(obj) == false)
                        {
                            currentNode = currentNode.up;
                            continue;
                        } 
                        objsMove.Add(obj);
                        StartCoroutine(MoveToPosition(obj, new Vector3(node.worldPosX, node.worldPosY), 5f));
                        currentNode.occupied = false;
                        node.occupied = true;
                        return;
                    }
                    Debug.Log("not found");
                }
            }
        }        
    }
    private IEnumerator MoveToPosition(Transform obj, Vector3 target, float speed)
    {
        while (Vector3.Distance(obj.position, target) > 0.01f)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
            yield return null;
        }
        obj.position = target;
        objsMove.Remove(obj);
    }

}
