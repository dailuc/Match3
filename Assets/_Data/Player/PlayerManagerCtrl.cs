using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerCtrl : BaseSingleton<PlayerManagerCtrl>
{
    [Header("PlayerManagerCtrl")]
    [SerializeField] private PlayerInputController playerClickable;
    public PlayerInputController PlayerClickable => playerClickable;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerClickable();
    }
    protected virtual void LoadPlayerClickable()
    {
        if (this.playerClickable != null) return;
        this.playerClickable = GetComponentInChildren<PlayerInputController>();
        Debug.LogWarning(transform.name + ": Load PlayerClickable ", gameObject);
    }
}
