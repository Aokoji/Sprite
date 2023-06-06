using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelSpriteMessagePanel : PanelTopBase
{
    public GameObject mapNode;
    public GameObject spriteNode;
    public Button closeBtn;
    public Button[] squareBtn;

    public Button backBar;
    public UITool_ScrollView scroll;
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
    }
    IEnumerator initScrollData()
    {
        foreach (var item in PlayerManager.Instance.spriteList)
        {
            var obj = Instantiate(clone);
            obj.setData(item.Value);
            obj.initAction(goTravelAction);
            obj.gameObject.SetActive(true);
            spriteBarList.Add(item.Key,obj);
            scroll.addNewItem(obj.gameObject);
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
        }
        //不可选置灰，不可点，人物头像
        TravelManager.Instance._data.quest.ForEach(item =>
        {
            squareLock[item.squareID] = item.spID;
        });
        //刷新状态
    }
    void goTravelAction(int id)
    {
        //不足判定
        if (Config_t_TravelRandom.getOne(selectSquare).spendPhy>PlayerManager.Instance.getSpriteData(id).phy_cur)
        {
            PanelManager.Instance.showTips3("妖精体力不足");
            return;
        }
        TravelManager.Instance.goTravel(id, selectSquare);
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
        AnimationTool.playAnimation(gameObject, "Hide_Panel", false, ()=> { spriteNode.SetActive(true); });
    }
    void clickClose()
    {
        PanelManager.Instance.DisposePanel();
    }
}
