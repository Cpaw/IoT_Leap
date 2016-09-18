using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {
	public Text Text_Name;
	public Text Text_Price;
	public Text Text_Value;
	public string str_name;
	public string str_value;
	public string str_price;
	[SerializeField]
	RawImage ItemImage;

	// Use this for initialization
	void Start () {
		//StartCoroutine(GetStreetViewImage("http://localhost:2000/images/Orange.jpg"));
		//StartCoroutine(GetItems("http://localhost:2000"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetItem(string name,int price,int value){
		Text_Name.text = name;
		Text_Price.text =price.ToString() ;
		Text_Value.text = value.ToString();
	}
	public void test(string url){
		Debug.Log("recieve");
		StartCoroutine(GetItemImage(url));
	}

	private IEnumerator GetItemImage(string url) {
		//現在地マーカーはここの「&markers」以下で編集可能
     	Debug.Log(url);

		WWW www = new WWW(url);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) { // ダウンロードでエラーが発生した
            Debug.Log(www.error);
        }
        ItemImage.texture = www.texture;
 		ItemImage.material.mainTexture = www.texture;
 		ItemImage.color = new Color(ItemImage.color.r, ItemImage.color.g, ItemImage.color.b, 1f);
 	}

}
