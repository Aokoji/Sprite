using System.Collections.Generic;using UnityEditor;using System;public class Config_t_DataCard : AssetData{	public List<t_DataCard> asset = new List<t_DataCard>();	public void addContent(t_DataCard item)	{		asset.Add(item);	}	public static bool isloaded;	public static Dictionary<int, t_DataCard> _data { get; private set; }	public void init()	{		_data = new Dictionary<int, t_DataCard>();		for (int i = 0; i < asset.Count; i++){			_data.Add(asset[i].id, asset[i]);		}		asset = null;		isloaded = true;	}	public static t_DataCard getOne(int id)	{		if (_data.ContainsKey(id))			return _data[id];		else			throw new Exception("not find" + id);	}	public static void Dispose()	{		_data = null;	}}[Serializable]public class t_DataCard{	public int id;	public string sname;	public string sDescribe;	public CardType1 type1;	public CardType2 type2;	public CardSelfType limit;	public CardType2 conditionType1;	public CardType2 conditionType2;	public CardType2 conditionType3;	public int damage1;	public int damage2;	public int damage3;	public int cost;	public int limitcount;}