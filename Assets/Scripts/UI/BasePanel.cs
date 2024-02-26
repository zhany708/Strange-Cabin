using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePanel : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    public CanvasGroup CanvasGroup      //Lazy Loading��ֻ����Ҫʹ�����ʱ�ż����������������Awake������Ĭ�ϼ��أ�����ʡ�ڴ棩
    {
        get
        {
            if (m_CanvasGroup == null)
            {
                m_CanvasGroup = GetComponent<CanvasGroup>();
            }
            return m_CanvasGroup;
        }
    }



    protected PlayerInputHandler playerInputHandler;

    protected bool isTyping = false;        //��ʾ�Ƿ�������ʾ�ı�

    protected bool isRemoved = false;       //��ʾUI�Ƿ��Ƴ�
    protected new string name;








    protected virtual void Awake() 
    {
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();     //Ѱ����PlayerInputHandler���������
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
            isTyping = true;        //��ʾ���ڴ��֣���ֹ���ڴ���ʱ���ո��ر�UI��

            TypeWriterEffect(thisText, 0.05f);     //ÿ��0.05���һ����
        }
    }

    protected void TypeWriterEffect(TextMeshProUGUI textComponent, float typeSpeed)      //���ֻ�Ч��
    {
        StartCoroutine(TypeText(textComponent, textComponent.text, typeSpeed));
    }




    protected IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float typeSpeed)      //���ڴ��ֻ���ÿ����һ��һ���������
    {
        textComponent.text = " ";       //������ı��������

        foreach (char letter in fullText.ToCharArray())     //�����ı����ÿһ��Ԫ�أ������֡����š��ո�ȣ�
        {
            if (playerInputHandler.IsSpacePressed)
            {
                textComponent.text = fullText;      //��Ұ��¿ո��ֱ����ʾȫ������

                yield return new WaitForSeconds(0.5f);      //0.5��������ò�������ֹ��ʾ���������ֺ�UI������ʧ�����鿴�����ClosePanelAfterDelay������
                isTyping = false;

                yield break;    //�˳�Э��
            }


            textComponent.text += letter;       

            yield return new WaitForSeconds(typeSpeed);     //ÿ��һ���ִ�����󣬵ȴ�һ��ʱ���ټ������У�����һ���֣�
        }

        isTyping = false;       //�ı�ȫ�����������isTyping����Ϊfalse
    }

    protected IEnumerator ClosePanelAfterDelay(float delay)      //�����ӳ�һ��ʱ����Զ��رս���
    {
        float elapsedTime = 0;

        while (elapsedTime < delay)
        {
            if (playerInputHandler.IsSpacePressed && !isTyping)     //����ӳٹ����У����ı�����ʾ��ϵ��������Ұ��¿ո��������ر�UI
            {
                ClosePanel();

                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ClosePanel();       //����ִ�и�д���ĺ���
    }
}