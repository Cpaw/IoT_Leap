using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class PanelSlider : MonoBehaviour {
	
    // スライドイン（Pauseボタンが押されたときに、これを呼ぶ）
    public void SlideIn(){
    	 iTween.MoveTo(gameObject, iTween.Hash ("y", 200, "time", 1.5));
        Debug.Log("SLIDEIN");
    }
 
    // スライドアウト
    public void SlideOut(){
    	iTween.MoveTo(gameObject, iTween.Hash ("y", -400, "time", 1));
    }
 
}