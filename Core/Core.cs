using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }
    
   

    private readonly List<CoreComponent> m_CoreComponents = new List<CoreComponent>();      //������Core����ӽ�ȥ��readonly���ڱ���List����ֹ����ʱ��С�����¸�ֵ

    private void Awake()
    {
        Animator = GetComponentInParent<Animator>();        //���ø�����Ķ������������
    }


    public void LogicUpdate()
    {
        /*
        foreach (CoreComponent component in m_CoreComponents)
        {
            component.LogicUpdate();    //����ÿ���齨��LogicUpdate����
        }
        */
    }

    public void Addcomponent(CoreComponent component)
    {
        if (!m_CoreComponents.Contains(component))
        {
            m_CoreComponents.Add(component);     //����������List����ӽ�ȥ
        }
    }



    public T GetCoreComponent<T>() where T : CoreComponent      //T��������Generic����
    {
        var comp = m_CoreComponents.OfType<T>().FirstOrDefault();  //���ص�һ���ҵ���ֵ�����򷵻ػ���ֵ���󲿷ֱ������͵Ļ���ֵΪnull��

        if (comp) { return comp; }
        comp = GetComponentInChildren<T>();

        if (comp) { return comp; }
        
        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");      //�������Ƿ����
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;       //��������Ĳο�
    }
}
