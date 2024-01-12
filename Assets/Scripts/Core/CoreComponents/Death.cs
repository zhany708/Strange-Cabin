using UnityEngine;

public class Death : CoreComponent      //�����Ҫ��ͬ������Ч�������½�һ���ű���Ȼ��̳д˽ű�
{
    //[SerializeField] private GameObject[] m_DeathParticles;



    private void OnEnable()
    {
        Stats.OnHealthZero += Die;    //�������ӽ��¼�
    }

    private void OnDisable()
    {
        Stats.OnHealthZero -= Die;    //������ú���¼����Ƴ���������ֹ��Ϊ�Ҳ����������ڵĽű�������
    }




    public virtual void Die()
    {
        /*
        foreach (var particle in m_DeathParticles)
        {
            m_Manager.StartParticles(particle);
        }
        */

        //core.transform.parent.gameObject.SetActive(false);  //������Ϸ����

        Movement.Rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;        //��ֹ���������廹�ܽ����ƶ�

        core.Animator.SetBool("Death", true);
    }
}
