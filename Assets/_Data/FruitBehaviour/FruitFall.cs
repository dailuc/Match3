using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFall : GamePlayManagerCtrlAbstract
{
    [Header("Fruit Fall")]
    [SerializeField] protected float speedDrop = 8f;

    public virtual void FindFruitDrop()
    {
        Node[,] nodes = GridManagerCtrl.GridSystem.GetNodes();
        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);
        Transform obj;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node node = nodes[i, j];
                if (node.IsObjectActive()) continue;
                bool hasMoved = false;
                Node NodeUp = node.up;
                while (NodeUp != null)
                {
                    obj = NodeUp.GetObject();
                    if (NodeUp.IsObjectActive())
                    {
                        node.StealObject(NodeUp);
                       // StartCoroutine(MoveToPosition(obj, node.GetWorldPos()));
                        obj.position = node.GetWorldPos();
                        hasMoved = true;
                        break;
                    }
                    NodeUp = NodeUp.up;
                }
                if (!hasMoved)
                {
                    obj = FruitSpawner.Instance.Spawn(FruitSpawner.Instance.RandomPrefabs(),
                                                        node.GetWorldPos(), Quaternion.identity);
                    node.SetObject(obj);
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }
    protected virtual IEnumerator MoveToPosition(Transform obj, Vector3 target)
    {
        yield return new WaitForSeconds(0.2f);
        while (Vector3.Distance(obj.position, target) > 0.01f)
        {
            obj.position = Vector3.Lerp(obj.position, target, this.speedDrop * Time.deltaTime);
            yield return null;
        }
        obj.position = target;
    }

}
