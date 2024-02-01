using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public AnimatorStateInfo AnimatorInfo { get; private set; }

    public float MaxHealth { get; private set; }
    public float Defense { get; private set; }
    public float HitResistance { get; private set; }     //���˿���


    private readonly List<CoreComponent> m_CoreComponents = new List<CoreComponent>();      //������Core����ӽ�ȥ��readonly���ڱ���List����ֹ����ʱ��С�����¸�ֵ






    private void Awake()
    {
        
        //Debug.Log("Core Awake");
        Animator = GetComponentInParent<Animator>();        //���ø�����Ķ������������

        if (!Animator)
        {
            Debug.LogError("Animator is missing!");
        }
        
    }






    public void LogicUpdate()
    {
        
        foreach (CoreComponent component in m_CoreComponents)
        {
            component.LogicUpdate();    //����ÿ���齨��LogicUpdate����
        }
        
        AnimatorInfo = Animator.GetCurrentAnimatorStateInfo(0);
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
        comp = GetComponentInChildren<T>();     //����Ҳ���������������Ѱ��

        if (comp) { return comp; }
        
        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");      //�������Ƿ����
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;       //��������Ĳο�
    }


    #region Setters
    public void SetParameters(float maxHealth, float defense, float hitResistamce)  //���ò������Թ�CoreComponent�е����ʹ��
    {
        MaxHealth = maxHealth;
        Defense = defense;
        HitResistance = hitResistamce;
    }
    #endregion
}
