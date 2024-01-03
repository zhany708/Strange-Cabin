using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();       //�Ӹ������������Core���

        if (!core)
        {
            Debug.LogError("There is no Core on the parent");
        }
    }
}
