using TMPro;
using DG.Tweening;
using UnityEngine;


public class TransitionStagePanel : BasePanel
{
    TextMeshProUGUI m_TransitionStageText;      //�ı����

    float m_DisplayDuration = 10f;





    protected override void Awake()
    {
        base.Awake();

        m_TransitionStageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OpenPanel("TransitionStagePanel");
    }




    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        FadeIn(CanvasGroup, 1f);     //����

        DisplayText(m_TransitionStageText);     //��ʾ�ı�
        StartCoroutine(ClosePanelAfterDelay(m_DisplayDuration));     //��ʾһ��ʱ����Զ��رս���
    }

    public override void ClosePanel()
    {
        FadeOut(CanvasGroup, 1f);     //����
    }



    public override void FadeOut(CanvasGroup thisCanvasGroup, float fadeDuration)
    {
        if (UIManager.Instance.PanelDict.ContainsKey(name))
        {
            UIManager.Instance.PanelDict.Remove(name);
        }


        DOTween.To(() => thisCanvasGroup.alpha, x => thisCanvasGroup.alpha = x, 0, fadeDuration).OnComplete(      //ִ�������������
            () =>
            {
                Destroy(gameObject);
            }
        );
    }
}