using UnityEngine;


public class ParticleController : MonoBehaviour
{
    private void DestroyGameObject()    //���ڶ���֡�¼�
    {
        ParticlePool.Instance.PushObject(gameObject);
    }
}
