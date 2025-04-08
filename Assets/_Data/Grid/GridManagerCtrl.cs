using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerCtrl : BaseSingleton<GridManagerCtrl>
{
    [SerializeField] protected FruitHandle fruitHandle;
    public FruitHandle FruitHandle => fruitHandle;

    [SerializeField] protected GridSystem gridSystem;
    public GridSystem GridSystem => gridSystem;

    [SerializeField] protected FruitMatch fruitMatch;
    public FruitMatch FruitMatch => fruitMatch;

    [SerializeField] protected FruitFall fruitFall;
    public FruitFall FruitFall => fruitFall;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadFruitHandle();
        this.LoadGridSystem();
        this.LoadFruitMatch();
        this.LoadFruitFall();
    }
    protected virtual void LoadFruitHandle()
    {
        if (this.fruitHandle != null) return;
        this.fruitHandle = GetComponentInChildren<FruitHandle>();
        Debug.LogWarning(transform.name + ": Load FruitHandle ", gameObject);
    }
    protected virtual void LoadGridSystem()
    {
        if (this.gridSystem != null) return;
        this.gridSystem = GetComponentInChildren<GridSystem>();
        Debug.LogWarning(transform.name + ": Load GridSystem ", gameObject);
    }
    protected virtual void LoadFruitMatch()
    {
        if (this.fruitMatch != null) return;
        this.fruitMatch = GetComponentInChildren<FruitMatch>();
        Debug.LogWarning(transform.name + ": Load FruitMatch ", gameObject);
    }
    protected virtual void LoadFruitFall()
    {
        if (this.fruitFall != null) return;
        this.fruitFall = GetComponentInChildren<FruitFall>();
        Debug.LogWarning(transform.name + ": Load FruitFall ", gameObject);
    }
}
