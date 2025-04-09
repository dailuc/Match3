using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClickable : BaseMonoBehaviour
{
    [Header("PlayerClickable")]
    [SerializeField] protected bool isAllowClick = true;
    [SerializeField] protected Camera mainCamera;
    protected virtual void Update()
    {
        this.DetectClick();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCamera();
    }
    protected virtual void LoadCamera()
    {
        if (this.mainCamera != null) return;
        this.mainCamera = FindObjectOfType<Camera>();
        Debug.LogWarning(transform.name + ": Load Load MainCamera ", gameObject);
    }
    protected virtual void DetectClick()
    {
        if(!isAllowClick) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Debug.Log("hit: "+hit.collider.gameObject.name);
            if (hit.collider != null && hit.transform.tag == "Fruit")
            {
                GamePlayManagerCtrl.Instance.ObjectHandle.SetObject(hit.transform);
               
            }
        }
    }
    public void EnableClick()
    {
        isAllowClick = true;
    }
    public void DisableClick()
    {
        isAllowClick = false;
    }
}
