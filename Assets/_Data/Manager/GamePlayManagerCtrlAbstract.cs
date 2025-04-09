using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayManagerCtrlAbstract : BaseMonoBehaviour
{
    [Header("GamePlayManagerCtrlAbstract")]
    [SerializeField] private GamePlayManagerCtrl gamePlayManagerCtrl;
    public GamePlayManagerCtrl GamePlayManagerCtrl => gamePlayManagerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGridManagerCtrl();
    }
    protected virtual void LoadGridManagerCtrl()
    {
        if (this.gamePlayManagerCtrl != null) return;
        this.gamePlayManagerCtrl = GetComponentInParent<GamePlayManagerCtrl>();
        Debug.LogWarning(transform.name + ": Load GamePlayManagerCtrl ", gameObject);
    }

}
