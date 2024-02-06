using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : CoreComponent    //����������Ч
{




    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, quaternion rotation)
    {
        GameObject particle = ParticlePool.Instance.GetObject(particlePrefab);
        particle.transform.position = position;
        particle.transform.rotation = rotation;

        return particle;
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
