using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInputController : BaseMonoBehaviour
{
    [Header("Player Input Controller")]
    [SerializeField] private bool isAllowClick = true;
    [SerializeField] private Transform selectedObject;
    [SerializeField] private Camera mainCamera;

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

        HandleInput();
        DragSelectedObject();
    }

    protected virtual void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryDropObject();
        }
    }

    private void TrySelectObject()
    {
        Transform hitObject = GetObjectUnderMouse();
        if (hitObject == null) return;

        switch (hitObject.tag)
        {
            case "Fruit":
                GamePlayManagerCtrl.Instance.ObjectHandle.SetObject(hitObject);
                break;

            case "PowerUp":
                selectedObject = hitObject;
                break;
        }
    }

    private void TryDropObject()
    {
        if (selectedObject == null) return;

        Transform[] hitObjects = GetAllObjectUnderMouse();
        if(hitObjects.Length == 0) return;

        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i] != null && hitObjects[i].CompareTag("Fruit"))
            {
                GamePlayManagerCtrl.Instance.PowerUpHandle.ActivePowerUp(hitObjects[i]); 
                break;
            }
        }
        PointSpawner.Instance.Despawn(this.selectedObject);
        selectedObject = null; 
    }

    private void DragSelectedObject()
    {
        if (selectedObject == null) return;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        selectedObject.position = new Vector3(mousePos.x, mousePos.y, selectedObject.position.z);
    }

    private Transform GetObjectUnderMouse()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        return hit.collider != null ? hit.transform : null;
    }
    private Transform[] GetAllObjectUnderMouse()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
        selectedObject = obj;
    }

    public void EnableClick() => isAllowClick = true;

    public void DisableClick() => isAllowClick = false;

}
