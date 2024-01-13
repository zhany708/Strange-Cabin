using System.Collections.Generic;
using UnityEngine;

public class EnemyPool       //用于生成敌人的对象池
{
    public static EnemyPool Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new EnemyPool();
            }
            return m_Instance;
        }
    }

    private static EnemyPool m_Instance;

    private Dictionary<string, Queue<GameObject>> m_EnemyPool = new Dictionary<string, Queue<GameObject>>();

    private GameObject m_Pool;



    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;

        if (!m_EnemyPool.ContainsKey(prefab.name) || m_EnemyPool[prefab.name].Count == 0)       //检查池中是否有物体，或物体数量是否为0
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if (m_Pool == null)
            {
                m_Pool = new GameObject("EnemyPool");   //生成储存所有池对象的总父物体
            }

            GameObject m_ChildPool = GameObject.Find(prefab.name + "Pool");
            if (!m_ChildPool)
            {
                m_ChildPool = new GameObject(prefab.name + "Pool");     //生成每个池对象的父物体
                m_ChildPool.transform.SetParent(m_Pool.transform);      //将每个父物体设置为总父物体的子物体
            }

            _object.transform.SetParent(m_ChildPool.transform);     //将每个物体设置为其父物体的子物体
        }

        _object = m_EnemyPool[prefab.name].Dequeue();       //按照预制体名获取对象池中的预制体
        _object.SetActive(true); 
        return _object;       
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone", string.Empty);    //将克隆后缀替换成空

        if (!m_EnemyPool.ContainsKey(_name))
        {
            m_EnemyPool.Add(_name, new Queue<GameObject>());    //查找物体名是否存在于对象池，若不存在则新生成一个
        }
        m_EnemyPool[_name].Enqueue(prefab);     //生成后将物体放入对象池

        prefab.SetActive(false);    //放入后取消激活
    }
}
