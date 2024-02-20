using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public CanvasGroup CanvasGroup {  get; private set; }

    protected bool isRemoved = false;
    protected new string name;








    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }



    public virtual void OpenPanel(string name)
    {
        this.name = name;
    }

    public virtual void ClosePanel()
    {
        Debug.Log("Panel is closed!");

        isRemoved = true;

        
        gameObject.SetActive(false);    //隐藏界面后销毁物体
        Destroy(gameObject);
        

        //移除缓存，表示界面没打开
        if (UIManager.Instance.PanelDict.ContainsKey(name))
        {
            UIManager.Instance.PanelDict.Remove(name);
        }
    }


    protected void FadeIn(CanvasGroup thisCanvasGroup, float fadeDuration)   //用于界面的淡入
    {     
        thisCanvasGroup.alpha = 0f;
        DOTween.To(() => thisCanvasGroup.alpha, x => thisCanvasGroup.alpha = x, 1f, fadeDuration);     //在1秒之内将透明度从0变为1，实现淡入效果     
    }

    public virtual void FadeOut(CanvasGroup thisCanvasGroup, float fadeDuration)   //用于界面的淡出（另一种关闭界面的方式）
    {
        isRemoved = true;      

        DOTween.To(() => thisCanvasGroup.alpha, x => thisCanvasGroup.alpha = x, 0, fadeDuration);   //在1秒之内将透明度从1变为0，实现淡出效果     

        thisCanvasGroup.blocksRaycasts = false;     //取消遮挡射线（因为物体没有被销毁。只是看不见了）

        //移除缓存，表示界面没打开
        if (UIManager.Instance.PanelDict.ContainsKey(name))
        {
            UIManager.Instance.PanelDict.Remove(name);
        }
    }





    protected virtual void DisplayText(TextMeshProUGUI thisText)        //显示文本
    {
        if (thisText != null)
        {
            TypeWriterEffect(thisText, 0.05f);     //每隔0.05秒打一个字
        }
    }

    protected void TypeWriterEffect(TextMeshProUGUI textComponent, float typeSpeed)      //打字机效果
    {
        string fullText = textComponent.text;   //先储存文字段到临时变量，然后清空文本里的文字
        textComponent.text = " ";

        StartCoroutine(TypeText(textComponent, fullText, typeSpeed));
    }




    protected IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float typeSpeed)      //用于打字机（每个字一个一个打出来）
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;       //每当一个字打出来后，等待一段时间再打下一个字
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    protected IEnumerator ClosePanelAfterDelay(float delay)      //用于延迟一段时间后自动关闭界面
    {
        yield return new WaitForSeconds(delay);

        ClosePanel();       //优先执行改写过的函数
    }
}