using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : CoreComponent    //用于生成特效
{
    Transform m_ParticleContainer;

    protected override void Awake()
    {
        base.Awake();

        m_ParticleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;      //获取特效管理器的坐标
    }


    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, quaternion rotation)
    {
        return Instantiate(particlePrefab, position, rotation, m_ParticleContainer);     //生成特效,第四个参数为生成物体的父物体
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, quaternion.identity);      //生成特效于物体中心
    }

    public GameObject StartParticleWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));      //生成随机角度

        return StartParticles(particlePrefab, transform.position, randomRotation);
    }
}
