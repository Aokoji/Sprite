using System.Collections.Generic;using UnityEditor;using System;public class Config_t_items : AssetData{	public List<t_items> asset = new List<t_items>();	public void addContent(t_items item)	{		asset.Add(item);	}	public static bool isloaded;	public static Dictionary<int, t_items> _data { get; private set; }	public void init()	{		_data = new Dictionary<int, t_items>();		for (int i = 0; i < asset.Count; i++){			_data.Add(asset[i].id, asset[i]);		}		asset = null;		isloaded = true;	}	public static t_items getOne(int id)	{		if (_data.ContainsKey(id))			return _data[id];		else			throw new Exception("not find" + id);	}	public static void Dispose()	{		_data = null;	}}[Serializable]public class t_items{	public int id;	public string sname;	public string describe;	public int type;	public int type2;	public string iconName;	public int pay;}