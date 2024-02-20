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

        
        gameObject.SetActive(false);    //���ؽ������������
        Destroy(gameObject);
        

        //�Ƴ����棬��ʾ����û��
        if (UIManager.Instance.PanelDict.ContainsKey(name))
        {
            UIManager.Instance.PanelDict.Remove(name);
        }
    }


    protected void FadeIn(CanvasGroup thisCanvasGroup, float fadeDuration)   //���ڽ���ĵ���
    {     
        thisCanvasGroup.alpha = 0f;
        DOTween.To(() => thisCanvasGroup.alpha, x => thisCanvasGroup.alpha = x, 1f, fadeDuration);     //��1��֮�ڽ�͸���ȴ�0��Ϊ1��ʵ�ֵ���Ч��     
    }

    public virtual void FadeOut(CanvasGroup thisCanvasGroup, float fadeDuration)   //���ڽ���ĵ�������һ�ֹرս���ķ�ʽ��
    {
        isRemoved = true;      

        DOTween.To(() => thisCanvasGroup.alpha, x => thisCanvasGroup.alpha = x, 0, fadeDuration);   //��1��֮�ڽ�͸���ȴ�1��Ϊ0��ʵ�ֵ���Ч��     

        thisCanvasGroup.blocksRaycasts = false;     //ȡ���ڵ����ߣ���Ϊ����û�б����١�ֻ�ǿ������ˣ�

        //�Ƴ����棬��ʾ����û��
        if (UIManager.Instance.PanelDict.ContainsKey(name))
        {
            UIManager.Instance.PanelDict.Remove(name);
        }
    }





    protected virtual void DisplayText(TextMeshProUGUI thisText)        //��ʾ�ı�
    {
        if (thisText != null)
        {
            TypeWriterEffect(thisText, 0.05f);     //ÿ��0.05���һ����
        }
    }

    protected void TypeWriterEffect(TextMeshProUGUI textComponent, float typeSpeed)      //���ֻ�Ч��
    {
        string fullText = textComponent.text;   //�ȴ������ֶε���ʱ������Ȼ������ı��������
        textComponent.text = " ";

        StartCoroutine(TypeText(textComponent, fullText, typeSpeed));
    }




    protected IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float typeSpeed)      //���ڴ��ֻ���ÿ����һ��һ���������
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;       //ÿ��һ���ִ�����󣬵ȴ�һ��ʱ���ٴ���һ����
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    protected IEnumerator ClosePanelAfterDelay(float delay)      //�����ӳ�һ��ʱ����Զ��رս���
    {
        yield return new WaitForSeconds(delay);

        ClosePanel();       //����ִ�и�д���ĺ���
    }
}