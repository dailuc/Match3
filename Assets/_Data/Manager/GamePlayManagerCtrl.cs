using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManagerCtrl : BaseSingleton<GamePlayManagerCtrl>
{
    [Header("GamePlayManagerCtrl")]
    [SerializeField] protected ObjectHandleGamePlay objectHandle;
    public ObjectHandleGamePlay ObjectHandle => objectHandle;

    [SerializeField] protected GridSystem gridSystem;
    public GridSystem GridSystem => gridSystem;

    [SerializeField] protected ObjectMatch objectMatch;
    public ObjectMatch ObjectMatch => objectMatch;

    [SerializeField] protected ObjectDrop objectFall;
    public ObjectDrop ObjectFall => objectFall;

    [SerializeField] protected ObjectSwap objectSwap;
    public ObjectSwap ObjectSwap => objectSwap;

    [SerializeField] protected PowerUpManagerCtrl powerUpManagerCtrl;
    public PowerUpManagerCtrl PowerUpManagerCtrl => powerUpManagerCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjectHandle();
        this.LoadGridSystem();
        this.LoadObjectMatch();
        this.LoadObjectDrop();
        this.LoadObjectSwap();
        this.LoadPowerUpManagerCtrl();
    }
    protected virtual void LoadObjectHandle()
    {
        if (this.objectHandle != null) return;
        this.objectHandle = GetComponentInChildren<ObjectHandleGamePlay>();
        Debug.LogWarning(transform.name + ": Load Object Handle ", gameObject);
    }
    protected virtual void LoadGridSystem()
    {
        if (this.gridSystem != null) return;
        this.gridSystem = GetComponentInChildren<GridSystem>();
        Debug.LogWarning(transform.name + ": Load GridSystem ", gameObject);
    }
    protected virtual void LoadObjectMatch()
    {
        if (this.objectMatch != null) return;
        this.objectMatch = GetComponentInChildren<ObjectMatch>();
        Debug.LogWarning(transform.name + ": Load Object Match ", gameObject);
    }
    protected virtual void LoadObjectDrop()
    {
        if (this.objectFall != null) return;
        this.objectFall = GetComponentInChildren<ObjectDrop>();
        Debug.LogWarning(transform.name + ": Load Object Drop ", gameObject);
    }
    protected virtual void LoadObjectSwap()
    {
         if (this.objectSwap != null) return;
        this.objectSwap = GetComponentInChildren<ObjectSwap>();
        Debug.LogWarning(transform.name + ": Load Object Swap ", gameObject);
    }
    protected virtual void LoadPowerUpManagerCtrl()
    {
        if (this.powerUpManagerCtrl != null) return;
        this.powerUpManagerCtrl = GetComponentInChildren<PowerUpManagerCtrl>();
        Debug.LogWarning(transform.name + ": Load PowerUpManagerCtrl ", gameObject);
    }
}
