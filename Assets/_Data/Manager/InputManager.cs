using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : BaseSingleton<InputManager>
{
    [Header("InputManager")]
    [SerializeField] private bool isAllowClick = true;
    [SerializeField] private Transform selectedObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] public static event Action<Transform, PowerUpCode> OnPowerUpActive;
    [SerializeField] public static event Action<Transform> SetObjectSwap;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCamera();
    }

    protected virtual void LoadCamera()
    {
        if (mainCamera != null) return;
        mainCamera = Camera.main;
        Debug.LogWarning(transform.name + ": Loaded MainCamera", gameObject);
    }

    protected virtual void Update()
    {
        if (!isAllowClick) return;

        this.HandleInput();
       // this.DragSelectedObject();
    }

    protected virtual void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropObject();
        }
    }

    private void SelectObject()
    {
        Transform hitObject = this.GetObjectUnderMouse();
        if (hitObject == null) return;

        switch (hitObject.tag)
        {
            case "Fruit":
                SetObjectSwap?.Invoke(hitObject);
                break;
        }
    }

    private void DropObject()
    {
        if (this.selectedObject == null) return;

        Transform[] hitObjects = this.GetAllObjectUnderMouse();
        if(hitObjects.Length == 0) return;
        for (int i = 0; i < hitObjects.Length; i++)
       
        this.selectedObject = null; 
    }

    private void DragSelectedObject()
    {
        if (this.selectedObject == null) return;

        Vector2 mousePos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        this.selectedObject.position = new Vector3(mousePos.x, mousePos.y, selectedObject.position.z);
    }

    public Transform GetObjectUnderMouse()
    {
        Vector2 mousePos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        return hit.collider != null ? hit.transform : null;
    }
    public Transform[] GetAllObjectUnderMouse()
    {
        Vector2 mousePos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero);
        Transform[] objects = new Transform[hit.Length];
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider == null) continue;
            objects[i] = hit[i].transform;
        }

        return objects;
    }

    public void SetSelectedObject(Transform obj)
    {
        this.selectedObject = obj;
    }

    public void EnableClick() => this.isAllowClick = true;

    public void DisableClick() => this.isAllowClick = false;

}
