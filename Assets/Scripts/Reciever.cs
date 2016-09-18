using UnityEngine;

//RecieveInterfaceを実装
public class Reciever : MonoBehaviour, RecieveInterface{

  //privateはダメ
  public void OnRecieve (){
    Debug.Log("受け取った！");
  }

}