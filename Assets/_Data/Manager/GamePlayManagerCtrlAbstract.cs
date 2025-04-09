using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayManagerCtrlAbstract : BaseMonoBehaviour
{
    [Header("GridManagerCtrlAbstract")]
    [SerializeField] private GamePlayManagerCtrl gridManagerCtrl;
    public GamePlayManagerCtrl GridManagerCtrl => gridManagerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGridManagerCtrl();
    }
    protected virtual void LoadGridManagerCtrl()
    {
        if (this.gridManagerCtrl != null) return;
        this.gridManagerCtrl = GetComponentInParent<GamePlayManagerCtrl>();
        Debug.LogWarning(transform.name + ": Load GridManagerCtrl ", gameObject);
    }

}
