using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private int Hp;
    private int MaxHp;
    private int Damage;
    private int BossLv = 0;
    public GameObject BossContainer;
    public GameObject BossPoker;
    public GameObject BossHp;
    public GameObject BossDamage;
    public GameObject CurrentHpBar;
    private float HpBarLength;
    public List<GameObject> BossList;
    public GameObject CardDeck;
    public GameObject BossCard;
    public GameObject BossStateBackground;
    public GameObject ForbidIcon;
    public bool isSuitForbid = false;

    private List<GameObject> tempList;
    private float timer = 0f;

    private void Awake()
    {
        tempList = new List<GameObject>();
        BossList = new List<GameObject>();
    }
    void Start()
    {
        InitialBossList();  //此时列表已有卡牌，需要按规则打乱
        SetBossCard();
    }
    private void Update()   
    {
        timer += Time.deltaTime;
        if(timer>1f)
        {
            UpdateBossInfo();
            timer = 0f;
        }
    }
    public void ReadyState()
    {
        SEManager.Instance.BossAttackReady();
        BossCard.GetComponent<Animator>().SetInteger("BossState", 1);
        BossStateBackground.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void AttackState() 
    { 
        SEManager.Instance.BossAttack();
        BossCard.GetComponent<Animator>().SetInteger("BossState", 2);
        Invoke("ResetColor", 0.5f);
    }
    public void ResetColor() { BossStateBackground.GetComponent<SpriteRenderer>().color = Color.black; }
    public void SetBossCard()
    {
        if (BossList.Count == 0)
        {
            //MainThread.Instance.Victory();
            return;
        }
        BossPoker = BossList[0];
        BossPoker.transform.SetParent(BossContainer.transform);
        BossPoker.transform.position = BossContainer.transform.position;
        if(BossPoker.GetComponent<Poker>().cardNumber == 11)
        {
            MaxHp = 20 + (int)(1.5*BossLv);  SetHp(20 + (int)(1.5 * BossLv));  SetDamage(10 + (int)(0.5*BossLv)); 
        }
        else if(BossPoker.GetComponent<Poker>().cardNumber == 12)
        {
            MaxHp = 30 + (int)(1.5 * BossLv);  SetHp(30 + (int)(1.5 * BossLv)); SetDamage(15 + (int)(0.5 * BossLv));
        }
        else if (BossPoker.GetComponent<Poker>().cardNumber == 13)
        {
            MaxHp = 40 + (int)(1.5 * BossLv);  SetHp(40 + (int)(1.5 * BossLv)); SetDamage(20 + (int)(0.5 * BossLv)); 
        }
        else
        {
            Debug.LogWarning("Boss卡牌出错");
        }
        
    }
    public void UpdateBossInfo()
    {
        BossHp.GetComponent<TextMesh>().text = "生命值："+Hp;

        GameObject heart = GameObject.Find("HeartNecklace(Clone)");
        if (heart != null && heart.GetComponent<HeartNecklace>().CheckIfTrue())
               BossDamage.GetComponent<TextMesh>().text = "攻击力："+Damage+"-"+(int)(Damage* heart.GetComponent<HeartNecklace>().ReduceRate);
        else BossDamage.GetComponent<TextMesh>().text = "攻击力："+Damage;

        HpBarLength = 2*Hp / (float)MaxHp;
        CurrentHpBar.transform.localScale = new Vector3(HpBarLength, 1, 1);
    }
    public int GetHp()
    {
        return Hp;
    }
    public void SetHp(int hp)
    {
        Hp = hp;
        UpdateBossInfo();
    }
    public int GetDamage()
    {
        return Damage;
    }
    public void SetDamage(int damage)
    {
        if (damage < 0) Damage = 0;
        else Damage = damage;
        UpdateBossInfo();
    }
    public void StartSwitchBoss()
    {
        StartCoroutine(DisposeFormerBoss());    //该函数末尾会更换Boss卡
    }
    IEnumerator DisposeFormerBoss()   //动画版将当前boss牌置入牌堆 //该函数末尾会更换Boss卡
    {
        yield return new WaitForSeconds(1.2f);
        GameObject poker = BossPoker;
        ObtainGem(poker);
        BossList.Remove(poker);
        BossStateBackground.gameObject.SetActive(false);
        AllowSuit();    //重置禁止花色效果

        Vector3 start = poker.transform.position;
        Vector3 dest = CardDeck.transform.position;

        // 移动卡牌到目标位置
        float duration = 0.2f; // 假设动画持续时间为 0.5 秒
        float elapsed = 0f;

        while (elapsed < duration)
        {
            poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 确保卡牌位置设置为目标位置
        poker.transform.position = dest;
        poker.transform.SetParent(CardDeck.transform);

        if (Hp > 0) Debug.LogError("Boss未被消灭就调用了销毁方法");
        else if (Hp == 0)   //荣誉消灭  ->  加入牌堆顶
        {
            CardDeck.GetComponent<CardDeck>().pokerList.Insert(0, poker);   //加入牌堆顶
        }
        else CardDeck.GetComponent<CardDeck>().AddToList(poker);    //加入牌堆底
        CardDeck.GetComponent<CardDeck>().SortCard();

        yield return new WaitForSeconds(0.2f);

        SetBossLv(+1);
        SetBossCard();
        MyEventSystem.Instance.TriggerNewBossInit();    //事件：Boss刷新
        BossStateBackground.gameObject.SetActive(true);
        // 等待指定的时间

    }
    private void Shuffle(List<GameObject> list) //打乱List算法
    {
        System.Random rad = new();
        int n = list.Count;
        while (n > 1)
        {
            // 随机选择当前索引或之后的元素进行交换
            int k = rad.Next(n--); // 生成 [0, n) 范围内的随机数
            GameObject temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
    private void InitialBossList()
    {
        
        List<GameObject> list = new List<GameObject>();
        for(int i = BossList.Count - 1 ; i >= 0; i--)
        {
            if (BossList[i].GetComponent<Poker>().cardNumber == 11) //J
            {
                list.Add(BossList[i]);
                BossList.RemoveAt(i);
            }
        }
        Shuffle(list);  //打乱四个J
        tempList.AddRange(list);
        list.Clear();

        for(int i = BossList.Count - 1 ; i >= 0; i--)
        {
            if (BossList[i].GetComponent<Poker>().cardNumber == 12) //Q
            {
                list.Add(BossList[i]);
                BossList.RemoveAt(i);
            }
        }
        Shuffle(list);  //打乱四个Q
        tempList.AddRange(list);
        list.Clear();
        for (int i = BossList.Count - 1 ; i >= 0; i--)
        {
            if (BossList[i].GetComponent<Poker>().cardNumber == 13) //K
            {
                list.Add(BossList[i]);
                BossList.RemoveAt(i);
            }
        }
        Shuffle(list);  //打乱四个K
        tempList.AddRange(list);
        list.Clear();

        BossList.Clear();
        BossList = tempList;
    }
    private void ObtainGem(GameObject boss)    //击杀BOSS后获得宝石
    {
        SEManager.Instance.GemCollected();
        int baseNum = (boss.GetComponent<Poker>().cardNumber - 9) * 4 + 1;
        int delta = Random.Range(0, boss.GetComponent<Poker>().cardNumber - 7 ); //有概率多几颗宝石
        GemBoard.Instance.AddGem(baseNum + delta);
    }
    public void ForbidSuit()    //禁止花色
    {
        ForbidIcon.SetActive(true);
        isSuitForbid = true;
    }
    private void AllowSuit()
    {
        ForbidIcon.SetActive(false);
        isSuitForbid = false;
    }
    public void SetBossLv(int delta)
    {
        BossLv += delta;
    }
    public bool CheckIfVictory()//此函数在boss被击败后，更换boss卡牌之前调用，如果胜利，应该是剩余boss==1
    {
        if (BossList.Count == 1)
        {
            MainThread.Instance.Victory();
            return true;
        }
        else return false;
    }
}
