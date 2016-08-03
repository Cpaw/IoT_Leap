using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Leap;
using System;
using WebSocketSharp;
using WebSocketSharp.Net;

public class HandGesture : MonoBehaviour {
    public Text label;  // 表示用ラベル
    public string WSAddress = "ws://127.0.0.1:3000";
    private Controller controller = new Controller();   // ジェスチャー検知に必要
    public GameObject[] FingerObjects;
  	WebSocket ws;
  	float swipeStart=0;
    float swipeStop=0;
  	
    void Start() {
        // ジェスチャー有効化
        controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
        controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
        controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        Connect ();
    }

    void OnApplicationQuit () {
        Disconnect ();
    }

    void Connect () {

        ws = new WebSocket (WSAddress);

        ws.OnOpen += (sender, e) => {
            Debug.Log ("WebSocket Open");
        };

        ws.OnMessage += (sender, e) => {
            Debug.Log ("WebSocket Message Type: " + e.Type + ", Data: " + e.Data);
        };

        ws.OnError += (sender, e) => {
            Debug.Log ("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) => {
            Debug.Log ("WebSocket Close");
        };

        ws.Connect ();

    }

    void Disconnect () {
        ws.Close ();
        ws = null;
    }

    void Send (string message) {
        ws.Send (message);
    }
 
    void Update() {
        var frame = controller.Frame();
        var fingerCount = frame.Fingers.Count;
        var gestures = frame.Gestures();
        var interactionBox = frame.InteractionBox;

 		
        if ( frame.Fingers[0].IsValid ) {
        	 for ( int i = 0; i < FingerObjects.Length; i++ ) {
      var leapFinger = frame.Fingers[i];
      var unityFinger = FingerObjects[i];
      SetVisible( unityFinger, leapFinger.IsValid );
      if ( leapFinger.IsValid ) {
        Vector normalizedPosition = interactionBox.NormalizePoint(leapFinger.TipPosition );
        normalizedPosition *= 10;
        normalizedPosition.z = -normalizedPosition.z;
        unityFinger.transform.localPosition = ToVector3( normalizedPosition );
      }
    }
            for ( int i = 0 ; i < gestures.Count ; i++ ) {
                // ジェスチャー結果取得＆表示
                Gesture gesture = gestures[i];
                switch ( gesture.Type ) {
                case Gesture.GestureType.TYPECIRCLE:
                    var circle = new CircleGesture(gesture);
						// 回転方向を計算
				        string clockwiseness;
				        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
				        {
				          // 角度が90度以下なら、時計回り
				          clockwiseness = "時計回り";
				        }
				        else
				        {
				          clockwiseness = "反時計回り";
				        }
                        Debug.Log(clockwiseness);
				        Send(clockwiseness);
                    break;
                case Gesture.GestureType.TYPEKEYTAP:
                    var keytapGesture = new KeyTapGesture(gesture);
                    printGesture("KeyTap");
                    break;
                case Gesture.GestureType.TYPESCREENTAP:
                    var screenTapGesture = new ScreenTapGesture(gesture);
                    printGesture("ScreenTap");
                    break;
		      	case Gesture.GestureType.TYPESWIPE:
		      	var swipe = new SwipeGesture(gesture);
		      	if(swipe.State == Gesture.GestureState.STATESTART){
		      		swipeStart=swipe.Direction.y;
		      	}
		      	if(swipe.State == Gesture.GestureState.STATESTOP){
		      		swipeStop=swipe.Direction.y;
		      	}
		        	Debug.Log(swipe.Direction.y);
		        break;
                default:
                    break;
                }
            }
        }
    }
 
    // ジェスチャー結果表示
    void printGesture(string str) {
        if ( label != null ) {
            label.text = str;
        } else {
            Debug.Log(str);
        }
    }
      void SetVisible( GameObject obj, bool visible )
  {
    foreach( Renderer component in obj.GetComponents<Renderer>() ) {
      component.enabled = visible;
    }
  }
 
  Vector3 ToVector3( Vector v )
  {
    return new UnityEngine.Vector3( v.x, v.y, v.z );
  }
}