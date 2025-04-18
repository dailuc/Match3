using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        //
    }

    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }
    protected virtual void OnEnable()
    {
        
    }
    protected virtual void OnDisable()
    {
        
    }
    protected virtual void LoadComponents()
    {
        //For override
    }

    protected virtual void ResetValue()
    {
        //For override
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
