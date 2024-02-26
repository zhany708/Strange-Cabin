using System.Collections.Generic;
using UnityEngine;



public class UIConst    //���ڴ洢���������
{
    public const string PlayerStatusBar = "PlayerStatusBar";    //���״̬������HealthBar�ű����ʼ��
    public const string TransitionStagePanel = "TransitionStagePanel";    //������׶�����
}




public class UIManager
{
    private static UIManager m_Instance;
    public static UIManager Instance    //����ģʽ��������Ϸֻ����һ�������ʵ����
    {
        get
        {
            if (m_Instance == null)     //��һ�μ��
            {
                m_Instance = new UIManager();
            }
            return m_Instance;
        }
    }


    private Transform m_UIRoot;
    public Transform UIRoot     //����UI�ĸ��ڵ㣨���ĸ����壩
    {
        get
        {
            if (m_UIRoot == null)
            {
                GameObject canvasObject = GameObject.Find("Canvas");

                if (canvasObject != null)
                {
                    m_UIRoot = canvasObject.transform;
                }
                else
                {
                    m_UIRoot = new GameObject("Canvas").transform;
                }               
            }
            return m_UIRoot;
        }
    }




    public Dictionary<string, BasePanel> PanelDict;      //����Ѵ򿪽�����ֵ䣨����洢�Ķ������ڴ򿪵Ľ��棩

    Dictionary<string, string> m_PathDict;       //ʹ���ֵ�洢���������·����
    Dictionary<string, GameObject> m_PrefabDict;     //Ԥ�Ƽ������ֵ�









    private UIManager()     //���캯��
    {
        InitDicts();
    }



    private void InitDicts()    //��ʼ���ֵ�
    {
        PanelDict = new Dictionary<string, BasePanel>();
        m_PrefabDict = new Dictionary<string, GameObject>();

        m_PathDict = new Dictionary<string, string>()   //��ʼ�����н����·��
        {
            { UIConst.TransitionStagePanel, "Others/TransitionStagePanel"},
            { UIConst.PlayerStatusBar, "Others/PlayerStatusBar"}
        };
    }




    //�򿪽���
    public BasePanel OpenPanel(string name)
    {
        //�������Ƿ��Ѿ���
        BasePanel panel = null;

        if (PanelDict.TryGetValue(name, out panel))     //�˺����᳢�Դ��ֵ���Ѱ�ҵ�һ��������Ԫ�ز���ֵ���ڶ�����������󷵻�true������ֵ����ҵ��˵�һ��������Ӧ��Ԫ��
        {
            Debug.LogError("This panel is already opened: " +  name);
            return null;
        }


        //���·���Ƿ�������
        string path = "";

        if (!m_PathDict.TryGetValue(name, out path))
        {
            Debug.LogError("Something wrong with the name or path of this panel: " + name);
            return null;
        }


        //ʹ�û����Ԥ�Ƽ�
        GameObject panelPrefab = null;

        if (!m_PrefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefab/UI/Panels/" + path;    //���û�б����ع�������س��������뻺���ֵ�

            panelPrefab = Resources.Load<GameObject>(realPath);     //ͨ��Load������Assets��Ѱ����Դ��ֵ��������Resources�ļ������棩
            m_PrefabDict.Add(name, panelPrefab);    //�����Ž���Ԥ�Ƽ����ֵ�
        }


        //�򿪽���
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);    //���ɳ�������ʹ����ΪUIRoot��һ���ӽڵ�

        panel = panelObject.GetComponent<BasePanel>();
        PanelDict.Add(name, panel);       //�������Ѵ򿪽�����ֵ�
        return panel;
    }

    //�رս���
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;

        if (!PanelDict.TryGetValue (name, out panel))     //�������Ƿ��Ѵ򿪣�û�򿪵Ļ��򱨴�
        {
            Debug.LogError("This panel is not opened yet: " + name);
            return false;
        }


        if (panel.CanvasGroup != null)
        {
            panel.FadeOut(panel.CanvasGroup, 1f);       //������Ե����Ļ����ȵ���
        }
        else
        {
            panel.ClosePanel();
        }

        return true;
    }



    public void ChangePanelLayer(BasePanel thisPanel)   //�ı�UI����Ⱦ�㼶
    {

    }
}