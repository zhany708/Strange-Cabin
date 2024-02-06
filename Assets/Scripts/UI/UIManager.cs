using System.Collections;
using TMPro;
using UnityEngine;



public class UIManager : MonoBehaviour
{
    public GameObject TransitionStageTextBG;




    TextMeshProUGUI TransitionStageText;
    float m_DisplayDuration = 10f;
    







    private void Awake()
    {
        TransitionStageText = TransitionStageTextBG.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        TransitionStageTextBG.SetActive(false);
    }







    public void DisplayText(GameObject thisTextBG)
    {
        thisTextBG.SetActive(true);    //��ʾ����


        if (TransitionStageText != null)
        {
            TypeWriterEffect(TransitionStageText, 0.05f);
        }
        
        StartCoroutine(DisableTextAfterDelay(thisTextBG, m_DisplayDuration));  
    }


    private void TypeWriterEffect(TextMeshProUGUI textComponent, float typeSpeed)      //���ֻ�Ч��
    {
        string fullText = textComponent.text;   //�ȴ������ֶΣ�Ȼ����������
        textComponent.text = " ";

        StartCoroutine(TypeText(textComponent, fullText, typeSpeed));
    }



    private IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float typeSpeed)      //���ڴ��ֻ���ÿ����һ��һ���������
    {
        foreach (char letter in fullText.ToCharArray() )
        {
            textComponent.text += letter;       //ÿ��һ���ִ�����󣬵ȴ�һ��ʱ���ٴ���һ����
            yield return new WaitForSeconds(typeSpeed);
        }
    }


    private IEnumerator DisableTextAfterDelay(GameObject thisTextBG, float delay)      //����һ��ʱ�����������
    {
        yield return new WaitForSeconds(delay);
        thisTextBG.SetActive(false);
    }
}
