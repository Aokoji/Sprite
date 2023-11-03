using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopBattle_SpriteState : UIBase
{
    public Image spriteIcon;
    public Text health;
    public RectTransform healthimg;
    public Text defence;
    public Text manatext;
    public GameObject[] manaList;
    public GameObject manaExtra;
    public GameObject manaExtra2;

    public GameObject AnimPos;  //播例如治疗动画位置

    SpriteData _data;
    private const float healthconstWidth = 100;

    public void initSprite(SpriteData data)
    {
        //icon
        setSpriteState(data);
        refreshMana();
    }
    void refreshSelf(bool isflow=false)
    {
        setSpriteState(_data, isflow);
    }
    public void setSpriteState(SpriteData data,bool isflow=false)
    {
        health.text = "health:" + data.hp_cur + "/" + data.hp_max;
        defence.text = data.def_cur.ToString();
        healthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)data.hp_cur / data.hp_max * healthconstWidth);

        //可加改变特效
        _data = data;
    }

    public void refreshMana()
    {
        manatext.text = "mana:" + _data.cost_cur + "/" + _data.cost_max;
        for (int i = 0; i < manaList.Length; i++)
        {
            manaList[i].SetActive(i < _data.cost_cur);
        }
        manaExtra.SetActive(_data.extraLimit >= 1);
        manaExtra2.SetActive(_data.extraLimit >= 2);
    }

    public void playActorAnim(E_Particle particle, string hit = "", Action callback = null)
    {
        //pos有特定位置  AnimPos
        ParticleManager.Instance.playEffect_special(particle, AnimPos.transform.position, hit, callback);
    }
}
