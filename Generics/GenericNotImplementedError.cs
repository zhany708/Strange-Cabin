using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericNotImplementedError<T>      //T成为了一种变量类型，表示Test
{
    public static T TryGet(T value, string name)       //用于检测是否调用到了特定组件
    {
        if (value != null)
        {
            return value;
        }

        Debug.LogError(typeof(T) + " not implemented on" + name);
        return default;
    }
}
