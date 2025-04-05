using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] protected Transform prefab;
    [SerializeField] protected Transform[,] prefabs;
    private void Awake()
    {
        prefabs = new Transform[4, 2];
        for (int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                GameObject obj = Instantiate(prefab.gameObject);
                this.prefabs[i,j] = obj.transform;
            }
        }
        Grid grid = new Grid(4, 2, 2f, prefabs);
    }
}
