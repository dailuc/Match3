using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    //[Header("TestEditor")]

    protected virtual void Update()
    {
        this.CheckInputEditor();
    }
    protected virtual void CheckInputEditor() 
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    switch (keyCode)
                    {

                        case KeyCode.Alpha1:
                            this.SwapFruit(FruitSpawner.Apple, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha2:
                            this.SwapFruit(FruitSpawner.Blueberry, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha3:
                            this.SwapFruit(FruitSpawner.Grape, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha4:
                            this.SwapFruit(FruitSpawner.Orange, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha5:
                            this.SwapFruit(FruitSpawner.Pear, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha6:
                            this.SwapFruit(FruitSpawner.Strawberry, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        case KeyCode.Alpha7:
                            this.SwapPowerUpFruit(FruitPowerUpSpawner.Apple_Bomb, InputManager.Instance.GetObjectUnderMouse());
                            break;
                        default:
                            break;
                    }

                    break; 
                }
            }
        }
    }
    protected virtual void SwapFruit(string objName,Transform objSwap)
    {
        Node node = GamePlayManagerCtrl.Instance.GridSystem.GetNodeByObject(objSwap);
        Transform obj = FruitSpawner.Instance.Spawn(objName, objSwap.position, Quaternion.identity);
        obj.gameObject.SetActive(true);
        FruitSpawner.Instance.Despawn(objSwap);
        
        node.obj = null;
        node.obj = obj;

    }
    protected virtual void SwapPowerUpFruit(string objName, Transform objSwap)
    {
        Node node = GamePlayManagerCtrl.Instance.GridSystem.GetNodeByObject(objSwap);
        Transform obj = FruitPowerUpSpawner.Instance.Spawn(objName, objSwap.position, Quaternion.identity);
        obj.gameObject.SetActive(true);
        FruitSpawner.Instance.Despawn(objSwap);

        node.obj = null;
        node.obj = obj;

    }
}
