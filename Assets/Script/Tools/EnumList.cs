using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace common_word
{
    /// <summary>
    /// 错误等级
    /// warn1 操作有误 不影响流程
    /// warn2 
    /// warn3 
    /// error1 引用逻辑错误
    /// error2 显示错误
    /// </summary>
    public enum warnLevel
    {
        warn1,warn2,warn3,warn4,
        error1, error2
    }

    public enum elementType
    {
        fire=1,       //火
        water=2,      //水
        wind=3,       //风
        soil=4,       //土

        thunder=5,        //雷     火风
        timber=6,     //木     水土
        snow=7,        //雪     水风
        light=8,        //光      火水
        gold=9,        //金      火土
        shadow=10,       //影      风土
        none=11,
    }
    public class elementGet
    {
        public static elementType getElement(int id)
        {
            switch (id)
            {
                case 1:return elementType.fire;
                case 2:return elementType.water;
                case 3:return elementType.wind;
                case 4:return elementType.soil;
                case 5:return elementType.thunder;
                case 6:return elementType.timber;
                case 7:return elementType.snow;
                case 8:return elementType.light;
                case 9:return elementType.gold;
                case 10:return elementType.shadow;
                default:return elementType.none;
            }
        }
    }
    public enum recycleTips
    {
        choose_normalWindow,
        actor_states,
        actor_task,
    }
    //地图方向
    public enum mapDirect
    {
        north=1,
        northeast=2,
        east=3,
        southeast=4,
        south=5,
        southwest=6,
        west=7,
        northwest=8
    }
}

namespace common_event
{
    public enum eventName
    {
        showtip,    //string
        showpop,    //string
        loggame,
        worldevent,     //PropertyChangeData
        panelChange,    //string
        panelChangeLoadingComplete,

        task_add,   //int 
        clearWindow,
        act_kill,       //  int  int
        act_propertyItem,   //int

        action_move,
        action_wait,

        action_allow,
        action_windowOpen,      //recycleTips
        action_windowOpenTask,      //
    }
}

