using UnityEngine;

public class Death : CoreComponent      //如果需要不同的死亡效果，则新建一个脚本，然会继承此脚本
{



    private void OnEnable()
    {
        Stats.OnHealthZero += Die;    //将函数加进事件
    }

    private void OnDisable()
    {
        Stats.OnHealthZero -= Die;    //物体禁用后从事件中移除函数，防止因为找不到函数所在的脚本而报错
    }




    public virtual void Die()
    {
        //core.transform.parent.gameObject.SetActive(false);  //禁用游戏物体

        Movement.Rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;        //防止死亡后物体还能接着移动

        core.Animator.SetBool("Death", true);
    }
}
