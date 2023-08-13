using System.Collections.Generic;using UnityEditor;using System;public class Config_t_ActorMessage : AssetData{	public List<t_ActorMessage> asset = new List<t_ActorMessage>();	public void addContent(t_ActorMessage item)	{		asset.Add(item);	}	public static bool isloaded;	public static Dictionary<int, t_ActorMessage> _data { get; private set; }	public void init()	{		_data = new Dictionary<int, t_ActorMessage>();		for (int i = 0; i < asset.Count; i++){			_data.Add(asset[i].id, asset[i]);		}		asset = null;		isloaded = true;	}	public static t_ActorMessage getOne(int id)	{		if (_data.ContainsKey(id))			return _data[id];		else			throw new Exception("not find" + id);	}	public static void Dispose()	{		_data = null;	}}[Serializable]public class t_ActorMessage{	public int id;	public string sname;	public int defaultCard;	public string titleIcon;	public string wholeIcon;	public int phybase;	public int hpbase;	public int costmax;	public int spritePower;	public string drop;	public string dropRare;	public string dropCommon;	public int volume;	public int odds;}