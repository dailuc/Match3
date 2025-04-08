using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHandle : GridManagerCtrlAbstract
{
    [Header("FruitHandle")]
    [SerializeField] protected Transform fruitStart;
    [SerializeField] protected Transform fruitEnd;   

    [SerializeField] protected Vector3 targetPosEnd;
    [SerializeField] protected Vector3 targetPosStart;
    [SerializeField] protected float swapSpeed = 5f;
    [SerializeField] protected bool isSwapping = false;
    [SerializeField] protected bool isCanSwap = false;
    [SerializeField] protected bool OnBusy = false;

    protected virtual void Update()
    {
        this.CheckCanSwap();
        this.PrepareSwap();
        this.Swapping();
    }
    public virtual void SetFruit(Transform fruit)
    {
        if (this.OnBusy) return;
        if (this.fruitStart == null)
        {
            this.fruitStart = fruit;
            return;
        }
        if (this.fruitEnd == null)
        {
            this.fruitEnd = fruit;
            return;
        }
    }
    protected virtual void CheckCanSwap()
    {
        if (this.fruitStart == null || this.fruitEnd == null) return;

            Node nodeStart = GridManagerCtrl.GridSystem.GetNodeByWorldPos(fruitStart.position);
            Node nodeEnd = GridManagerCtrl.GridSystem.GetNodeByWorldPos(fruitEnd.position);
        if(nodeStart == nodeEnd) { this.ReSetObject(); return; }
        if (nodeStart.up != nodeEnd
            && nodeStart.down != nodeEnd
            && nodeStart.left != nodeEnd
            && nodeStart.right != nodeEnd) { this.ReSetObject(); return; }
       
        this.isCanSwap = true;
    }
    protected virtual void PrepareSwap()
    {
        if (!this.isCanSwap) return;
        if (this.isSwapping) return;

        this.targetPosStart = fruitStart.position;
        this.targetPosEnd = fruitEnd.position;  

        this.isSwapping = true;
    }
    protected virtual void Swapping()
    {
        if (!this.isCanSwap) return;
        if (!this.isSwapping) return;

        this.SwapFruit();
        List<Transform> ListDespawn;
        if (IsSwappingFinish())
        {
            bool hasDespawn = false;

            if (!this.OnBusy)
            {
                ListDespawn = GridManagerCtrl.FruitMatch.ListDespawnMatch(fruitStart, fruitEnd);
                if(ListDespawn.Count > 0)
                {
                    hasDespawn = true;
                    this.ReSetObject();
                    this.GridManagerCtrl.FruitMatch.DespawnMatch(ListDespawn);
                }
            }
            if (!hasDespawn && !this.OnBusy)
            {
                Transform tempFruit = this.fruitStart;
                this.fruitStart = this.fruitEnd;
                this.fruitEnd = tempFruit;
                this.OnBusy = true;
            }else
            {
                this.ReSetObject();
                this.isSwapping = false;
                this.isCanSwap = false;
                this.OnBusy = false; 
            }
        }
    }
    protected virtual void SwapFruit()
    {
        if (Vector3.Distance(fruitStart.position, targetPosEnd) > 0.01f)
        {
            fruitStart.position = Vector3.MoveTowards(fruitStart.position, targetPosEnd, swapSpeed * Time.deltaTime);
        }
        else
        {
            fruitStart.position = targetPosEnd;
        }

        if (Vector3.Distance(fruitEnd.position, targetPosStart) > 0.01f)
        {
            fruitEnd.position = Vector3.MoveTowards(fruitEnd.position, targetPosStart, swapSpeed * Time.deltaTime);
        }
        else
        {
            fruitEnd.position = targetPosStart;
        }
    }
    protected virtual void ReSetObject()
    {
        this.fruitStart = null;
        this.fruitEnd = null;
    }
    protected virtual bool IsSwappingFinish()
    {
        return (fruitStart.position == targetPosEnd && fruitEnd.position == targetPosStart);
    }
}
