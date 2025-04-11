using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ObjectHandleGamePlay : GamePlayManagerCtrlAbstract
{
   // [Header("Object Handle GamePlay")]
   
    public IEnumerator ProcessChainReaction(List<Transform> listDespawn,List<Node> nodeSpawnPowerUp)
    {
        InputManager.Instance.DisableClick();

        this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(listDespawn);

        this.SpawnPowerUpObject(nodeSpawnPowerUp);

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            List<Node> nodeDrop = this.GamePlayManagerCtrl.ObjectFall.FindObjectDrop();

            yield return StartCoroutine(this.GamePlayManagerCtrl.ObjectFall.MoveObjsToItsNode(nodeDrop));
            yield return new WaitForSeconds(0.3f);

            List<Transform> list = this.GamePlayManagerCtrl.ObjectMatch.CheckFullBoard();

            if (list.Count == 0)
                break;

            this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(list);
            yield return new WaitForSeconds(0.3f);

        }

        InputManager.Instance.EnableClick();
    }  
    protected virtual void SpawnPowerUpObject(List<Node> nodeSpawnPowerUp)
    {
        if (nodeSpawnPowerUp.Count > 0)
        {
            foreach (var nodePos in nodeSpawnPowerUp)
            {
                Transform obj = FruitPowerUpSpawner.Instance.Spawn(FruitPowerUpSpawner.Apple_Bomb,
                                     nodePos.GetWorldPos(), Quaternion.identity);
                nodePos.SetObject(obj);
                obj.gameObject.SetActive(true);
            }
        }
    }
}
