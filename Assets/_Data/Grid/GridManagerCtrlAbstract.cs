using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridManagerCtrlAbstract : BaseMonoBehaviour
{
    [Header("GridManagerCtrlAbstract")]
    [SerializeField] private GridManagerCtrl gridManagerCtrl;
    public GridManagerCtrl GridManagerCtrl => gridManagerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGridManagerCtrl();
    }
    protected virtual void LoadGridManagerCtrl()
    {
        if (this.gridManagerCtrl != null) return;
        this.gridManagerCtrl = GetComponentInParent<GridManagerCtrl>();
        Debug.LogWarning(transform.name + ": Load GridManagerCtrl ", gameObject);
    }

}
