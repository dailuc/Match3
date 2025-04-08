
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : BaseSingleton<T> where T : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform holder;
    public Transform Holder => holder;

    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected List<Transform> poolObjs;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadHolder();
        this.LoadPrefabs();
    }
    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
        Debug.LogWarning(transform.name + ": Load Holder ", gameObject);
    }
    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;
        Transform prefabsObj = transform.Find("Prefabs");
        foreach (Transform child in prefabsObj)
        {
            this.prefabs.Add(child);
        }
        this.HidePrefabs();
        Debug.LogWarning(transform.name + ": Load Prefabs ", gameObject);
    }
    protected virtual void HidePrefabs()
    {
        foreach (Transform child in prefabs)
        {
            child.gameObject.SetActive(false);
        }
    }
    public virtual Transform Spawn(string prefabName, Vector3 pos, Quaternion rot)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }
        return this.Spawn(prefab, pos, rot);
    }
    public virtual Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(pos, rot);
        newPrefab.SetParent(this.holder);
        return newPrefab;
    }
    public virtual void Despawn(List<Transform> objs)
    {
        foreach (Transform child in objs)
        {
            Despawn(child);
        }
    }
    public virtual void Despawn(Transform obj)
    {
        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
    }
    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (var poolObj in poolObjs)
        {
            if (poolObj.name == prefab.name)
                return poolObj;
        }
        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    protected virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform child in prefabs)
        {
            if (child.name == prefabName) return child;
        }
        return null;
    }
    public virtual Transform RandomPrefabs()
    {
        int rand = Random.Range(0, this.prefabs.Count);
        return this.prefabs[rand];
    }
    public virtual Transform RandomPrefabsWithoutInvalid(List<string> preFabNames)
    {
        List<Transform> availablePrefabs = new List<Transform>(this.prefabs);
        if (preFabNames.Count > 0)
        {
            foreach (Transform prefab in this.prefabs)
            {
                foreach (string preFabName in preFabNames)
                {
                    if (prefab.name == preFabName)
                        availablePrefabs.Remove(prefab);
                }
            }

        }
        int rand = Random.Range(0, availablePrefabs.Count);
        return availablePrefabs[rand];
    }
}
