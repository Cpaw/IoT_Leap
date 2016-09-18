using UnityEngine.EventSystems; //必須

//IEventSystemHandlerを継承させる
public interface RecieveInterface : IEventSystemHandler {
  void OnRecieve();//受け取るようのメソッドを定義
}