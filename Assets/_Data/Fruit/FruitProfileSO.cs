using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fruits", menuName = "ScriptableObject/FruitProfile", order = 1)]
public class FruitProfileSO : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
}
