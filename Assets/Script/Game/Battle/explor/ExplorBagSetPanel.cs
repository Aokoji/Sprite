using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorBagSetPanel : PanelTopBase
{
    public GameObject messbar;
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button[] itembags;

    public Text _sname;
    public Text _sdes;
    public Button takebagBtn;

    public Button confirm;
    public Button back;

    int curChoose;
    Dictionary<int, int> itemlist;
    List<int> finalBagList;
    bool allow;
    bool changed;
    public override void init()
    {
        base.init();
        itemlist = new Dictionary<int, int>();
        finalBagList = new List<int>();
        messbar.SetActive(false);
        allow = true;
        changed = false;
        var bagdata = PlayerManager.Instance.getExplorData().explorBag;
        //转换数据
        foreach(var i in bagdata)
        {
            if (!itemlist.ContainsKey(i))
                itemlist[i] = 0;
            itemlist[i]++;
        }
        refreshBagPreview();
        scroll.initConfig(80, 80, clone);
        StartCoroutine(setScroll());
    }

    public override void registerEvent()
    {
        base.registerEvent();
        takebagBtn.onClick.AddListener(clickTakeBag);
        confirm.onClick.AddListener(clickConfirm);
        back.onClick.AddListener(() =>
        {
            if (changed)
                PanelManager.Instance.showTips2("探险背包还未保存，是否放弃本次更改？", PanelManager.Instance.DisposePanel);
            else
                PanelManager.Instance.DisposePanel();
        });
        for (int i = 0; i < itembags.Length; i++)
        {
            int index = i;
            itembags[i].onClick.AddListener(() => { clickReleaseOne(index); });
        }
    }

    IEnumerator setScroll()
    {
        scroll.recycleAll();
        var items = PlayerManager.Instance.playerItemDic;
        t_items config;
        foreach(var item in items)
        {
            config = Config_t_items.getOne(item.Key);
            if (config.type == ItemsType.magic || (config.type == ItemsType.consum && config.type2 == ItemType2.explor))
            {
                //冒险用  消耗品
                var script = scroll.addItemDefault().GetComponent<BagItem>();
                script.setData(item.Key,item.Value);
                script.initAction(chooseOne);
            }
        }
        scroll.reCalculateHeigh();
        yield return null;
    }
    void refreshBagPreview()
    {
        finalBagList.Clear();
        foreach(var i in itemlist)
        {
            for (int k = 0; k < i.Value; k++)
                finalBagList.Add(i.Key);
        }
        for(int i = 0; i < 5; i++)
        {
            if (i < finalBagList.Count)
            {
                itembags[i].gameObject.SetActive(true);
                itembags[i].GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(finalBagList[i]).iconName);
            }
            else
            {
                itembags[i].gameObject.SetActive(false);
            }
        }
    }

    void chooseOne(int id)
    {
        var data = Config_t_items.getOne(id);
        curChoose = id;
        _sname.text = data.sname;
        _sdes.text = data.describe;
        if (itemlist[curChoose] >= PlayerManager.Instance.getItem(curChoose))
        {
            takebagBtn.GetComponent<Image>().color = Color.gray;
            allow = false;
        }
        else
        {
            takebagBtn.GetComponent<Image>().color = Color.white;
            allow = true;
        }
        messbar.SetActive(true);
    }
    void clickTakeBag()
    {
        if (!allow)
        {
            PanelManager.Instance.showTips3("数量不足");
            return;
        }
        if (finalBagList.Count >= 5)
        {
            PanelManager.Instance.showTips3("背包已满");
            return;
        }
        changed = true;
        if (!itemlist.ContainsKey(curChoose))
            itemlist[curChoose] = 0;
        itemlist[curChoose]++;
        if (itemlist[curChoose] >= PlayerManager.Instance.getItem(curChoose))
        {
            takebagBtn.GetComponent<Image>().color = Color.gray;
            allow = false;
        }
        refreshBagPreview();
    }
    void clickReleaseOne(int index)
    {
        int id = finalBagList[index];
        changed = true;
        if (curChoose==id && !allow)
        {
            allow = true;
            takebagBtn.GetComponent<Image>().color = Color.white;
        }
        itemlist[id]--;
        if (itemlist[id] == 0)
            itemlist.Remove(id);
        refreshBagPreview();
    }
    void clickConfirm()
    {
        PlayerManager.Instance.setExplorBag(finalBagList, () =>
        {
            PanelManager.Instance.showTips3("保存成功");
        });
        changed = false;
    }
}
