using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwap : GamePlayManagerCtrlAbstract
{
    [Header("Object Swap")]
    [SerializeField] protected Transform ObjectStart;
    [SerializeField] protected Transform ObjectEnd;
    [SerializeField] protected Transform hightLightStart;
    [SerializeField] protected Transform hightLightEnd;

    [SerializeField] protected Vector3 targetPosEnd;
    [SerializeField] protected Vector3 targetPosStart;
    [SerializeField] protected float swapSpeed = 10f;
    [SerializeField] protected bool isSwapping = false;
    [SerializeField] protected bool isCanSwap = false;
    [SerializeField] protected bool OnSwapBack = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputManager.SetObjectSwap += SetObject;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        InputManager.SetObjectSwap -= SetObject;
    }
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
            hightLightStart = VFXSpawner.Instance.Spawn(VFXSpawner.ClickHightLight,
                                                Object.position, Quaternion.identity);
            hightLightStart.gameObject.SetActive(true);
            hightLightStart.SetParent(Object.transform);
            return;
        }
        if (this.ObjectEnd == null)
        {
            this.ObjectEnd = Object;
            hightLightEnd = VFXSpawner.Instance.Spawn(VFXSpawner.ClickHightLight,
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

        InputManager.Instance.DisableClick();
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

        SwapMatchResult swapMatchResult = GamePlayManagerCtrl.ObjectMatch.SwapMatch(ObjectStart, ObjectEnd);

        bool hasAnyMatch = false;
        this.ProcessSwap(swapMatchResult, ref hasAnyMatch);

        if (!hasAnyMatch)
        {
            SwapBack();
        }
    }
    protected virtual void ProcessSwap(SwapMatchResult swapMatchResult, ref bool hasAnyMatch)
    {
        List<Transform> mergedList = new List<Transform>();
        List<Node> nodeSpawnPowerUp = new List<Node>();


        this.ProcessResult(swapMatchResult, ref mergedList, ref nodeSpawnPowerUp, ref hasAnyMatch);
        if (hasAnyMatch)
        {
            this.ReSetObject();
            FinishSwapping();

            mergedList.AddRange(swapMatchResult.listStart);
            mergedList.AddRange(swapMatchResult.listEnd);

            StartCoroutine(GamePlayManagerCtrl.ObjectHandle.ProcessChainReaction(mergedList, nodeSpawnPowerUp));
        }
    }
    protected virtual void ProcessResult(SwapMatchResult swapMatchResult, ref List<Transform> mergedList
                                        , ref List<Node> nodeSpawnPowerUp, ref bool hasAnyMatch)
    {
        var listToCheck = new List<(List<Transform> list, Node node)>
            {
                (swapMatchResult.listStart, swapMatchResult.nodeStart),
                (swapMatchResult.listEnd, swapMatchResult.nodeEnd)
            };
        foreach (var (list, node) in listToCheck)
        {
            if (list.Count > 0)
            {
                hasAnyMatch = true;
                if (list.Count >= 5)
                {
                    nodeSpawnPowerUp.Add(node);
                }
            }
        }
    }
    protected virtual void FinishSwapBack()
    {
        InputManager.Instance.EnableClick();
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
        VFXSpawner.Instance.Despawn(hightLightStart);
        VFXSpawner.Instance.Despawn(hightLightEnd);

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
