using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //必須
using Leap;
using System;
using WebSocketSharp;
using WebSocketSharp.Net;

public class HandControll : MonoBehaviour {
    public Text label;  // 表示用ラベル
    public string WSAddress = "ws://127.0.0.1:3000";
    public string WSAddress_DB = "";
    private Controller controller = new Controller();   // ジェスチャー検知に必要
    public GameObject[] FingerObjects;
  	WebSocket ws;
  	WebSocket ws_DB;
    public GameObject UIPanel;
    public GameObject[] UIItems;
    public ItemController[] ItemControllers;
    public string[] ItemNames;
    public int[] ItemPrices;
    public int[] ItemValues;
    bool UIOn;
    PanelSlider panel;
    bool UIOpened=false;
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
  	
    void Start() {
        // ジェスチャー有効化
        controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
        controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
        controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        Connect ();
        panel = UIPanel.GetComponent<PanelSlider>();
    }

    void OnApplicationQuit () {
        Disconnect ();
    }

    void Connect () {
        ws = new WebSocket (WSAddress);
        ws_DB = new WebSocket(WSAddress);

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

        ws_DB.OnOpen += (sender, e) => {
            Debug.Log ("WebSocket Open");
        };
        ws_DB.OnMessage += (sender, e) => {
            Debug.Log ("WebSocket Message Type: " + e.Type + ", Data: " + e.Data);
            GetItem(e.Data);
        };
        ws_DB.OnError += (sender, e) => {
            Debug.Log ("WebSocket Error Message: " + e.Message);
        };
        ws_DB.OnClose += (sender, e) => {
            Debug.Log ("WebSocket Close");
        };
        ws_DB.Connect ();
        ws_DB.Send("");
    }

    void Disconnect () {
        ws.Close ();
        ws = null;
        ws_DB.Close ();
        ws_DB = null;
    }

    void Send (string message) {
        ws.Send (message);
    }
    void Send_DB(string message){
    	ws_DB.Send(message);
    }
 
    void Update() {
        var frame = controller.Frame();
        var fingerCount = frame.Fingers.Count;
        var gestures = frame.Gestures();
        var interactionBox = frame.InteractionBox;
        if(UIOn){
            for ( int i = 0; i < UIItems.Length; i++ ) {
                ItemControllers[i].SetItem(ItemNames[i],ItemPrices[i],ItemValues[i]);
            }
        }
        if ( frame.Fingers[0].IsValid ) {
        	for ( int i = 0; i < FingerObjects.Length; i++ ) {
                var leapFinger = frame.Fingers[i];
                var unityFinger = FingerObjects[i];
      // if(leapFinger.Type == Leap.Finger){
      //    var indexFinger = FingerObjects[i];
      //    SetVisible( indexFinger, leapFinger.IsValid );
      // }
                //Debug.Log(leapFinger.TipPosition);
                //Debug.Log(frame.Hands[0]);
                if ( leapFinger.IsValid ) {
                    Vector normalizedPosition = interactionBox.NormalizePoint(leapFinger.TipPosition);
                    // Debug.Log(normalizedPosition);
                    normalizedPosition *= 10;
                    unityFinger.transform.localPosition = ToVector3( normalizedPosition );
                }
            }
        }else{
            Vector normalizedPosition = interactionBox.NormalizePoint(new Vector(0.5f,0.5f,0.5f));
            // Debug.Log(normalizedPosition);
            normalizedPosition.y+=0.35f;
            normalizedPosition *= 10;
            FingerObjects[0].transform.localPosition = ToVector3( normalizedPosition );
            FingerObjects[0].transform.localPosition = ToVector3( normalizedPosition );
        }

                // ジェスチャー結果取得＆表示
                Gesture gesture = gestures[0];
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
                    if(circle.State == Gesture.GestureState.STATESTOP){
                        //Debug.Log("stop");
                        if(clockwiseness=="時計回り"){
                                Send_DB("");
                                Debug.Log("UIOpen");
                                UIOpened=true;
                                panel.SlideIn();
                        }else{
                                UIOpened=false;
                                panel.SlideOut();
                        }

                }
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
                    Debug.Log(swipe.Direction.y);
                break;
                default:
                    break;

            }
    }
    void OnTriggerStay(Collider col){
        if(UIOpened==false){
        Debug.Log(col.gameObject.tag);
        }
        Send(col.gameObject.tag);
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

  void GetItem(string data){
        var textAsset = data;
        string itemJson = textAsset;
        var items = JsonUtility.FromJson<Items>(itemJson);
        for(int i = 0;i<items.itemList.Length;i++){
        Debug.Log ("name: " + items.itemList[i].name+" price: "+items.itemList[i].price+"value: "+items.itemList[i].value);
        ItemNames[i] = items.itemList[i].name;
        ItemPrices[i] = items.itemList[i].price;
        ItemValues[i] = items.itemList[i].value;
        UIOn=true;
        ItemControllers[0].test(WSAddress_DB+"/images/Orange.jpg");
        //ItemControllers[i].SetItem(items.itemList[i].name,items.itemList[i].price,items.itemList[i].value);
        //ExecuteEvents.Executeを使ってメソッド実行
        }
  }
}