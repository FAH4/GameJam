using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public enum PlayerMovementCurveTypes {Linear_Time, Log_Time, Exp_Time};
	public Camera MainCamera;
	Vector2 PlayerPosition;
	Vector2 PlayerSpeed;
	Vector2 PlayerPreviousSpeed;
	public float PlayerSpeedModifier = 1;

	//public PlayerMovementCurveTypes PlayerMovementCurve = PlayerMovementCurveTypes.Log_Time;
	
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithInput =.5f;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithoutInput =.5f;
	[Range (0,1)]
	public float ControllerDeadZone =.1f;
	
	
	public bool FollowMouse = false;
	public float WeaponCooldown;
	public float BulletSpeed;
	private float WeaponTimer;
	private GameObject TempBullet;
	private Vector3 MousePosition;
	
	//public float PlayerLinearTimeToReachSpeed;
	
	// Use this for initialization
	void Start () {
		EnemyManager.Instance.SpawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMovement ();
		PlayerFire ();
	}
	void PlayerMovement(){
		PlayerPosition = this.transform.localPosition;
		MoveShip ();
		CheckInPlayArea ();
		this.transform.localPosition = PlayerPosition;
		PlayerPreviousSpeed = PlayerSpeed;
	}
	void PlayerFire(){
		WeaponTimer -= Time.deltaTime;
		if (WeaponTimer <= 0 && Input.GetButton ("Fire1")) {
			WeaponTimer = WeaponCooldown;
			TempBullet = (GameObject)Instantiate(Resources.Load("Bullet"),this.transform.position, this.transform.rotation);
			//TempBullet.transform.parent = this.transform;
			TempBullet.GetComponent<MoveOnZ>().BulletSpeed = BulletSpeed;
			TempBullet.transform.forward=(this.transform.right);

			if(FollowMouse){
				MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
				MousePosition.z =this.transform.position.z;

				TempBullet.transform.LookAt(MousePosition - this.transform.position);

				Debug.Log(MousePosition);
			}
		}
	}
	
	void MoveShip(){
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > ControllerDeadZone || Mathf.Abs (Input.GetAxis ("Horizontal")) > ControllerDeadZone) {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, Input.GetAxis ("Horizontal"), PlayerLogarithmicSmoothingWithInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, Input.GetAxis ("Vertical"), PlayerLogarithmicSmoothingWithInput);

		} else {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, 0, PlayerLogarithmicSmoothingWithoutInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, 0, PlayerLogarithmicSmoothingWithoutInput);

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
