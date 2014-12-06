using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public enum PlayerMovementCurveTypes {Linear_Time, Log_Time, Exp_Time};
	Vector2 PlayerPosition;
	Vector2 PlayerSpeed;
	Vector2 PlayerPreviousSpeed;
	public float PlayerSpeedModifier = 1;
	public bool ISControllerDead;
	//public PlayerMovementCurveTypes PlayerMovementCurve = PlayerMovementCurveTypes.Log_Time;

	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithInput =.5f;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithoutInput =.5f;
	[Range (0,1)]
	public float ControllerDeadZone =.1f;

	//public float PlayerLinearTimeToReachSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PlayerPosition = this.transform.localPosition;

		MoveShip ();

		CheckInPlayArea ();
		this.transform.localPosition = PlayerPosition;
		PlayerPreviousSpeed = PlayerSpeed;
	}

	void MoveShip(){
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > ControllerDeadZone || Mathf.Abs (Input.GetAxis ("Horizontal")) > ControllerDeadZone) {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, Input.GetAxis ("Horizontal"), PlayerLogarithmicSmoothingWithInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, Input.GetAxis ("Vertical"), PlayerLogarithmicSmoothingWithInput);
			ISControllerDead = true;
		} else {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, 0, PlayerLogarithmicSmoothingWithoutInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, 0, PlayerLogarithmicSmoothingWithoutInput);
			ISControllerDead = false;
		}
		PlayerPosition.x += PlayerSpeed.x * PlayerSpeedModifier * Time.deltaTime;
		PlayerPosition.y += PlayerSpeed.y * PlayerSpeedModifier * Time.deltaTime;
	}
	void CheckInPlayArea(){
		PlayerPosition.x = PlayerPosition.x < -.5f ? -.5f : PlayerPosition.x;
		PlayerPosition.x = PlayerPosition.x > .5f ? .5f : PlayerPosition.x;
		PlayerPosition.y = PlayerPosition.y < -.5f ? -.5f : PlayerPosition.y;
		PlayerPosition.y = PlayerPosition.y > .5f ? .5f : PlayerPosition.y;
	}
}
