using System.Collections.Generic;
using UnityEngine;



namespace ZhangYu.Utilities     //�����ļ��������Ժ�������Ϸ�����ܻ��õ��ĺ��������ʱ����������������
{
    public class RandomPosition
    {
        Vector2 m_LeftDownPosition;     //�����������Ѳ������
        Vector2 m_RightTopPosition;


        public RandomPosition(Vector2 leftDwonPos, Vector2 rightTopPos)
        {
            m_LeftDownPosition = leftDwonPos;
            m_RightTopPosition = rightTopPos;
        }





        public Vector2 GenerateSingleRandomPos()
        {
            Vector2 newPos = new Vector2(Random.Range(m_LeftDownPosition.x, m_RightTopPosition.x), Random.Range(m_LeftDownPosition.y, m_RightTopPosition.y));

            return newPos;
        }

        public List<Vector2> GenerateMultiRandomPos(int num)
        {
            //Debug.Log("Num is " + num);

            if (num > 0)
            {
                List<Vector2> newPos = new List<Vector2>();

                bool isDone = false;

                for (int i = 0; i < num; i++)       //�Ƚ������������ɳ���
                {
                    newPos.Add(GenerateSingleRandomPos());
                }


                if (num > 1)    //������������1��ʱ����Ƿ����ظ�����
                {
                    while (!isDone)
                    {
                        isDone = true;

                        for (int i = 0; i < num - 1; i++)   //�ӵ�һ�����꿪ʼ��飬�����������ڶ������꣨���һ�����������飩
                        {
                            for (int j = i + 1; j < num; j++)       //ÿ�μ��ӵ�ǰ�������һ�����꿪ʼ�������һ�����꣨��ǰ����֮ǰ���Ѿ������ˣ�
                            {
                                if (IsOverlap(newPos[i], newPos[j]))
                                {
                                    newPos[i] = GenerateSingleRandomPos();    //����ظ��ˣ���������������

                                    isDone = false;
                                }
                            }
                        }
                    }
                }

                return newPos;
            }

            return null;
        }




        private bool IsOverlap(Vector2 firstPos, Vector2 secondPos)     //������������Ƿ񼸺��غ�
        {
            if ( (secondPos.x - firstPos.x <= Mathf.Abs(0.5f)) && (secondPos.y - firstPos.y <= Mathf.Abs(0.5f)) )
            {
                return true;
            }

            return false;
        }
    }
}
