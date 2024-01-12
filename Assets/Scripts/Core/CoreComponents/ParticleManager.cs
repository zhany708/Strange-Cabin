using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : CoreComponent    //����������Ч
{
    Transform m_ParticleContainer;

    protected override void Awake()
    {
        base.Awake();

        m_ParticleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;      //��ȡ��Ч������������
    }


    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, quaternion rotation)
    {
        return Instantiate(particlePrefab, position, rotation, m_ParticleContainer);     //������Ч,���ĸ�����Ϊ��������ĸ�����
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, quaternion.identity);      //������Ч����������
    }

    public GameObject StartParticleWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));      //��������Ƕ�

        return StartParticles(particlePrefab, transform.position, randomRotation);
    }
}
