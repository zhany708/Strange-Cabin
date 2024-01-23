using UnityEngine;


namespace ZhangYu.Utilities
{
    public class Flip
    {
        Transform m_Transform;

        int m_FlipNum;

        public Flip(Transform transform)
        {
            m_Transform = transform;
        }


        public void DoFlip(int flipNum)
        {
            m_FlipNum = flipNum;

            m_Transform.localScale = new Vector3(Mathf.Abs(m_Transform.localScale.x) * m_FlipNum, m_Transform.localScale.y, m_Transform.localScale.z);      //用于翻转角色
        }
    }
}
