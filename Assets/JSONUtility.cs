using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class JSONUtility : MonoBehaviour {
	[Serializable] //なくても一応変換はされるが、一部挙動が変になるので必ずつけておくべき
	public class Item
	{
	    public string name;
	    public int price;
	    public int value;
	}
	[Serializable]
	public class Items{
		public Item[] itemList;
	}

	// Use this for initialization
	void Start () {
		var textAsset = Resources.Load ("jsontext") as TextAsset;
		Debug.Log (textAsset.text);
 		string itemJson = textAsset.text;

 		var items = JsonUtility.FromJson<Items>(itemJson);
 		Debug.Log (items.itemList[0].name);
 		Debug.Log (items.itemList[1].name);
// Debug.Log("items " + item);
// Debug.Log("item price " + item.price);
// Debug.Log("item name " + item.name);
// Debug.Log("item value " + item.value);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
