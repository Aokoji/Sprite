using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MillUpgradePanel : PanelTopBase
{
    public Button upgrade;
    public Button back;
    public Text upContext;
    public Text leveltext;
    public Image levelExplain;  //升级改变直接用图片解释
    public GameObject[] needsNode;

    int level;
    bool isallFinish;
    //固定数据id1-5
    int getConfigID(int level)
    {
        return Mathf.Min(ConfigConst.UPGRADE_MAX,level + 1);
    }
    public override void init()
    {
        base.init();
        isallFinish = false;
        refreshLevelData();
    }
    void refreshLevelData()
    {
        level = PlayerManager.Instance.Milldata.extendLv;
        leveltext.text = "当前等级：" + level;
        if (level == ConfigConst.UPGRADE_MAX)
            upContext.text = "以达到等级上限";
        else
            upContext.text = "升级所需材料：";
        var data = Config_t_UpgradeConfig.getOne(getConfigID(level));
        string[] strneed = data.needid.Split('|');
        string[] strcount = data.needcount.Split('|');
        int id;
        int count;
        isallFinish = true;
        int havecount;
        for(int i = 0; i < needsNode.Length; i++)
        {
            if (i < strneed.Length)
            {
                needsNode[i].SetActive(true);
                id = int.Parse(strneed[i]);
                count = int.Parse(strcount[i]);
                //设置图片
                needsNode[i].GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(id).iconName);
                //设置文本
                havecount = PlayerManager.Instance.getItem(id);
                if (havecount < count)
                    isallFinish = false;
                needsNode[i].GetComponentInChildren<Text>().text = havecount + "/" + count;
            }
            else
            {
                needsNode[i].SetActive(false);
            }
        }
        //按钮状态
        if (isallFinish)
            upgrade.GetComponent<Image>().color = Color.white;
        else
            upgrade.GetComponent<Image>().color = Color.gray;
    }
    public override void registerEvent()
    {
        base.registerEvent();
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        upgrade.onClick.AddListener(clickUpgrade);
    }

    void clickUpgrade()
    {
        if (!isallFinish)
            return;
        PlayerManager.Instance.upgradeMill(() =>
        {
            PanelManager.Instance.showTips3("磨坊升级成功");
            PanelManager.Instance.DisposePanel();
        });
    }
}
