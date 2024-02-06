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
        thisTextBG.SetActive(true);    //显示文字


        if (TransitionStageText != null)
        {
            TypeWriterEffect(TransitionStageText, 0.05f);
        }
        
        StartCoroutine(DisableTextAfterDelay(thisTextBG, m_DisplayDuration));  
    }


    private void TypeWriterEffect(TextMeshProUGUI textComponent, float typeSpeed)      //打字机效果
    {
        string fullText = textComponent.text;   //先储存文字段，然后清零文字
        textComponent.text = " ";

        StartCoroutine(TypeText(textComponent, fullText, typeSpeed));
    }



    private IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float typeSpeed)      //用于打字机（每个字一个一个打出来）
    {
        foreach (char letter in fullText.ToCharArray() )
        {
            textComponent.text += letter;       //每当一个字打出来后，等待一段时间再打下一个字
            yield return new WaitForSeconds(typeSpeed);
        }
    }


    private IEnumerator DisableTextAfterDelay(GameObject thisTextBG, float delay)      //用于一段时间后隐藏文字
    {
        yield return new WaitForSeconds(delay);
        thisTextBG.SetActive(false);
    }
}
