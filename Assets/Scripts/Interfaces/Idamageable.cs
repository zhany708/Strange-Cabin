using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idamageable
{
    void Damage(float amount);      //��������ֵ

    void GetHit(Vector2 direction);     //�ܻ��ƶ�
}
