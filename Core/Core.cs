using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }
    
   

    private readonly List<CoreComponent> m_CoreComponents = new List<CoreComponent>();      //将所有Core组件加进去。readonly用于保护List，防止运行时不小心重新赋值

    private void Awake()
    {
        Animator = GetComponentInParent<Animator>();        //调用父物体的动画控制器组件
    }


    public void LogicUpdate()
    {
        /*
        foreach (CoreComponent component in m_CoreComponents)
        {
            component.LogicUpdate();    //运行每个组建的LogicUpdate函数
        }
        */
    }

    public void Addcomponent(CoreComponent component)
    {
        if (!m_CoreComponents.Contains(component))
        {
            m_CoreComponents.Add(component);     //如果组件不在List，则加进去
        }
    }



    public T GetCoreComponent<T>() where T : CoreComponent      //T代表这是Generic函数
    {
        var comp = m_CoreComponents.OfType<T>().FirstOrDefault();  //返回第一个找到的值，否则返回基础值（大部分变量类型的基础值为null）

        if (comp) { return comp; }
        comp = GetComponentInChildren<T>();

        if (comp) { return comp; }
        
        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");      //检查组件是否存在
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;       //返还组件的参考
    }
}
