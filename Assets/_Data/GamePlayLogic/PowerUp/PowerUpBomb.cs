using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBomb : BasePowerUp
{
    public override void ActivePowerUp(Transform ObjectActive, PowerUpCode code)
    {
        if (code != PowerUpCode.Bomb) return;
        List<Transform> affectedObjects = this.CalculateExplosionArea(ObjectActive);
      //  StartCoroutine(this.GamePlayManagerCtrl.ObjectHandle.ProcessChainReaction(affectedObjects));
    }
    public virtual List<Transform> CalculateExplosionArea(Transform obj)
    {
        List<Transform> affectedObjects = new List<Transform>();
        if (obj == null) return affectedObjects;

        Node centerNode = GamePlayManagerCtrl.GridSystem.GetNodeByObject(obj);
        affectedObjects.Add(centerNode.obj);
        // 4 main directions
        if (centerNode.up != null) affectedObjects.Add(centerNode.up.obj);
        if (centerNode.down != null) affectedObjects.Add(centerNode.down.obj);
        if (centerNode.left != null) affectedObjects.Add(centerNode.left.obj);
        if (centerNode.right != null) affectedObjects.Add(centerNode.right.obj);

        // 4 diagonal directions
        if (centerNode.up.left != null) affectedObjects.Add(centerNode.up.left.obj);
        if (centerNode.up.right != null) affectedObjects.Add(centerNode.up.right.obj);
        if (centerNode.down.left != null) affectedObjects.Add(centerNode.down.left.obj);
        if (centerNode.down.right != null) affectedObjects.Add(centerNode.down.right.obj);

        return affectedObjects;
    }
  
}
