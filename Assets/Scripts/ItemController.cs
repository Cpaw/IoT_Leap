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
	RawImage rawimage;

	// Use this for initialization
	void Start () {
		StartCoroutine(GetStreetViewImage("http://localhost:2000/images/Orange.jpg"));
		StartCoroutine(GetItems("http://localhost:2000"));
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
		StartCoroutine(GetStreetViewImage(url));
	}

	private IEnumerator GetStreetViewImage(string url) {
		//現在地マーカーはここの「&markers」以下で編集可能
     	Debug.Log(url);

		WWW www = new WWW(url);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) { // ダウンロードでエラーが発生した
            Debug.Log(www.error);
        }
        rawimage.texture = www.texture;
 		rawimage.material.mainTexture = www.texture;
 		rawimage.color = new Color(rawimage.color.r, rawimage.color.g, rawimage.color.b, 1f);
 	}

	private IEnumerator GetItems(string url) {
		//現在地マーカーはここの「&markers」以下で編集可能
     	Debug.Log(url);
		WWW www = new WWW(url);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) { // ダウンロードでエラーが発生した
            Debug.Log(www.error);
        }
        Debug.Log(www.text);
 			}

}
