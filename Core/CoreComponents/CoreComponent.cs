using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();       //从父物体那里调用Core组件

        if (!core)
        {
            Debug.LogError("There is no Core on the parent");
        }
    }
}
