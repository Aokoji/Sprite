using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class BattlePanel : PanelBase
{
    //创建ui
    public Transform createCardPos; //创建卡牌点
    public Transform createEnemyCardPos; 
    public Transform createCardPosIn; //创建卡牌点
    public Transform showCardPos;
    public Transform showCardPos2;

    public GameObject[] takeCardPos;     //四个点  或3-5
    public GameObject[] takeEnemyPos;     //四个点  或3-5
    public GameObject[] handCardPos;     //5个点
    public GameObject[] enemyCardPos;     //5个点
    public GameObject[] giftsCardPos;     //5个点
    public Transform cardParent;

    //--------------交互按钮--------------------
    public Button endRoundBtn; //回合结束
    public Button dealCardBtn;      //主动抽牌
    //--------------显示面板--------------------
    public Text residuenum; //剩余卡
    public Image spriteIcon;
    public Text health;
    public RectTransform healthimg;
    public Text defence;
    public Text manatext;
    public GameObject[] manaList;
    public GameObject manaExtra;
    public GameObject manaExtra2;

    public Text enemyhealth;
    public RectTransform enemyhealthimg;
    public Text enemydefence;
    public GameObject enemyExtra;
    public GameObject enemyExtra2;

    public GameObject battleSettle;

    public GameObject stateclone;    //状态小图标    image
    public UITool_ScrollView scroll1;

    public GameObject lockPanel;
    //===================   obj    ==========

    private Queue<int> playerque;
    private Queue<int> enemyque;

    private List<CardEntity> takeCardlist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> takeEnemylist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> handCardlist = new List<CardEntity>();
    private List<CardEntity> handEnemylist = new List<CardEntity>();
    private Queue<CardEntity> discardCard = new Queue<CardEntity>();

    private Queue<Action> rankAction = new Queue<Action>();
    SpriteData player;
    SpriteData enemy;

    SpriteData player_a;
    SpriteData enemy_a;
    bool dealbtnAllow;
    private const float healthconstWidth = 100;
    string CARDPATH = "ui/battle/card/";
    enum rank
    {
        none,
        dealcard,
        takecard,
        roundCalcu,
        showcard,
    }
    private rank currank;

    public void initData(SpriteData enemy_data,SpriteData player_data)
    {
        player = player_data;
        enemy = enemy_data;
        battleSettle.SetActive(false);
        scroll1.initConfig(50, 50, stateclone);
        getPlayerNewCardQue();
        getEnemyNewCardQue();
        refreshPlayerData();
        refreshEnemyData();
        refreshMana();
        buff_changed = true;
        refreshState();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        endRoundBtn.onClick.AddListener(endroundClick);
        dealCardBtn.onClick.AddListener(dealcardClick);
    }
    private void getPlayerNewCardQue()
    {
        List<int> list;
        if (PlayerManager.Instance.getPlayerCards().Count > 0)
        {
            //自定义牌组
            list = new List<int>(PlayerManager.Instance.getPlayerCards());
            if (list.Count < 20)
                for (int i = list.Count; i < 20; i++)
                    list.Add(1);
        }
        else
        {
            //默认牌组
            var cards = Config_t_DefaultCardGroup.getOne(PlayerManager.Instance.getcursprite().takeDefaultCardsID);
            string[] ids = cards.cardlist.Split('|');
            list = new List<int>();
            for (int i = 0; i < ids.Length; i++)
                list.Add(int.Parse(ids[i]));
        }
        playerque = CardCalculate.getRandomList(list);
        //buff
        bufflistP.Add(1, 0);
        bufflistP.Add(4, 0);
        initAddBuff(player.id, false);
        refreshCardGroups();
    }
    private void getEnemyNewCardQue()
    {
        var cards = Config_t_DefaultCardGroup.getOne(enemy.takeDefaultCardsID);
        string[] ids = cards.cardlist.Split('|');
        List<int> list = new List<int>();
        for (int i = 0; i < ids.Length; i++)
            list.Add(int.Parse(ids[i]));
        enemyque = CardCalculate.getRandomList(list);
        //buff
        initAddBuff(enemy.id, true);
    }
    void initAddBuff(int id,bool isenemy)
    {
        stateclone.SetActive(false);
        if (isenemy)
        {
            var emy = Config_t_ActorMessage.getOne(enemy.id);
            if (!emy.buff.Equals("0"))
            {
                string[] str =emy.buff.Split('|');
                foreach (var i in str)
                    bufflistE.Add(int.Parse(i), 0);
            }
        }
        else
        {
            string[] levelstr = Config_t_TakeCardLevel.getOne(player.id).stateLevel.Split('|');
            string[] bufstr = Config_t_TakeCardLevel.getOne(player.id).stateid.Split('|');
            for (int i = 0; i < levelstr.Length; i++)
            {
                if (player.level < int.Parse(levelstr[i])) continue;
                if (!bufstr[i].Equals("0"))
                    bufflistP.Add(int.Parse(bufstr[i]), 0);
            }
        }
    }

    #region buff
    public Dictionary<int, int> bufflistP = new Dictionary<int, int>();
    public Dictionary<int, int> bufflistE = new Dictionary<int, int>();
    bool buff_changed;
    float bufficonLength;
    public void AddBuff(int id, bool isenemy = false, int num = 0)
    {
        buff_changed = true;
        if (isenemy)
            if (bufflistE.ContainsKey(id))
                bufflistE[id] += num;
            else
                bufflistE.Add(id, num);
        else
        {
            if (bufflistP.ContainsKey(id))
                bufflistP[id] += num;
            else
                bufflistP.Add(id, num);
        }
    }
    public void removeBuff(int id, bool isenemy = false)
    {
        buff_changed = true;
        if (isenemy)
        {
            if (bufflistE.ContainsKey(id))
                bufflistE.Remove(id);
        }
        else
        {
            if (bufflistP.ContainsKey(id))
                bufflistP.Remove(id);
        }
    }
    //更新一下buff
    public void refreshState()
    {
        if (!buff_changed) return;
        //回合结束也会刷新一下
        scroll1.recycleAll();
        foreach(var i in bufflistP)
        {
            var obj = scroll1.addItemDefault();
            obj.transform.localScale = Vector3.one;
            var script = obj.GetComponent<BuffItem>();
            script.setData(i.Key, i.Value);
        }
        buff_changed = false;
    }
    #endregion


    #region     阶段控制
    //开始阶段
    public void startGame()
    {
        currank = rank.takecard;
        dealbtnAllow = true;
        addAction(() =>
        {
            initCardEnemy(4);
            dealCard(4);
        });
        PanelManager.Instance.showTips1("对局开始", playerNextQue);
    }
    private void addAction(Action action)
    {
        rankAction.Enqueue(action);
    }
    private void playerNextQue()
    {
        if (rankAction.Count <= 0)
        {
            nextRank();
            return;
        }
        rankAction.Dequeue().Invoke();
    }
    void panellock(bool islock)
    {
        lockPanel.SetActive(islock);
    }
    private void nextRank()
    {
        switch (currank)
        {
            case rank.showcard: //计算take牌
                currank = rank.roundCalcu;
                panellock(true);
                EventAction.Instance.TriggerAction(eventType.roundEnd_C, takeCardlist);
                break;
            case rank.roundCalcu:
                currank = rank.dealcard;    //回合结算
                EventAction.Instance.TriggerAction(eventType.playRoundNext);
                break;
            case rank.dealcard: //计算take
                currank = rank.takecard;
                addAction(() => { dealCard(1); });
                dealEnemyCard(1);
                break;
            case rank.takecard:
                currank = rank.showcard;
                refreshMana();
                StringBuilder str = new StringBuilder();
                foreach (var i in handEnemylist)
                    str.Append(i._data.sname);
                PubTool.Log(str.ToString());
                //回合开始
                PanelManager.Instance.showTips1("回合开始", PanelManager.Instance.panelUnlock);
                break;
        }
    }
    #endregion
    public void initCardEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(enemyque.Dequeue());
            var item = newcard(data, true);
            item.clickAllow = false;
            handEnemylist.Add(item);
            item.transform.position = enemyCardPos[handEnemylist.Count - 1].transform.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }
    }

    int finishNum;
    public void dealCard(int num)
    {
        //屏蔽点击
        panellock(true);
        StartCoroutine(rundeal(num));
        //抽牌结束开放
    }
    public void dealEnemyCard(int num)
    {
        StartCoroutine(runEnemydeal(num));
    }
    IEnumerator rundeal(int num)
    {
        finishNum = 0;
        for(int i = 0; i < num; i++)
        {
            //判断没牌
            var data = Config_t_DataCard.getOne(playerque.Count <= 0? player.underCard:playerque.Dequeue());
            var item = newcard(data);
            item.transform.position = createCardPos.position;
            item.transform.eulerAngles = createCardPosIn.eulerAngles;
            item.transform.localScale = Vector3.one / 2;
            item.gameObject.SetActive(true);
            if (handCardlist.Count == ConfigConst.maxCardHand)
                tearCardAnim(item);
            else
            {
                handCardlist.Add(item);
                dealCardAnim(item,handCardlist.Count);
            }
            yield return new WaitForSeconds(ConfigConst.cardwait);
        }
        while (finishNum < num)
            yield return null;
        panellock(false);
        playerNextQue();
    }
    IEnumerator runEnemydeal(int num)
    {
        for (int i = 0; i < num; i++)
        {
            t_DataCard data;
            if (enemyque.Count > 0)
                data = Config_t_DataCard.getOne(enemyque.Dequeue());
            else
                data= Config_t_DataCard.getOne(enemy.underCard);
            var item = newcard(data, true);
            item.transform.position = createEnemyCardPos.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            if (handEnemylist.Count == ConfigConst.maxCardHand)
                tearCardAnim(item);
            else
            {
                handEnemylist.Add(item);
                dealCardEnemyAnim(item, handEnemylist.Count);
            }
            yield return new WaitForSeconds(0.5f);
        }
        playerNextQue();
    }

    public List<CardEntity> getEnemyround(SpriteData data)
    {
        takeEnemylist = EnemyCalculate.Instance.calculateEnemyAction(handEnemylist, data, player, handCardlist.Count, dealEnemyCard);
        return takeEnemylist;
    }
    private CardEntity dealEnemyCard()
    {
        var data = Config_t_DataCard.getOne(enemy.dealCard);
        var item = newcard(data, true);
        item.transform.position = enemyCardPos[2].transform.position;
        item.transform.eulerAngles = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.gameObject.SetActive(true);
        return item;
    }
    //整理卡槽
    private void refreshCard()
    {
        //整理卡槽
        for(int i = 0; i < handCardlist.Count; i++)
        {
            if (handCardlist[i].transform.position != handCardPos[i].transform.position)
            {
                int index = i;
                handCardlist[i].clickAllow = false;
                RunSingel.Instance.moveToAll(handCardlist[index].gameObject, handCardPos[index].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero,()=> { handCardlist[index].clickAllow = true; });
            }
        }
    }
    private void refreshEnemyCard()
    {
        //整理卡槽
        for (int i = 0; i < handEnemylist.Count; i++)
        {
            if (handEnemylist[i].transform.position != enemyCardPos[i].transform.position)
            {
                int index = i;
                handEnemylist[i].backCard();
                RunSingel.Instance.moveToAll(handEnemylist[i].gameObject, enemyCardPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero);
            }
        }
    }
    private void refreshTakeCard()
    {
        //整理卡槽
        for (int i = 0; i < takeCardlist.Count; i++)
        {
            if (takeCardlist[i].transform.position != takeCardPos[i].transform.position)
            {
                int index = i;
                takeCardlist[i].clickAllow = false;
                RunSingel.Instance.moveToAll(takeCardlist[i].gameObject, takeCardPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero, () => { takeCardlist[index].clickAllow = true; });
            }
        }
    }
    private void refreshEnemyTakeCard()
    {
        for (int i = 0; i < takeEnemylist.Count; i++)
        {
            if (takeEnemylist[i].transform.position != takeEnemyPos[i].transform.position)
            {
                int index = i;
                RunSingel.Instance.moveToAll(takeEnemylist[i].gameObject, takeEnemyPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero);
            }
        }
    }
    private void refreshCardGroups()
    {
        residuenum.text = playerque.Count.ToString();
    }

    private void chooseCard(CardEntity card)
    {
        if (!card.isStaying && takeCardlist.Count >= ConfigConst.maxCardTake) return;//加个提示
        if (!card.isStaying && card._data.cost > player.cost_cur) return;
        card.clickAllow = false;
        if (card.isdealcreate)
        {
            takeCardlist.Remove(card);
            card.playJustHideAnim(() => {
                player.cost_cur += card._data.cost;
                refreshMana();
                card.gameObject.SetActive(false);
                discardCard.Enqueue(card);
                refreshTakeCard();
            });
            return;
        }
        if (card.isStaying)
        {
            //回卡槽
            player.cost_cur += card._data.cost;
            refreshMana();
            card.isStaying = false;
            takeCardlist.Remove(card);
            handCardlist.Add(card);
        }
        else
        {
            //飞上去
            player.cost_cur -= card._data.cost;
            refreshMana();
            card.isStaying = true;
            handCardlist.Remove(card);
            takeCardlist.Add(card);
        }
        refreshTakeCard();
        refreshCard();
    }

    private CardEntity newcard(t_DataCard data, bool isback = false, bool isextra = false)
    {
        CardEntity item;
        if (discardCard.Count > 0)
        {
            item = discardCard.Dequeue();
            item.transform.SetParent(cardParent);
        }
        else
            item = PanelManager.Instance.LoadUI(E_UIPrefab.cardHand, CARDPATH, cardParent).GetComponent<CardEntity>();
        item.isback = isback;
        item.initData(data, isextra);
        item.onChoose = chooseCard;
        return item;
    }
    private void dealCardAnim(CardEntity card,int topos)
    {
        refreshCardGroups();
        RunSingel.Instance.moveToAll(card.gameObject,showCardPos.position,MoveType.moveAll_FTS, ConfigConst.cardtime_dealtoshow, Vector3.one, Vector3.zero, ()=> {
            RunSingel.Instance.moveToBezier(card.gameObject, showCardPos2.position,Vector3.Lerp(showCardPos.position,showCardPos2.position,0.5f)+Vector3.up* (showCardPos2.position.y-showCardPos.position.y)/2, ConfigConst.cardtime_showstay,()=> {
                RunSingel.Instance.moveToAll(card.gameObject, handCardPos[topos - 1].transform.position, MoveType.moveAll_STF, ConfigConst.cardtime_showtohand, Vector3.one, Vector3.zero,()=> { finishNum++; });
            });
        });
    }
    private void dealCardEnemyAnim(CardEntity card, int topos)
    {
        RunSingel.Instance.moveToAll(card.gameObject, enemyCardPos[topos - 1].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_showtohand, Vector3.one, Vector3.zero);
    }
    private void tearCardAnim(CardEntity obj)
    {
        //撕卡
        RunSingel.Instance.moveToAll(obj.gameObject, showCardPos.position, MoveType.moveAll_FTS, ConfigConst.cardtime_dealtoshow, Vector3.one, Vector3.zero, () => {
            RunSingel.Instance.moveToBezier(obj.gameObject, showCardPos2.position, Vector3.Lerp(showCardPos.position, showCardPos2.position, 0.5f) + Vector3.up * (showCardPos2.position.y - showCardPos.position.y) / 2, ConfigConst.cardtime_showstay, () => {
                //撕毁动画
                obj.playCounterAnim(() => { obj.gameObject.SetActive(false); });
                discardCard.Enqueue(obj);
                finishNum++;
            });
        });
    }
    private void dealcardClick()
    {
        if (player.cost_cur < 2) return;
        if (!dealbtnAllow) return;
        dealbtnAllow = false;
        player.cost_cur -= 2;
        refreshMana();
        var item = newcard(Config_t_DataCard.getOne(player.dealCard));
        takeCardlist.Add(item);
        item.clickAllow = false;
        item.transform.position = takeCardPos[takeCardlist.Count - 1].transform.position;
        item.transform.eulerAngles = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.isdealcreate = true;
        item.isStaying = true;
        item.gameObject.SetActive(true);
        item.playJustShowAnim(()=> { item.clickAllow = true; dealbtnAllow = true; });
    }
    private void endroundClick()
    {
        player_a = player.Copy();
        enemy_a = enemy.Copy();
        playerNextQue();
    }
    void refreshPDataTemp()
    {
        health.text = "health:" + player_a.hp_cur + "/" + player_a.hp_max;
        defence.text = player_a.def_cur.ToString();
        healthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)player_a.hp_cur / player_a.hp_max * healthconstWidth);
        enemyhealth.text = "health:" + enemy_a.hp_cur + "/" + enemy_a.hp_max;
        enemydefence.text = enemy_a.def_cur.ToString();
        enemyhealthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)enemy_a.hp_cur / enemy_a.hp_max * healthconstWidth);
    }
    public void refreshPlayerData()
    {
        health.text = "health:"+player.hp_cur+"/"+player.hp_max;
        defence.text = player.def_cur.ToString();
        healthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)player.hp_cur / player.hp_max * healthconstWidth);
    }
    public void refreshEnemyData()
    {
        enemyhealth.text = "health:" + enemy.hp_cur + "/" + enemy.hp_max;
        enemydefence.text = enemy.def_cur.ToString();
        enemyhealthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)enemy.hp_cur / enemy.hp_max * healthconstWidth);
        enemyExtra.SetActive(enemy.extraLimit >= 1);
        enemyExtra2.SetActive(enemy.extraLimit >= 2);
    }
    public void refreshMana()
    {
        manatext.text = "mana:" + player.cost_cur + "/" + player.cost_max;
        for(int i = 0; i < manaList.Length; i++)
        {
            manaList[i].SetActive(i < player.cost_cur);
        }
        manaExtra.SetActive(player.extraLimit >= 1);
        manaExtra2.SetActive(player.extraLimit >= 2);
    }
    //腾一下展示桌面  准备回合生效
    public void playRoundWillShow()
    {
        //小动画
        refreshEnemyTakeCard();
        refreshEnemyCard();
        RunSingel.Instance.laterDo(0.5f, playerNextQue);
    }
    public void playThisCard(RoundData dataround)
    {
        //Debug.Log("playcard====" + dataround._card.sname+dataround.hitnum);
        bool isplayer = dataround.isplayer;
        //播放卡
        addAction(() =>
        {
            //置顶
            dataround.entity.transform.SetAsLastSibling();
            if (!isplayer)
                dataround.entity.turnCard();
            RunSingel.Instance.moveToAll(dataround.entity.gameObject, dataround.entity.transform.position + (isplayer ? Vector3.up : Vector3.down), MoveType.moveAll_FTS, ConfigConst.cardtime_effectShow, Vector3.one * 1.5f, Vector3.zero, () => {
                //消失动画
                if (dataround.isCounter)
                    dataround.entity.playCounterAnim(playerNextQue);
                else
                {
                    playerNextQue();
                }
            });
        });
        if (!dataround.isCounter)
        {
            Vector3 pos1 = isplayer ? enemyhealth.transform.position : health.transform.position;   //伤害
            Vector3 pos2 = isplayer ? health.transform.position : enemyhealth.transform.position;   //buff
            //先抽牌
            if (dataround.dealnum > 0)
            {
                if (isplayer) addAction(() => { dealCard(dataround.dealnum); });
                else addAction(() => { dealEnemyCard(dataround.dealnum); });
            }
            if (dataround.haveCounter)
            {
                addAction(() => {
                    //诅咒特效
                    ParticleManager.Instance.playEffect(E_Particle.particle_counter, dataround.entity.transform.position);
                    RunSingel.Instance.laterDo(2.8f, playerNextQue);
                });
            }
            if (dataround.isdecounter)
            {
                addAction(() => {
                    easybuffAnimPlay(E_Particle.particle_ani_deconter, pos2);
                });
            }
            if (dataround.recovernum > 0)
            {
                addAction(() => {
                    if (isplayer) player_a.hp_cur = Mathf.Min(player_a.hp_max, player_a.hp_cur + dataround.recovernum);
                    else enemy_a.hp_cur= Mathf.Min(enemy_a.hp_max, enemy_a.hp_cur + dataround.recovernum);
                    easybuffAnimPlay(E_Particle.particle_ani_recover, pos2, "+" + dataround.recovernum);
                });
            }
            if (dataround.havePower)
            {
                addAction(() =>
                {   //power升级效果
                    var par = ParticleManager.Instance.getPlayEffect(E_Particle.particle_power, dataround.entity.transform.position);
                    RunSingel.Instance.laterDo(1.5f, () =>
                    {
                        par.SetActive(false);
                        playerNextQue();
                    });
                });
            }
            //攻击
            if (dataround.hitnum > 0)
            {
                addAction(() => {
                    E_Particle willanim;
                    if (dataround.isbroken)
                    {
                        willanim = E_Particle.particle_brokenhit;
                        if(isplayer)
                            enemy_a.hp_cur = Mathf.Max(enemy_a.hp_cur - dataround.hitnum, 0);
                        else
                            player_a.hp_cur = Mathf.Max(player_a.hp_cur - dataround.hitnum, 0);
                    }
                    else
                    {
                        willanim = E_Particle.particle_hit;
                        if (isplayer)
                        {
                            enemy_a.def_cur = Mathf.Max(0, enemy_a.def_cur - dataround.hitdef);
                            enemy_a.hp_cur = Mathf.Max(0, enemy_a.hp_cur - dataround.hitnum + dataround.hitdef);
                        }
                        else
                        {
                            player_a.def_cur = Mathf.Max(0, player_a.def_cur - dataround.hitdef);
                            player_a.hp_cur = Mathf.Max(0, player_a.hp_cur - dataround.hitnum + dataround.hitdef);
                        }

                    }
                    ParticleManager.Instance.playEffect_special(willanim, pos1, "-" + dataround.hitnum, () =>
                    {
                        //伤害结束回调
                        refreshPDataTemp();
                        playerNextQue();
                    });
                });
            }
            if (dataround.defnum > 0)
            {
                addAction(() => {
                    if (isplayer) player_a.def_cur += dataround.defnum;
                    else enemy_a.def_cur += dataround.defnum;
                    easybuffAnimPlay(E_Particle.particle_ani_def, pos2, "+" + dataround.defnum);
                });
            }
            if (dataround.gift.Count > 0)
            {
                addAction(() => {
                    int[] str;
                    List<CardEntity> desList = new List<CardEntity>();
                    switch (dataround.gift.Count)
                    {
                        case 2: str =new int[] { 1, 3 };break;
                        case 3: str =new int[]{ 1, 2, 3 };break;
                        case 4: str =new int[]{ 0, 1, 2, 3 };break;
                        case 5: str =new int[]{ 0, 1, 2, 3, 4 };break;
                        default:str = new int[] { 2 };break;
                    }
                    for (int i = 0; i < dataround.gift.Count; i++)
                    {
                        var item = newcard(Config_t_DataCard.getOne(dataround.gift[i]), false, true);
                        item.transform.position = giftsCardPos[str[i]].transform.position;
                        if (dataround.isplayer)
                            if (handCardlist.Count >= ConfigConst.maxCardHand)
                                desList.Add(item);
                            else
                                handCardlist.Add(item);
                        else
                            if (handEnemylist.Count >= ConfigConst.maxCardHand)
                                desList.Add(item);
                            else
                                handEnemylist.Add(item);
                        int index = i;
                        item.playJustShowAnim(() =>
                        {
                            if (index == dataround.gift.Count-1)
                            {
                                RunSingel.Instance.laterDo(1.2f, () => {
                                    if (dataround.isplayer)
                                        refreshCard();
                                    else
                                        refreshEnemyCard();
                                    foreach (var cad in desList)
                                        cad.playCounterAnim(null);
                                    RunSingel.Instance.laterDo(1.5f, playerNextQue);
                                });
                            }
                        });
                    }
                });
            }
            if (dataround.extraLimit > 0)
            {
                //播动画
                addAction(() => {
                    if(dataround.isplayer)
                        refreshMana();
                    else
                        refreshEnemyData();
                    RunSingel.Instance.laterDo(0.5f, playerNextQue);
                });
            }
            if (dataround.addition > 0)
            {
				addAction(() => {
                    dataround.entity.gameObject.SetActive(false);
                    if (dataround.isplayer)
                    {
                        playerque = CardCalculate.addOneCard(playerque, dataround.addition);
                        var item = newcard(Config_t_DataCard.getOne(dataround.addition));
                        item.transform.position = giftsCardPos[2].transform.position;
                        item.playNormalShowAnim(() =>
                        {
                            item.playTurnBackAnim(() =>
                            {
                                RunSingel.Instance.moveToAll(item.gameObject, createCardPos.position, MoveType.moveAll_STF, ConfigConst.cardtime_addition, Vector3.one / 2, createCardPosIn.eulerAngles, () => { refreshCardGroups(); playerNextQue(); });
                            });
                        });
                    }
                    else
                    {
                        playerNextQue();
                    }
				});
            }
            if (dataround.etch)
            {
                dataround.entity.playCounterAnim(playerNextQue);
            }
        }
        //下一张
        addAction(() => {
            dataround.entity.gameObject.SetActive(false);
            cardAlign(dataround);      //对齐
            //刷新状态
            refreshState();
        });
        addAction(() => { EventAction.Instance.TriggerAction(eventType.playRoundNext); });
        playerNextQue();
    }
    void easybuffAnimPlay(E_Particle sname, Vector3 pos, string hit="")
    {
        refreshPDataTemp();
        ParticleManager.Instance.playEffect_special(sname, pos, hit, playerNextQue);
    }
    //展示卡对齐（回调
    void cardAlign(RoundData data)
    {
        //对齐
        discardCard.Enqueue(data.entity);
        if (data.isplayer)
        {
            takeCardlist.Remove(data.entity);
            refreshTakeCard();
        }
        else
        {
            takeEnemylist.Remove(data.entity);
            refreshEnemyTakeCard();
        }
        RunSingel.Instance.laterDo(ConfigConst.cardtime_refshMove, playerNextQue);
    }
    public void roundEndAndContinue()
    {
        refreshState();
        playerNextQue();
    }
    public void gameSettle(bool iswin)
    {
        //播放动画，然后出结算
        //战斗胜利弹窗
        PanelManager.Instance.panelLock();
        AnimationTool.playAnimation(battleSettle, iswin ? "winsettle" : "losesettle", false, () =>
        {
            BattleManager.Instance.settleStep(iswin);
        });
    }
}
