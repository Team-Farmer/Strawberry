using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{

    [Serializable]
    public class ResearchStruct
    {
        public string Name;//���� ����
        public Sprite Picture;//����
        public string Explanation;//����
        public int Price;//����
        public int[] Prices = new int[25];//����

        public ResearchStruct(string Name, string Explanation, int Price, int[] Prices, Sprite Picture)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Prices = Prices;   // ���� ������ �迭�� �ֿ�..?
            this.Picture = Picture;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ResearchStruct[] Info;//����ü

    //Research Info  ������ �͵�
    [Header("==========INFO ������ ���=========")]
    [SerializeField]
    public GameObject titleText;
    public GameObject Picture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //test
    private GameObject warningBlackPanel2;
    private GameObject noCoinPanel2;
    private GameObject panelCoinText2;

    //Prefab���� ���� �ο�
    static int Prefabcount = 0;
    int prefabnum;


    //�� ��ƼŬ
    private ParticleSystem rainParticle;


    // �۷ι� ����
    private Globalvariable globalVar;
    //===================================================================================================
    //===================================================================================================

    void Start()
    {
        InfoUpdate();

        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

    }

    
    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� 6�� ������ ���ڿ� ���õǾ� �ִ�!!! ���� ���� . ���������ϱ�

        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6)
        { Prefabcount -= 6; }
        prefabnum = Prefabcount;

        Info[Prefabcount].Prices = new int[25];

        for (int i=0; i<25; i++)
        {
            Info[prefabnum].Prices[i] = 100 * (i+1);
        }

        //Ÿ��Ʋ, ����, ���ΰ�, ����, ���뿩�� �ؽ�Ʈ�� ǥ��
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//����(�̸�) ǥ��

        Picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;//�׸� ǥ��

        
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation+"\n"+
            ((DataController.instance.gameData.researchLevel[prefabnum]*2) + "% ��" + 
            (DataController.instance.gameData.researchLevel[prefabnum]+1)*2 + "%");//���� �ؽ�Ʈ ǥ��

        //coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";
       
        GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[Prefabcount].Prices[DataController.instance.gameData.researchLevel[prefabnum]]); //��� ǥ��

        levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();



        Prefabcount++;
    }

    //=============================================================================================================================

    //���� ����
    public void clickCoin_Research() {
        AudioManager.instance.Cute1AudioPlay();
        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//���� 25�� �Ѱ�α�
        {
            //�ش� �ݾ��� ���� ���� ���κ��� ������
            if (DataController.instance.gameData.coin >= Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]])
            {
                //�ش� �ݾ��� ������ �����ϰ� ������
                GameManager.instance.UseCoin(Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();

                switch (Info[prefabnum].Name)
                {
                    case "���� ���� �ݰ�": IncreaseBerryPrice(); break;
                    case "���Ⱑ ����": DecreaseBerryGrowTime(); break;
                    case "�θ��� �� ��": break;
                    case "������� ������": DecreaseBugGenerateProb(); break;
                    case "���� ���̹���": DecreaseWeedGenerateProb(); break;
                    case "�ÿ��� �ҳ���": IncreaseRainDuration(); break;
                }

                if (DataController.instance.gameData.researchLevel[prefabnum] == 25)
                {
                    coinNum.GetComponent<Text>().text = "Max";
                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                        ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%");//���� �ؽ�Ʈ ǥ��
                }
                else
                {
                    GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);

                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                    ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%" + "��" +
                    (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");//���� �ؽ�Ʈ ǥ��
                }
            }
            else
            {
                //��ȭ ���� ��� �г� ����
                AudioManager.instance.Cute4AudioPlay();
                //panelCoinText2.GetComponent<Text>().text= DataController.instance.gameData.coin.ToString();
                //���� ������
                //GameManager.instance.ShowCoinText(panelCoinText2.GetComponent<Text>(), DataController.instance.gameData.coin); 
                //warningBlackPanel2.SetActive(true);
                //noCoinPanel2.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        else
        {
            // ���� �̹� �ƽ���~ �г� �ʿ� (�ƴԸ���)
            Debug.Log("���� �� �ƽ���");
        }
            

    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();     
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.getEffi();

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.getEffi();     
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.getEffi();
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[5]) * Globalvariable.instance.getEffi();
        var main = rainParticle.main;
        main.duration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
        DataController.instance.gameData.rainDuration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
    }
    


}