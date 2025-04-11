using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpColorBomb : BasePowerUp
{
    public override void ActivePowerUp(Transform ObjectActive, PowerUpCode code)
    {
        if (code != PowerUpCode.ColorBomb) return;
        List<Transform> allColorObjects = this.FindAllColorObjects(ObjectActive);
       // StartCoroutine(this.GamePlayManagerCtrl.ObjectHandle.ProcessChainReaction(allColorObjects));
    }
    protected virtual List<Transform> FindAllColorObjects(Transform obj)
    {
        Debug.Log(obj.name);
        Node[,] nodes = GamePlayManagerCtrl.GridSystem.GetNodes();
        int height = nodes.GetLength(0);
        int width = nodes.GetLength(1);

        List<Transform> allColorObjects = new List<Transform>();
        string targetName = obj.name;
        Node node;
        Transform currentObj;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                node = nodes[i, j];
                currentObj = node.obj;
                if(currentObj ==  null || !currentObj.name.Contains(targetName)) continue;
                allColorObjects.Add(currentObj);
            }
        }
        Debug.Log(allColorObjects.Count);
        return allColorObjects;
    }
    
}
