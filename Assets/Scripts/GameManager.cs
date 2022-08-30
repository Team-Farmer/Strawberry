using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    #region �ν�����
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    public Text MedalText;
    public Text coinAnimText;
    public Text heartAnimText;

    [Header("[ Object ]")]
    private Globalvariable globalVar;
    public GameObject stemPrefab; // ������
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
    public List<GameObject> stemObjList = new List<GameObject>();
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;
    public Text truckCoinBonusText;
    public int bonusTruckCoin;

    public const int TRUCK_CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int TRUCK_CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int TRUCK_CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int TRUCK_CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    //PTJ, NEWS================================
    [Header("[ PTJ ]")]
    //PTJ �˹�
    public GameObject workingCountText;//���� ���� ���� ��
    public GameObject PTJList;

    [Header("PTJ === Warning Panel")]
    public GameObject warningBlackPanel;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;

    //NEWS
    [NonSerialized]
    public int NewsPrefabNum;


    //���ο����================================
    [Header("[ NEW BERRY === OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;

    public GameObject TimeReuce_newBerry;
    public GameObject TimeReduceBlackPanel_newBerry;
    public GameObject TimeReducePanel_newBerry;
    public Text TimeReduceText_newBerry;
    public GameObject AcheivePanel_newBerry;
    public Sprite[] AcheiveClassify_newBerry;

    public GameObject NoPanel_newBerry;
    public GameObject BlackPanel_newBerry;

    [Header("[ NEW BERRY === SPRITE ]")]
    public Sprite StartImg;
    public Sprite DoneImg;
    public Sprite IngImg;
    public SpriteRenderer[] stemLevelSprites;


    private int price_newBerry;//�̹��� ���ߵǴ� ���� ����
    private string BtnState;//���� ��ư ����
    private int newBerryIndex2;//�̹��� ���ߵǴ� ���� ���� �ѹ�

    [Header("[ NEW BERRY === GLOBAL ]")]
    public GameObject Global;
    //===========================================

    [Header("[ Check/Settings Panel ]")]
    public GameObject settingsPanel;
    public GameObject checkPanel;


    [Header("[ Check/Day List ]")]
    public GameObject attendanceCheck;
    public string url = "";
    private int revenue;

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public Text AbsenceMoneyText;
    public Text AbsenceTimeText;
    public GameObject noCoinPanel;
    public GameObject noHeartPanel;
    public GameObject blackPanel;
    public GameObject coinAnimManager;
    public GameObject heartAnimManager;
    public GameObject AbsencePanel;
    public GameObject AbsenceBlackPanel;



    [Header("[ Game Flag ]")]
    public bool isGameRunning;
    public bool isBlackPanelOn = false;
    private int coinUpdate;
    public bool isStart;
    public bool isMiniGameMode = false;
    #endregion

    #region �⺻


    void Start()
    {
        StartCoroutine(PreWork());
        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        //PrintTime();


        Application.targetFrameRate = 60;
        instance = this; 

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //TimerStart += Instance_TimerStart;

        DisableObjColliderAll();

        isGameRunning = true;

        //NEW BERRY
        NewBerryUpdate();

        ShowCoinText(CoinText, DataController.instance.gameData.coin);
        HeartText.text = DataController.instance.gameData.heart.ToString();

        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

        isStart = true;

        InitDataInGM();
    }

    public void GameStart()
    {
        isGameRunning = true;
        Invoke("EnableObjColliderAll", 4.5f);
    }

    void InitDataInGM()
    {
        // ���� ���� �� ���� ���� �ѹ� ������Ʈ ���ֱ�
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            /*if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));*/
            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((50) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((1000) * (1 + researchCoeffi));
        }

        for (int i = 0; i < 16; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].isStemEnable)
            {
                stemList[i].gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isWeedEnable)
            {
                farmList[i].weed.gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isBugEnable)
            {
                bugList[i].gameObject.SetActive(true);
            }
            float creatTimeTemp = DataController.instance.gameData.berryFieldData[i].createTime;
            if ((0 < creatTimeTemp && creatTimeTemp < DataController.instance.gameData.stemLevel[4]) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
        //PTJ
        //���ֱ�
        workingCountText.GetComponent<Text>().text = DataController.instance.gameData.PTJCount.ToString();//�˹����� �ο���

        //NEW BERRY ����
        //���ֱ�
        switch (DataController.instance.gameData.newBerryBtnState)
        {
            case 0: BtnState = "start"; startBtn_newBerry.GetComponent<Image>().sprite = StartImg; break;
            case 1: BtnState = "ing"; startBtn_newBerry.GetComponent<Image>().sprite = IngImg; break;
            case 2: BtnState = "done"; startBtn_newBerry.GetComponent<Image>().sprite = DoneImg; break;
        }

        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư����
        {
            GameObject obj = ClickObj(); // Ŭ������ ������ �����´�
            if (obj != null)
            {

                if (obj.CompareTag("Farm"))
                {
                    ClickedFarm(obj);
                }
                else if (obj.CompareTag("Bug"))
                {
                    ClickedBug(obj);
                }
                else if (obj.CompareTag("Weed"))
                {
                    ClickedWeed(obj);
                }
            }
        }

        //������ �ڷΰ��� ��ư ������ ��/�����Ϳ���ESC��ư ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataController.instance.SaveData();
            //isStart = false;
            Application.Quit();
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        //ShowCoinText(CoinText, DataController.instance.gameData.coin); // Ʈ������ ��Ÿ�� �� ���̾����� �Ű������� �ް� �����߾�� - �����
        //HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (DataController.instance.gameData.isPrework == true)
                DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

            DataController.instance.SaveData();
        }
        else
        {
            if (isStart)//&&Intro.isEnd)
                StartCoroutine(CheckElapseTime());
        }

    }


    #endregion

    #region �����
    void ClickedFarm(GameObject obj)
    {

        Farm farm = obj.GetComponent<Farm>();

        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant)
        {
            Stem st = GetStem(farm.farmIdx);
            if (st != null)
            {
                PlantStrawBerry(st, obj); // �ɴ´�
                AudioManager.instance.SowAudioPlay();
                DataController.instance
                    .gameData.berryFieldData[farm.farmIdx].isPlant = true; // üũ ���� ����
            }
        }
        else
        {
            if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].canGrow)
            {
                Harvest(stemList[farm.farmIdx]); // ��Ȯ
            }
        }
    }
    void ClickedBug(GameObject obj)
    {
        Bug bug = obj.GetComponent<Bug>();
        bug.DieBug();
    }
    void ClickedWeed(GameObject obj)
    {
        Weed weed = obj.GetComponent<Weed>();
        weed.DeleteWeed();
    }
    public void ClickedTruck()
    {
        bonusTruckCoin = (int)(DataController.instance.gameData.truckCoin *
            DataController.instance.gameData.researchLevel[5] * Globalvariable.instance.getEffi());
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);
        ShowCoinText(truckCoinBonusText, bonusTruckCoin);
    }

    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    public void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // ���� Transform�� ���⸦ �ɴ´�
        stem.gameObject.SetActive(true); // ���� Ȱ��ȭ              
        coll.enabled = false; // ���� �ݶ��̴��� ��Ȱ��ȭ (���ʿ� �浹 ����)
    }
    public void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;

        AudioManager.instance.HarvestAudioPlay();//���� ��Ȯ�Ҷ� ȿ����
        farm.isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.getInstantBerryObj().GetComponent<Berry>().Explosion(pos, target.position, 0.5f);
        //stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 3;

        StartCoroutine(HarvestRoutine(farm, stem)); // �������� ���Ⱑ �ɾ����� ������ ����

    }
    GameObject ClickObj() // Ŭ������ ������Ʈ�� ��ȯ
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm, Stem stem)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; // ���� ��� ��Ȱ��ȭ

        yield return new WaitForSeconds(0.75f); // 0.75�� �ڿ�

        UpdateTruckState(stem);

        DataController.instance.gameData.totalHarvBerryCnt++; // ��Ȯ�� ������ �� ���� ������Ʈ            
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // ���� ����ش�

        //�ٱ⿡ ���̵� �ƿ� ����
        Animator anim = stemObjList[stem.stemIdx].GetComponent<Animator>();
        anim.SetInteger("Seed", 5);

        yield return new WaitForSeconds(0.3f); // 0.3�� �ڿ�

        stem.gameObject.SetActive(false);

        farm.isHarvest = false; // ��Ȯ�� ����              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !isBlackPanelOn) // ���ʰ� ���ٸ�
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // ���� �ٽ� Ȱ��ȭ 
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < TRUCK_CNT_LEVEL_MAX)
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.getInstantBerryObj().GetComponent<Berry>().berryPrice;
        }
    }
    #endregion

    #region ��ȭ

    IEnumerator CountAnimation(int cost, String text, int num) //��ȭ ���� �ִϸ��̼�

    {
        if (num == 0)
        {
            if (cost <= 9999)           // 0~9999���� A
            {
                coinAnimText.text = text + cost.ToString() + "A";
            }
            else if (cost <= 9999999)   // 10000~9999999(=9999B)���� B
            {
                cost /= 1000;
                coinAnimText.text = text + cost.ToString() + "B";
            }
            else                        // �� �� C (�ִ� 2100C)
            {
                cost /= 1000000;
                coinAnimText.text = text + cost.ToString() + "C";
            }
            coinAnimManager.GetComponent<HeartMover>().CountCoin(-240f);
            Invoke("invokeCoin", 1.1f);
        }
        else
        {
            heartAnimText.text = text + cost.ToString();
            heartAnimManager.GetComponent<HeartMover>().CountCoin(100f);
            Invoke("invokeHeart", 1.1f);
        }
        yield return null;
    }

    public void invokeCoin()
    {
        ShowCoinText(CoinText, DataController.instance.gameData.coin);
    }

    public void invokeHeart()
    {
        HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999���� A
        {
            coinText.text = coin.ToString() + "A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)���� B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + "B";
        }
        else                        // �� �� C (�ִ� 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + "C";
        }
    }

    public void GetCoin(int cost) // ���� ȹ�� �Լ�
    {
        StartCoroutine(CountAnimation(cost, "+", 0));
        DataController.instance.gameData.coin += cost; // ���� ���� +
        DataController.instance.gameData.accCoin += cost; // ���� ���� +
        AudioManager.instance.CoinAudioPlay();
    }

    public void UseCoin(int cost) // ���� ��� �Լ� (���̳ʽ� ���� ����)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
        {
            StartCoroutine(CountAnimation(cost, "-", 0));
            DataController.instance.gameData.coin -= cost;
        }
        else
        {
            //��� �г� ����
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            blackPanel.SetActive(true);
            noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetHeart(int cost) // ��Ʈ ȹ�� �Լ�
    {
        StartCoroutine(CountAnimation(cost, "+", 1));
        DataController.instance.gameData.heart += cost; // ���� ��Ʈ +
        DataController.instance.gameData.accHeart += cost; // ���� ��Ʈ +
    }

    public void UseHeart(int cost) // ��Ʈ ȹ�� �Լ� (���̳ʽ� ���� ����)
    {
        int myHeart = DataController.instance.gameData.heart;

        if (myHeart >= cost)
        {
            DataController.instance.gameData.heart -= cost;
            StartCoroutine(CountAnimation(cost, "-", 1));
        }
        else
        {
            //��� �г� ����
            panelHearText.text = DataController.instance.gameData.heart.ToString() + "��";
            blackPanel.SetActive(true);
            noHeartPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }

    public void GetMedal(int cost)
    {
        DataController.instance.gameData.medal += cost;
        ShowMedalText();
    }

    public void UseMedal(int cost)
    {
        int myMedal = DataController.instance.gameData.medal;
        if (myMedal >= cost)
        {
            DataController.instance.gameData.medal -= cost;
            ShowMedalText();
        }
        else
        {
            //�޴��� ���ڸ��� �ߴ� ���
        }
    }
    public void ShowMedalText()
    {
        MedalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();
    }
    #endregion

    #region �ݶ��̴�
    public void DisableObjColliderAll() // ��� ������Ʈ�� collider ��Ȱ��ȭ
    {
        BoxCollider2D coll;
        isBlackPanelOn = true;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            //stemList[i].canGrow = false;
            bugList[i].GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            // Weed�� Collider ����
            //farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // ��� ������Ʈ�� collider Ȱ��ȭ
    {
        BoxCollider2D coll;
        isBlackPanelOn = false;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // ���ʰ� ���� ���� �� ���� ColliderȰ��ȭ
            {
                coll.enabled = true;
            }
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4]) // (4)�� ��Ȳ, �� ������ ���� �� �� ���� �� �� �ڶ� ������� �ݶ��̴��� ���ش�.
            {
                coll.enabled = true;
            }
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // ������ Collider Ȱ��ȭ
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region ����Ʈ

    #region PTJ

    //���� ��ư Ŭ����
    public void PTJEmployButtonClick(int prefabNum)
    {
        //ȿ����
        AudioManager.instance.Cute1AudioPlay();

        //�������� �ƴ� �����̴�
        if (DataController.instance.gameData.PTJNum[prefabNum] == 0)
        {
            if (DataController.instance.gameData.PTJCount < 3)
            {
                int cost = PTJ.instance.Info[prefabNum].Price * DataController.instance.gameData.PTJSelectNum[1];
                if (cost <= DataController.instance.gameData.coin)
                {
                    int ID = DataController.instance.gameData.PTJSelectNum[0];
                    //HIRE

                    //���λ��
                    UseCoin(cost);

                    //����
                    DataController.instance.gameData.PTJNum[prefabNum] = DataController.instance.gameData.PTJSelectNum[1];

                    //�������� �˹ٻ� �� ����
                    DataController.instance.gameData.PTJCount++;
                }
                else
                {
                    //ȿ����
                    AudioManager.instance.Cute4AudioPlay();
                    //��ȭ ���� ��� �г�
                    ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                    warningBlackPanel.SetActive(true);
                }
            }
            else
            {
                //ȿ����
                AudioManager.instance.Cute4AudioPlay();
                //3���̻� �������̶�� ��� �г� ����
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "���� ������ �˹� ����\n�Ѿ���!";
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                warningBlackPanel.SetActive(true);
            }
        }
        //�������� �����̴�
        else
        {
            //FIRE
            //Ȯ��â����
            HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            warningBlackPanel.SetActive(true);
        }


    }

    public void Fire()
    {
        int ID = DataController.instance.gameData.PTJSelectNum[0];
        //���� ����
        DataController.instance.gameData.PTJNum[ID] = 0;
        //���� ���� �˹ٻ� �� ����
        //PTJ���� ������

        //Ȯ��â������
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();
        warningBlackPanel.SetActive(false);
    }
    #endregion

    #region New Berry Add
    public void NewBerryUpdate()
    {
        //�� ���� ����======

        //PRICE
        price_newBerry = 90 + 10 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
        //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
        ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

        if (DataController.instance.gameData.newBerryBtnState == 1)//�������̸�
        { StartCoroutine("Timer"); }
        else
        {
            if (isNewBerryAble() == true)//���� ������ ���� �ִ��� �˻�
            {

                DataController.instance.gameData.newBerryIndex = selectBerry();//��������, �ð� ��������
                timeText_newBerry.GetComponent<Text>().text = "??:??";//TIME (�̰���)

                //���� ���� �����
                NoPanel_newBerry.SetActive(false);
            }
            else { NoPanel_newBerry.SetActive(true); }
        }
    }
    public void NewBerryUpdate2() //���� ������ ���� ���� �о����� ����Ǵ� ��. ���� ��ġ��
    {

        if (isNewBerryAble() == true)//�� �� �� Ȯ��
        {
            if (DataController.instance.gameData.newBerryBtnState == 0)//���� ��� �ִ� ���� �ƴϸ�
            {
                //�������Ⱑ ��������.->�ð�,���� ��������.
                //PRICE
                price_newBerry = 100 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);
                //TIME
                DataController.instance.gameData.newBerryIndex = selectBerry();
                timeText_newBerry.GetComponent<Text>().text = "??:??";//���� �ð� ���� �̰��� "?"

            }
            //���� ���� �����
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true); }

    }

    public bool isNewBerryAble()
    {
        //���� �����⸦ ���� �� �� �ֳ�
        switch (DataController.instance.gameData.newBerryResearchAble)
        {
            case 0://classic ���߰���
                if (BerryCount("classic", false) == BerryCount("classic", true))
                { return false; }
                break;
            case 1://classic, special ���߰���
                if (BerryCount("classic", false) + BerryCount("special", false) ==
                    BerryCount("classic", true) + BerryCount("special", true))
                { return false; }
                break;
            case 2: //classic, special, unique ���߰���
                if (BerryCount("classic", false) + BerryCount("special", false) + BerryCount("unique", false) ==
                    BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true))
                { return false; }
                break;
        }
        return true;
    }


    //isUnlock-> false=���� ���� �����ϴ� ���� �������� ��ȯ / true=���� unlock�� ���� �������� ��ȯ�Ѵ�.
    private int BerryCount(string berryClssify, bool isUnlock)
    {
        int countIsExsist = 0;
        int countIsUnlock = 0;
        switch (berryClssify)
        {
            case "classic":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().classicBerryList.Count; i++)
                {
                    if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; }
                    if (Global.GetComponent<Globalvariable>().classicBerryList[i] == true) { countIsExsist++; }
                }
                break;

            case "special":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().specialBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64 + 64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().uniqueBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().uniqueBerryList[i] == true) { countIsExsist++; } }
                for (int i = 128; i < 128 + 64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;
                //default:Debug.Log("�߸��� �� �޾Ҵ�");break;
        }


        if (isUnlock == true)
        { return countIsUnlock; }
        else { return countIsExsist; }
    }



    //���ο� ���� ���� ��ư ������
    public void NewBerryButton()
    {

        switch (BtnState)
        {
            case "start":
                //�̹� ������ ���߿� �ʿ��� ���ݰ� �ð�
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();//����
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

                if (DataController.instance.gameData.coin >= price_newBerry)
                {
                    timeText_newBerry.GetComponent<Text>().text
                        = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));//�ð�
                    //���Һ�
                    UseCoin(price_newBerry);

                    //��ư���� ing��
                    DataController.instance.gameData.newBerryBtnState = 1;

                    //Ÿ�̸� ����
                    StartCoroutine("Timer");

                    //�ð� ���ҿ��� ���� �г��� ����.
                    TimeReduceBlackPanel_newBerry.SetActive(true); //�ÿ� �ǵ帲
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale(); //�ÿ� �ǵ帲
                    TimeReduceText_newBerry.GetComponent<Text>().text //�ÿ� �ǵ帲
                        = "하트 3개를 소모하여 3분 단축하시겠습니까??\n";
                }
                else//���� ���ڸ�
                { UseCoin(price_newBerry); }
                break;

            case "ing":
                if (DataController.instance.gameData.newBerryTime > 1)
                {
                    //�ð� ���ҿ��� ���� �г��� ����.
                    TimeReduceBlackPanel_newBerry.SetActive(true);
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale();
                    TimeReduceText_newBerry.GetComponent<Text>().text
                        = "하트 3개를 소모하여 3분 단축하시겠습니까?\n";
                }
                break;

            case "done": //���� ����
                GetNewBerry();
                break;

        }

    }

    //TimeReucePanel_newBerry
    //��Ʈ �Ἥ �ð��� ������ ���� �г�
    public void TimeReduce(bool isTimeReduce)
    {
        //��Ʈ �Ἥ �ð��� ���ϰŸ�
        if (isTimeReduce == true)
        {
            if (DataController.instance.gameData.heart >= 3)
            {
                //�ð��� 10�� �ٿ��ش�.
                if (DataController.instance.gameData.newBerryTime < 3 * 60)
                { DataController.instance.gameData.newBerryTime = 0; }
                else
                { DataController.instance.gameData.newBerryTime -= 3 * 60; }

                timeText_newBerry.GetComponent<Text>().text
                    = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));
                //��Ʈ�� �Һ��Ѵ�.
                UseHeart(3);
            }
            else
            { UseHeart(3); }
        }

        //â ����
        TimeReduceBlackPanel_newBerry.SetActive(false);
        TimeReducePanel_newBerry.GetComponent<PanelAnimation>().CloseScale();

    }

    IEnumerator Timer()
    {

        while (true)
        {
            //1�ʾ� ����
            yield return new WaitForSeconds(1f);
            DataController.instance.gameData.newBerryTime--;

            //�����ϴ� �ð� ���̱�
            if (DataController.instance.gameData.newBerryTime <= 0)
            { timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(0)); }
            else
            {
                timeText_newBerry.GetComponent<Text>().text
             = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime));
            }


            //Ÿ�̸� ������
            if (DataController.instance.gameData.newBerryTime < 0.1f)
            {
                DataController.instance.gameData.newBerryBtnState = 2;//Done���·�
                break;
            }
        }
        StopCoroutine("Timer");

    }


    private int selectBerry()
    {
        int newBerryIndex = 1;

        while (true)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0:
                    newBerryIndex = berryPercantage(64);
                    /*newBerryIndex = UnityEngine.Random.Range(1, 64);
                    DataController.instance.gameData.newBerryTime = 10 * 60;*/
                    break;
                case 1:
                    newBerryIndex = berryPercantage(128);
                    break;
                case 2:
                    newBerryIndex = berryPercantage(192);
                    break;
            }

            if (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == false
            && Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex] != null)
            { break; }
        }
        return newBerryIndex;
    }


    private void GetNewBerry()
    {

        //���ο� ���Ⱑ �߰��ȴ�.
        DataController.instance.gameData.isBerryUnlock[DataController.instance.gameData.newBerryIndex] = true;
        DataController.instance.gameData.unlockBerryCnt++;

        //����ǥ ǥ��
        DataController.instance.gameData.isBerryEM[DataController.instance.gameData.newBerryIndex] = true;

        //���� ���� ȿ����(¥��)
        AudioManager.instance.TadaAudioPlay();

        //���� ���� ����â
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().sprite //���� ���� �̹���
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;

        AcheivePanel_newBerry.transform.GetChild(2).GetComponent<Text>().text //���� ���� �̸�
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<Berry>().berryName;
        //���� �з� �̹��� 
        if (DataController.instance.gameData.newBerryIndex < 64)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[0]; }
        else if (DataController.instance.gameData.newBerryIndex < 128)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[1]; }
        else
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[2]; }


        //����â ����
        BlackPanel_newBerry.SetActive(true);


        DataController.instance.gameData.newBerryBtnState = 0;

        NewBerryUpdate();

    }

    private int berryPercantage(int endIndex)
    {
        int randomNum = 0;
        int newBerryIndex = 0;

        //RANDOM NUM -> classic(45)=0~44  special(35)=45~79  unique(20)=80~101
        if (endIndex == 128) { randomNum = UnityEngine.Random.Range(0, 80); }//���� Ŭ�����̶� ����ȸ� �����ϸ�
        else if (endIndex == 192) { randomNum = UnityEngine.Random.Range(0, 100 + 1); }//���� ���δ� �����ϸ�



        if (randomNum < 45)
        {
            newBerryIndex = UnityEngine.Random.Range(1, 64);
            DataController.instance.gameData.newBerryTime = 3 * 60;
        }//classic
        else if (randomNum < 80)
        {
            newBerryIndex = UnityEngine.Random.Range(64, 128);
            DataController.instance.gameData.newBerryTime = 6 * 60;
        }//special
        else if (randomNum <= 100)
        {
            newBerryIndex = UnityEngine.Random.Range(128, 192);
            DataController.instance.gameData.newBerryTime = 9 * 60;
        }//unique


        return newBerryIndex;
    }


    public bool newsBerry()
    {
        if (isNewBerryAble())
        {
            do { newBerryIndex2 = selectBerry(); }
            while (newBerryIndex2 == DataController.instance.gameData.newBerryIndex);

            //������ �����̶� ���� �ر� ���ÿ� �ߴµ� ���� ���� �������� �ϸ� ����������� �ִٰ� ����

            //���ο� ���Ⱑ �߰��ȴ�.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex2] = true;
            DataController.instance.gameData.unlockBerryCnt++;
            //����ǥ ǥ��
            DataController.instance.gameData.isBerryEM[newBerryIndex2] = true;

            //새딸기 얻음 팝업창
            GetNewBerry();

            return true;

        }
        else { return false; }

    }

    #endregion

    #region Explanation
    /*
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //����â ����
                berryExp_BlackPanel.SetActive(true); //�ÿ� �ǵ帲
                berryExp_Panel.GetComponent<PanelAnimation>().OpenScale(); //�ÿ� �ǵ帲

                //GameObject berryExpImage = berryExp.transform.GetChild(1).GetChild(1).gameObject; //�ÿ� �ǵ帲
                //GameObject berryExpName = berryExp.transform.GetChild(1).GetChild(2).gameObject; //�ÿ� �ǵ帲
                //GameObject berryExpTxt = berryExp.transform.GetChild(1).GetChild(3).gameObject; //�ÿ� �ǵ帲


                //Explanation ������ ä���.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//�̹��� ����

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//�̸� ����

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryExplain;//���� ����    
            }
        }
        catch
        {
            Debug.Log("���⿡ �ش��ϴ� ������ ���� ����");
        }
    }
    */
    #endregion

    public void NewsUnlock()
    {
        News.instance.NewsUnlock(NewsPrefabNum);
    }

    #region ��Ÿ
    //Ȱ��ȭ ��Ȱ��ȭ�� â ���� �Ѱ�
    public void turnOff(GameObject Obj)
    {

        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }

    }

    public string TimeForm(int time)//�ʴ��� �ð��� ��:�ʷ� ����
    {
        int M = 0, S = 0;//M,S ����
        string Minutes, Seconds;//M,S �ؽ�Ʈ �����

        M = (time / 60);
        S = (time % 60);


        //M,S����
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S�� 10�̸��̸� 01, 02... ������ ǥ��
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S�� 0�̸� 00���� ǥ���Ѵ�.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #endregion

    #region �⼮

    //���ͳ� �ð� ��������.

    public static IEnumerator UpdateCurrentTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("https://naver.com"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    string date = request.GetResponseHeader("date");
                    DateTime dateTime = DateTime.Parse(date);
                    DataController.instance.gameData.currentTime = dateTime;
                }
            }
        }
    }

    public static IEnumerator TryGetCurrentTime()
    {
        while (DataController.instance.gameData.isPrework == false)
        {
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("https://naver.com"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    DataController.instance.gameData.isPrework = false;
                }
                else
                {
                    string date = request.GetResponseHeader("date");
                    DateTime dateTime = DateTime.Parse(date);
                    DataController.instance.gameData.currentTime = dateTime;
                    DataController.instance.gameData.isPrework = true;
                }
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    //���� üũ �� ��������
    void ResetTime()
    {
        DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

        attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        //DataController.instance.gameData.currentTime.Date.AddDays(1); //������ ���� ���� ����.

        //���� Ÿ�̸�
        Invoke(nameof(ResetTime),
            (float)(DataController.instance.gameData.nextMidnightTime
            - DataController.instance.gameData.currentTime).TotalSeconds);
    }

    public void ResetInvoke()
    {
        DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
        Invoke(nameof(ResetTime),
        (float)(DataController.instance.gameData.nextMidnightTime
            - DataController.instance.gameData.currentTime).TotalSeconds);
    }


    IEnumerator CheckElapseTime() //���� �����Ҷ�
    {
        DataController.instance.gameData.isPrework = false;
        yield return StartCoroutine(TryGetCurrentTime());

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        if (gap.TotalSeconds > 3610f)
        {
            yield return StartCoroutine(CalculateTime());
        }

        MidNightCheck();

        //PrintTime();

        if (!MiniGameManager.isOpen && DataController.instance.gameData.rewardAbsenceTime.TotalMinutes >= 60)//&&Intro.isEnd)
        {
            //������ �̺�Ʈ
            AbsenceTime();
        }

    }

    IEnumerator PreWork() //������ ��
    {
        yield return StartCoroutine(TryGetCurrentTime()); //���� �ð� �ҷ����� üũ
        yield return StartCoroutine(CalculateTime());

        StartCoroutine(UpdateCurrentTime()); //30�� ���� �����⸴

        MidNightCheck();

        if (DataController.instance.gameData.rewardAbsenceTime.TotalMinutes >= 60)//&&Intro.isEnd)
        {
            //������ �̺�Ʈ
            AbsenceTime();
        }
    }


    public void MidNightCheck()
    {
        DateTime temp = new DateTime();
        if (temp != DataController.instance.gameData.nextMidnightTime && temp != DataController.instance.gameData.currentTime)
        {
            //����ó��
            TimeSpan test = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime.Date;
            if (test.Days >= 2)
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

            //�����ð��� �����ٸ�
            TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.nextMidnightTime;
            if (gap.TotalSeconds >= 0)
            {
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
                attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
            }
            //�����ð��� ������ �ʾҴٸ�
            gap = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime;
            if (gap.TotalSeconds >= 0)
                Invoke(nameof(ResetTime), (float)gap.TotalSeconds);
        }
    }

    public static bool CheckFirstGame()
    {
        if (!DataController.instance.gameData.isFirstGame)
        {
            DataController.instance.gameData.isFirstGame = true;

            DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
            DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            DataController.instance.gameData.atdLastday = DataController.instance.gameData.currentTime.Date.AddDays(-1);
            DataController.instance.gameData.accDays = 0;
            GameManager.instance.attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
            return true;
        }
        return false;
    }

    public static IEnumerator CalculateTime() //������ �ð� ���
    {
        if (CheckFirstGame() == true) yield break;

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

        if ((DataController.instance.gameData.rewardAbsenceTime + gap).TotalMinutes >= 1440) //������ ���� �ִ�ġ ���� 24�ð�
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromMinutes(1440);
        else
            DataController.instance.gameData.rewardAbsenceTime += gap;

        //���� �˹� �ð� ���� �ڸ�
    }

    void PrintTime()
    {
        Debug.Log("현재 시간: " + DataController.instance.gameData.currentTime);
        Debug.Log("다음 자정시간: " + DataController.instance.gameData.nextMidnightTime);
        Debug.Log("다음 자정시간까지 남은시간: " + (DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime));
        Debug.Log("마지막 종료 시간: " + DataController.instance.gameData.lastExitTime);
        Debug.Log("부재중 시간: " + (DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime));
        Debug.Log("누적출석:" + DataController.instance.gameData.accDays);
        Debug.Log("마지막 출석:" + DataController.instance.gameData.atdLastday);
    }

    public void AbsenceTime()
    {
        int researchLevelAdd = 0;
        int minute = DataController.instance.gameData.rewardAbsenceTime.Minutes;
        int hour=0;
        if (minute > 59)
        {
            hour = minute / 60;
            minute &= minute;
        }

        AbsenceTimeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);

        for (int i = 0; i < 6; i++)
        {
            researchLevelAdd += DataController.instance.gameData.researchLevel[i];
        }
        revenue = (DataController.instance.gameData.rewardAbsenceTime.Minutes / 5) * researchLevelAdd / 6 * 2;

        //if (Intro.isEnd&&
        if (MiniGameManager.isOpen)
        {
            if (revenue == 0)
                return;

            if (revenue <= 9999)           // 0~9999���� A
            {
                AbsenceMoneyText.text =  revenue + "A";
            }
            else if (revenue <= 9999999)   // 10000~9999999(=9999B)���� B
            {
                revenue /= 1000;
                AbsenceMoneyText.text = revenue + "B";
            }
            else                        // �� �� C (�ִ� 2100C)
            {
                revenue /= 1000000;
                AbsenceMoneyText.text = revenue + "C";
            }
            AbsenceBlackPanel.SetActive(true);
            AbsencePanel.GetComponent<PanelAnimation>().OpenScale();
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
        }
    }

    public void AbsenceBtn()
    {
        GetCoin(revenue);
        AbsencePanel.GetComponent<PanelAnimation>().CloseScale();

    }

    /*    public void CheckTime()
        {
            //�÷��� ���� ������ �Ѿ ��� �⼮ �����ϰ�
            // �����ð� ���ϱ�.
            DateTime target = new DateTime(DataController.instance.gameData.currentTime.Year, 
                DataController.instance.gameData.currentTime.Month, DataController.instance.gameData.currentTime.Day);
            target = target.AddDays(1);
            // �����ð� - ����ð�
            TimeSpan ts = target - DataController.instance.gameData.currentTime;
            // �����ð� ��ŭ ��� �� OnTimePass �Լ� ȣ��.
            Invoke("OnTimePass", (float)ts.TotalSeconds);
            Debug.Log("�������� ���� �ð�(��): " + ts.TotalMinutes);
        }*/

    /*    public void OnTimePass()
        {
            //��������
            Debug.Log("�⼮ ������ ���ŵǾ����ϴ�.");

            StartCoroutine(UpdateCurrentTime());
            CheckTime();
            if (DataController.instance.gameData.currentTime.Day != DataController.instance.gameData.atdLastday.Day)
                DataController.instance.gameData.isAttendance = false;

            attendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        }*/


    #endregion

    #region ���� �޴�
    public void OnclickStart()
    {
    }

    public void OnclickOption()
    {

    }

    public void OnclickQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

}