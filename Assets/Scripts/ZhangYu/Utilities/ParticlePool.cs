using System.Collections.Generic;
using UnityEngine;

public class ParticlePool       //�����ӵ�����Ч�ȵĶ����
{
    public static ParticlePool Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ParticlePool();
            }
            return m_Instance;
        }
    }

    private static ParticlePool m_Instance;

    private Dictionary<string, Queue<GameObject>> m_ParticlePool = new Dictionary<string, Queue<GameObject>>();

    private GameObject m_Pool;



    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;

        if (!m_ParticlePool.ContainsKey(prefab.name) || m_ParticlePool[prefab.name].Count == 0)       //�������Ƿ������壬�����������Ƿ�Ϊ0
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if (m_Pool == null)
            {
                m_Pool = new GameObject("ParticlePool");   //���ɴ������гض�����ܸ�����
            }

            GameObject m_Child = GameObject.Find(prefab.name);
            if (!m_Child)
            {
                m_Child = new GameObject(prefab.name);     //����ÿ���ض���ĸ�����
                m_Child.transform.SetParent(m_Pool.transform);      //��ÿ������������Ϊ�ܸ������������
            }

            _object.transform.SetParent(m_Child.transform);     //��ÿ����������Ϊ�丸�����������
        }

        _object = m_ParticlePool[prefab.name].Dequeue();       //����Ԥ��������ȡ������е�Ԥ����
        _object.SetActive(true);
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);    //����¡��׺�滻�ɿ�

        if (!m_ParticlePool.ContainsKey(_name))
        {
            m_ParticlePool.Add(_name, new Queue<GameObject>());    //�����������Ƿ�����ڶ���أ�����������������һ��
        }
        m_ParticlePool[_name].Enqueue(prefab);     //���ɺ������������

        prefab.SetActive(false);    //�����ȡ������
    }
}