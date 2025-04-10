using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ObjectHandleGamePlay : GamePlayManagerCtrlAbstract
{
    [Header("Object Handle GamePlay")]
    [SerializeField] protected Transform ObjectStart;
    [SerializeField] protected Transform ObjectEnd;    
    [SerializeField] protected Transform hightLightStart;
    [SerializeField] protected Transform hightLightEnd;

    [SerializeField] protected Vector3 targetPosEnd;
    [SerializeField] protected Vector3 targetPosStart;
    [SerializeField] protected float swapSpeed = 5.5f;
    [SerializeField] protected bool isSwapping = false;
    [SerializeField] protected bool isCanSwap = false;
    [SerializeField] protected bool OnSwapBack = false;

    protected virtual void Update()
    {
        this.CheckCanSwap();
        this.PrepareSwap();
    }
    protected virtual void FixedUpdate()
    {
        this.Swapping();
    }
    public virtual void SetObject(Transform Object)
    {
        if (this.OnSwapBack) return;
        if (this.ObjectStart == null)
        {
            this.ObjectStart = Object;
            hightLightStart = HightLightSpawner.Instance.Spawn(HightLightSpawner.ClickHightLight,
                                                Object.position, Quaternion.identity);
            hightLightStart.gameObject.SetActive(true);
            hightLightStart.SetParent(Object.transform);
            return;
        }
        if (this.ObjectEnd == null)
        {
            this.ObjectEnd = Object;
            hightLightEnd = HightLightSpawner.Instance.Spawn(HightLightSpawner.ClickHightLight,
                                                Object.position, Quaternion.identity);
            hightLightEnd.gameObject.SetActive(true);
            hightLightEnd.SetParent(Object.transform);
            return;
        }
    }
    protected virtual void CheckCanSwap()
    {
        if (this.ObjectStart == null || this.ObjectEnd == null) return;

        Node nodeStart = GamePlayManagerCtrl.GridSystem.GetNodeByWorldPos(ObjectStart.position);
        Node nodeEnd = GamePlayManagerCtrl.GridSystem.GetNodeByWorldPos(ObjectEnd.position);
        if (nodeStart == nodeEnd) { this.ReSetObject(); return; }
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

        this.targetPosStart = ObjectStart.position;
        this.targetPosEnd = ObjectEnd.position;

        PlayerManagerCtrl.Instance.PlayerClickable.DisableClick();
        this.isSwapping = true;

    }
    protected virtual void SwapObject()
    {
        if (Vector3.Distance(ObjectStart.position, targetPosEnd) > 0.01f)
        {
            ObjectStart.position = Vector3.MoveTowards(ObjectStart.position, targetPosEnd, swapSpeed * Time.deltaTime);
        }
        else
        {
            ObjectStart.position = targetPosEnd;
        }

        if (Vector3.Distance(ObjectEnd.position, targetPosStart) > 0.01f)
        {
            ObjectEnd.position = Vector3.MoveTowards(ObjectEnd.position, targetPosStart, swapSpeed * Time.deltaTime);
        }
        else
        {
            ObjectEnd.position = targetPosStart;
        }
    }
    protected virtual void Swapping()
    {
        if (!this.isCanSwap || !this.isSwapping) return;
        

        this.SwapObject();

        if (!IsSwappingFinish()) return;

        if (this.OnSwapBack)
        {
            FinishSwapBack();
            return;
        }

        List<Transform> listDespawn = GamePlayManagerCtrl.ObjectMatch.ListDespawnMatch(ObjectStart, ObjectEnd);

        if (listDespawn.Count > 0)
        {
            this.ReSetObject();
            FinishSwapping();
            StartCoroutine(ProcessChainReaction(listDespawn));
        }
        else
        {
            SwapBack();
        }
    }
    public IEnumerator ProcessChainReaction(List<Transform> listDespawn)
    {
        PlayerManagerCtrl.Instance.PlayerClickable.DisableClick();

        this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(listDespawn);
        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            List<Node> nodeDrop = this.GamePlayManagerCtrl.ObjectFall.FindObjectDrop();

            yield return StartCoroutine(this.GamePlayManagerCtrl.ObjectFall.MoveObjsToItsNode(nodeDrop));
            yield return new WaitForSeconds(0.3f);

            List<Transform> list = this.GamePlayManagerCtrl.ObjectMatch.CheckFullBoard();

            if (list.Count == 0)
                break;

            this.GamePlayManagerCtrl.ObjectMatch.DespawnMatch(list);
            yield return new WaitForSeconds(0.3f);

        }

        PlayerManagerCtrl.Instance.PlayerClickable.EnableClick();
    }
    protected virtual void FinishSwapBack()
    {
        PlayerManagerCtrl.Instance.PlayerClickable.EnableClick();
        this.ReSetObject();
        this.FinishSwapping();
    }
    protected virtual void SwapBack()
    {
        Transform temp = this.ObjectStart;
        this.ObjectStart = this.ObjectEnd;
        this.ObjectEnd = temp;
        this.OnSwapBack = true;
    }

    protected virtual void FinishSwapping()
    {
        this.isSwapping = false;
        this.isCanSwap = false;
        this.OnSwapBack = false;
    }
    protected virtual void ReSetObject()
    {
        HightLightSpawner.Instance.Despawn(hightLightStart);
        HightLightSpawner.Instance.Despawn(hightLightEnd);

        this.ObjectStart = null;
        this.ObjectEnd = null;
    }
    protected virtual bool IsSwappingFinish()
    {
      
        if (ObjectStart.position == targetPosEnd && ObjectEnd.position == targetPosStart)
        {
            this.GamePlayManagerCtrl.GridSystem.SwapNode(ObjectStart, ObjectEnd);
            return true;
        }
        return false;
            
    }
   
}
