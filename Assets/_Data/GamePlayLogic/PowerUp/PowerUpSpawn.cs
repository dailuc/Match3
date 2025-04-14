using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSpawn : GamePlayManagerCtrlAbstract
{
    private GridSystem GridSystem;
    protected override void Start()
    {
        base.Start();
        this.GridSystem = this.GamePlayManagerCtrl.GridSystem;
    }
    public virtual void SpawnPowerUpObject(List<List<Transform>> spawnPowerUps)
    {
        if (spawnPowerUps.Count == 0) return;
        Node node;
        List<Node> nodes = new List<Node>();
        foreach (var matchs in spawnPowerUps)
        {
            nodes.Clear();
            foreach (var m in matchs) 
            {

                nodes.Add(this.GridSystem.GetNodeByWorldPos(m.position));
            }
            
            node = this.GridSystem.GetCenterPosition(nodes);
            Transform obj = FruitPowerUpSpawner.Instance.Spawn(this.CheckTypePowerUp(matchs),
                                 node.GetWorldPos(), Quaternion.identity);
            node.SetObject(obj);
            obj.gameObject.SetActive(true);
        }
    }
    public virtual void SpawnPowerUpObject(Dictionary<Node, List<Transform>> spawnPowerUpPerNode)
    {
        if (spawnPowerUpPerNode.Count == 0) return;

        foreach (var nodePos in spawnPowerUpPerNode)
        {
            Node node = nodePos.Key;
            Transform obj = FruitPowerUpSpawner.Instance.Spawn(this.CheckTypePowerUp(nodePos.Value),
                                 node.GetWorldPos(), Quaternion.identity);
            node.SetObject(obj);
            obj.gameObject.SetActive(true);
        }
    }
    protected virtual string CheckTypePowerUp(List<Transform> typePowerUp)
    {
        string type = "";
        type += typePowerUp[0].name;

        Node[] nodes = new Node[typePowerUp.Count];
        for(int i = 0; i < typePowerUp.Count; i++)
        {
            nodes[i] = this.GridSystem.GetNodeByWorldPos(typePowerUp[i].position);
        }
        if (nodes.Length == 5)
        {
            List<int> distinctX = nodes.Select(n => n.x).Distinct().ToList();
            List<int> distinctY = nodes.Select(n => n.y).Distinct().ToList();
            if (distinctX.Count == 1 || distinctY.Count == 1)
            {
                type += "_" + PowerUpCode.Bomb.ToString();
            }
            else
            {
                type += "_" + PowerUpCode.Bomb.ToString();
            }   
        }
        return type;
    }
}
