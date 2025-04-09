using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class ObjectClickable : BaseMonoBehaviour
{
    [Header("Object Clickable")]
    [SerializeField] public BoxCollider2D boxCollider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBoxColider();
    }

    protected virtual void LoadBoxColider()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.boxCollider.isTrigger = true;
        this.boxCollider.size = new Vector3(1.7f, 1.7f);
        Debug.LogWarning(transform.name + ": Load boxCollider ", gameObject);
    }
}
