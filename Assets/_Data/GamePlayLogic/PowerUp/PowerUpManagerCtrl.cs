using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManagerCtrl : BaseMonoBehaviour
{
    [Header("PowerUpManagerCtrl")]
    [SerializeField] private PowerUpSpawn powerUpSpawn;
    public PowerUpSpawn PowerUpSpawn => powerUpSpawn;

    [SerializeField] private PowerUpBomb powerUpBomb;
    public PowerUpBomb PowerUpBomb => powerUpBomb;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPowerUpSpawn();
        this.LoadPowerUpBomb();
    }
    public virtual List<Transform> CheckPowerUpDespawn(List<Transform> listDespawn)
    {
        var DespawnPowerUp = new List<Transform>();
        foreach (Transform t in listDespawn)
        {
            if (t.name.Contains(PowerUpCode.Bomb.ToString()))
            {
                DespawnPowerUp.AddRange(powerUpBomb.PowerUpDespawn(t, PowerUpCode.Bomb));
            }
        }
        return DespawnPowerUp;
    }
    protected virtual void LoadPowerUpSpawn()
    {
        if (this.powerUpSpawn != null) return;
        this.powerUpSpawn = GetComponentInChildren<PowerUpSpawn>();
        Debug.LogWarning(transform.name + ": Load PowerUpSpawn ", gameObject);
    }
    protected virtual void LoadPowerUpBomb()
    {
        if (this.powerUpBomb != null) return;
        this.powerUpBomb = GetComponentInChildren<PowerUpBomb>();
        Debug.LogWarning(transform.name + ": Load PowerUpBomb ", gameObject);
    }

}
