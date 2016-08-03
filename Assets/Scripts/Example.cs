using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;

public class Example : MonoBehaviour {

    public string WSAddress = "ws://127.0.0.1:3000";

    WebSocket ws;

    void Start () {

        Connect ();

    }

    void Update () {

        if (Input.GetKeyUp (KeyCode.Space)) {
            Send ("Test Message");
        }

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

}