using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCtrl : BaseMonoBehaviour
{
    [Header("FruitCtrl")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Transform model;
    [SerializeField] protected FruitProfileSO fruitProfile;
    public FruitProfileSO FruitProfile => fruitProfile;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadFruitProfile();
        this.LoadSprite();
    }
    protected virtual void LoadFruitProfile()
    {
        if (this.fruitProfile != null) return;
        this.fruitProfile = Resources.Load<FruitProfileSO>("Fruits");
        Debug.LogWarning(transform.name + ": Load FruitProfile ", gameObject);
    }

    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
        spriteRenderer = this.model.GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load model ", gameObject);
    }
    protected virtual void LoadSprite()
    {
        if (spriteRenderer.sprite != null) return;
        foreach(Sprite sprite in FruitProfile.sprites)
        {
            Debug.Log(sprite.name);
            if(sprite.name == gameObject.name)
            {
                
                spriteRenderer.sprite = sprite;
                break;
            }
        }
        Debug.LogWarning(transform.name + ": Load Sprite ", gameObject);
    }
}
