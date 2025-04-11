using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerUp : GamePlayManagerCtrlAbstract
{
    //    [Header("PowerUp Handle GamePlay ")]
    protected override void OnEnable()
    {
        base.OnEnable();
        InputManager.OnPowerUpActive += ActivePowerUp;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        InputManager.OnPowerUpActive -= ActivePowerUp;
    }
    public abstract void ActivePowerUp(Transform ObjectActive,PowerUpCode code);
}
