using System.Collections.Generic;
using UnityEngine;

public class EnemyPool       //�������ɵ��˵Ķ����
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

        if (!m_EnemyPool.ContainsKey(prefab.name) || m_EnemyPool[prefab.name].Count == 0)       //�������Ƿ������壬�����������Ƿ�Ϊ0
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if (m_Pool == null)
            {
                m_Pool = new GameObject("EnemyPool");   //���ɴ������гض�����ܸ�����
            }

            GameObject m_ChildPool = GameObject.Find(prefab.name + "Pool");
            if (!m_ChildPool)
            {
                m_ChildPool = new GameObject(prefab.name + "Pool");     //����ÿ���ض���ĸ�����
                m_ChildPool.transform.SetParent(m_Pool.transform);      //��ÿ������������Ϊ�ܸ������������
            }

            _object.transform.SetParent(m_ChildPool.transform);     //��ÿ����������Ϊ�丸�����������
        }

        _object = m_EnemyPool[prefab.name].Dequeue();       //����Ԥ��������ȡ������е�Ԥ����
        _object.SetActive(true); 
        return _object;       
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone", string.Empty);    //����¡��׺�滻�ɿ�

        if (!m_EnemyPool.ContainsKey(_name))
        {
            m_EnemyPool.Add(_name, new Queue<GameObject>());    //�����������Ƿ�����ڶ���أ�����������������һ��
        }
        m_EnemyPool[_name].Enqueue(prefab);     //���ɺ������������

        prefab.SetActive(false);    //�����ȡ������
    }
}
