using System.Collections;using UnityEditor;public class TableConfig{	string ASSETPATH = "config/tabledata/";	public bool loadsuccess;	public void init()	{		loadsuccess = false;		RunSingel.Instance.runTimer(loadData());	}	IEnumerator loadData()	{		AssetManager.loadAsset<Config_t_ActorMessage>(ASSETPATH +"t_ActorMessage_data").init();		while (!Config_t_ActorMessage.isloaded)			yield return null;		AssetManager.loadAsset<Config_t_DataCard>(ASSETPATH +"t_DataCard_data").init();		while (!Config_t_DataCard.isloaded)			yield return null;		AssetManager.loadAsset<Config_t_DefaultCardGroup>(ASSETPATH +"t_DefaultCardGroup_data").init();		while (!Config_t_DefaultCardGroup.isloaded)			yield return null;		AssetManager.loadAsset<Config_t_items>(ASSETPATH +"t_items_data").init();		while (!Config_t_items.isloaded)			yield return null;		AssetManager.loadAsset<Config_t_quest>(ASSETPATH +"t_quest_data").init();		while (!Config_t_quest.isloaded)			yield return null;		AssetManager.loadAsset<Config_t_TravelRandom>(ASSETPATH +"t_TravelRandom_data").init();		while (!Config_t_TravelRandom.isloaded)			yield return null;		loadsuccess = true;	}	public void Dispose()	{		Config_t_ActorMessage.Dispose();		Config_t_DataCard.Dispose();		Config_t_DefaultCardGroup.Dispose();		Config_t_items.Dispose();		Config_t_quest.Dispose();		Config_t_TravelRandom.Dispose();	}}