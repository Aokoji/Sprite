using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class TravelSpriteMessagePanel : PanelTopBase
{
    public GameObject mapNode;
    public GameObject spriteNode;
    public Button closeBtn;
    public Button[] squareBtn;

    public Button backBar;
    public UITool_ScrollView scroll;        //不变scroll
    public TravelSpriteMessageBar clone;

    Dictionary<int,TravelSpriteMessageBar> spriteBarList;
    Dictionary<int, int> squareLock;    //squareid  spid
    int selectSquare;
    public override void init()
    {
        base.init();
        spriteBarList = new Dictionary<int, TravelSpriteMessageBar>();
        squareLock = new Dictionary<int, int>();
        initSpriteBarAnim();
        spriteNode.SetActive(false);
        for (int i = 0; i < squareBtn.Length; i++)
            squareBtn[i].GetComponentInChildren<Text>().text = Config_t_TravelRandom.getOne(i+1).sname;
        selectSquare = 0;
        StartCoroutine(initScrollData());
    }
    void initSpriteBarAnim()
    {
        var anim = spriteNode.GetComponent<Animation>();
        if (anim == null)
        {
            anim = spriteNode.AddComponent<Animation>();
            AnimationClip clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Hide_Panel");
            anim.AddClip(clip, "Hide_Panel");
            clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Show_Panel");
            anim.AddClip(clip, "Show_Panel");
        }
        anim.clip = anim.GetClip("Show_Panel");
    }
    public override void registerEvent()
    {
        base.registerEvent();
        closeBtn.onClick.AddListener(clickClose);
        backBar.onClick.AddListener(clickCloseBar);
        for(int i = 0; i < ConfigConst.SQUARE_MAX; i++)
        {
            int index = i + 1;
            squareBtn[i].onClick.AddListener(() => { clickSquare(index); });
        }
        EventAction.Instance.AddEventGather<int>(eventType.spriteTravelComplete_I, travelComplete);
    }
    IEnumerator initScrollData()
    {
        scroll.initConfig(455, 100, clone.gameObject);
        foreach (var item in PlayerManager.Instance.spriteList)
        {
            var obj = scroll.addItemDefault().GetComponent<TravelSpriteMessageBar>();
            obj.setData(item.Value);
            obj.initAction(goTravelAction);
            obj.gameObject.SetActive(true);
            spriteBarList.Add(item.Key,obj);
        }
        yield return null;
    }
    void refreshSprites()
    {
        foreach (var item in spriteBarList)
        {
            item.Value.setData(PlayerManager.Instance.getSpriteData(item.Key));
        }
    }
    void refreshBtnState()
    {
        for(int i = 0; i < squareBtn.Length; i++)
        {
            squareLock[i] = 0;
            squareBtn[i].GetComponent<Image>().color = Color.white;
            squareBtn[i].GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
        //不可选置灰，不可点，人物头像
        TravelManager.Instance._data.quest.ForEach(item =>
        {
            squareLock[item.squareID] = item.spID;
            squareBtn[item.squareID-1].GetComponent<Image>().color = Color.gray;
            squareBtn[item.squareID-1].GetComponentInChildren<Image>().gameObject.SetActive(true);
            squareBtn[item.squareID-1].GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), PlayerManager.Instance.getSpriteData(item.spID).icon);
        });
    }
    void goTravelAction(int id)
    {
        //不足判定
        var spdata = PlayerManager.Instance.getSpriteData(id);
        if (Config_t_TravelRandom.getOne(selectSquare).spendPhy> spdata.phy_cur)
        {
            PanelManager.Instance.showTips3("妖精体力不足");
            return;
        }
        if (spdata.istraveling)
        {
            PanelManager.Instance.showTips3("妖精正在旅行中");
            return;
        }
        TravelManager.Instance.goTravel(id, selectSquare);
    }
    void travelComplete(int id)
    {
        PlayerManager.Instance.travel_sprite(id, Config_t_TravelRandom.getOne(selectSquare).spendPhy);
        refreshBtnState();
        clickCloseBar();
    }
    void clickSquare(int id)
    {
        if (TravelManager.Instance.checkSquareTraveling(selectSquare))
        {
            PanelManager.Instance.showTips3("该地区已有妖精在旅行");
            return;
        }
        selectSquare = id;
        refreshSprites();
        AnimationTool.playAnimation(spriteNode, "Show_Panel");
    }
    void clickCloseBar()
    {
        AnimationTool.playAnimation(spriteNode, "Hide_Panel", false, ()=> { spriteNode.SetActive(false); });
    }
    void clickClose()
    {
        PanelManager.Instance.DisposePanel();
    }
}
