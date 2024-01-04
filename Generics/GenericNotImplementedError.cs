using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericNotImplementedError<T>      //T��Ϊ��һ�ֱ������ͣ���ʾTest
{
    public static T TryGet(T value, string name)       //���ڼ���Ƿ���õ����ض����
    {
        if (value != null)
        {
            return value;
        }

        Debug.LogError(typeof(T) + " not implemented on" + name);
        return default;
    }
}
