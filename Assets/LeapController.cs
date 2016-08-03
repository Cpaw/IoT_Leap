using UnityEngine;
using System.Collections;
using Leap;
public class LeapController : MonoBehaviour {
	Controller controller = new Controller();
	public int FingerCount;
void Start () {
}

void Update () {
	objectOperationWithLeapMotion();
}
void objectOperationWithLeapMotion(){

		Frame frame = controller.Frame();
    	FingerCount = frame.Fingers.Count;
		GestureList gestures = frame.Gestures();
		InteractionBox interactionBox = frame.InteractionBox;
		if(frame.Fingers[0].IsValid){
			for(int n=0; n<gestures.Count; n++){
				
				Gesture gesture =gestures[n];
				Debug.Log(gesture.Type);
				switch(gesture.Type){
					case Gesture.GestureType.TYPEKEYTAP:
						KeyTapGesture keytapGesture = new KeyTapGesture(gesture);
							Debug.Log("KeyTap");
					break;
					case Gesture.GestureType.TYPESCREENTAP:
						ScreenTapGesture screenTapGesture = new ScreenTapGesture(gesture);
						Debug.Log("ScreenTap");
					break;
					default:
					break;
				}
      		}
		}
}
}