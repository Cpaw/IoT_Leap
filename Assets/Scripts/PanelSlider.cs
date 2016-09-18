using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class PanelSlider : MonoBehaviour {
	[SerializeField]
	private int slideInPos;
    // スライドイン（Pauseボタンが押されたときに、これを呼ぶ）
    public void SlideIn(){
    	 iTween.MoveTo(gameObject, iTween.Hash ("y", slideInPos, "time", 1.5));
        Debug.Log("SLIDEIN");
    }
 
    // スライドアウト
    public void SlideOut(){
    	iTween.MoveTo(gameObject, iTween.Hash ("y", -400, "time", 1));
    }
 
}