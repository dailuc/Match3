using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandleGamePlay : GamePlayManagerCtrlAbstract
{
//    [Header("PowerUp Handle GamePlay ")]

    public virtual void ActivePowerUp(Transform Object )
    {
        List<Transform> affectedObjects = this.GamePlayManagerCtrl.PowerUpBom.CalculateExplosionArea(Object);
        Debug.Log(affectedObjects.Count);
        StartCoroutine(this.GamePlayManagerCtrl.ObjectHandle.ProcessChainReaction(affectedObjects));
    }
}
