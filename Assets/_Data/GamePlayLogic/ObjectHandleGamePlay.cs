using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ObjectHandleGamePlay : GamePlayManagerCtrlAbstract
{
    // [Header("Object Handle GamePlay")]

    public IEnumerator ProcessChainReaction(List<Transform> listDespawn, Dictionary<Node, List<Transform>> spawnPowerUpPerNode)
    {
        InputManager.Instance.DisableClick();

        List<Transform> checkDespawnPowerUp = this.GamePlayManagerCtrl.PowerUpManagerCtrl.CheckPowerUpDespawn(listDespawn);

        if (checkDespawnPowerUp.Count > 0)
        {
            listDespawn = listDespawn.Concat(checkDespawnPowerUp).Distinct().ToList();
        }

        this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(listDespawn);

        try { this.GamePlayManagerCtrl.PowerUpManagerCtrl.PowerUpSpawn.SpawnPowerUpObject(spawnPowerUpPerNode); }
        catch { }


        yield return new WaitForSeconds(0.3f);

        List<Node> nodeDrop;
        List<List<Transform>> allMatches;
        List<Transform> mergedMatches;
        List<Transform> powerUpDespawnList = new List<Transform>();
        List<List<Transform>> spawnPowerUps = new List<List<Transform>>();
        int loopCount = 0;
        while (loopCount < 100)
        {
            loopCount++;
            powerUpDespawnList.Clear();
            spawnPowerUps.Clear();
            nodeDrop = this.GamePlayManagerCtrl.ObjectFall.FindObjectDrop();

            yield return StartCoroutine(this.GamePlayManagerCtrl.ObjectFall.MoveObjsToItsNode(nodeDrop));
            yield return new WaitForSeconds(0.3f);

            allMatches = this.GamePlayManagerCtrl.ObjectMatch.CheckFullBoard();

            if (allMatches.Count == 0)
                break;

            foreach (var matchs in allMatches)
            {
                checkDespawnPowerUp = this.GamePlayManagerCtrl.PowerUpManagerCtrl.CheckPowerUpDespawn(matchs);
                if (checkDespawnPowerUp.Count > 0)
                {
                    powerUpDespawnList.AddRange(checkDespawnPowerUp);
                }
            }

            mergedMatches = powerUpDespawnList
                .Concat(allMatches.SelectMany(list => list))
                .Distinct()
                .ToList();
            this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(mergedMatches);
            foreach (var matchs in allMatches)
            {

                if(matchs.Count >= 5) 
                {
                    spawnPowerUps.Add(matchs);
                }
            }

            try { this.GamePlayManagerCtrl.PowerUpManagerCtrl.PowerUpSpawn.SpawnPowerUpObject(spawnPowerUps); }
            catch { }

            yield return new WaitForSeconds(0.3f);

        }

        InputManager.Instance.EnableClick();
    }

}
