using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public enum PlayerMovementCurveTypes {Linear_Time, Log_Time, Exp_Time};
	public Camera MainCamera;
	Vector2 PlayerPosition;
	Vector3 PlayerRotation;
	Vector2 PlayerSpeed;
	Vector2 PlayerPreviousSpeed;

	[Header("Rotation Setings")]
	public float PlayerXRotationAmount = 5;
	public float PlayerYRotationAmount = 30;
	public bool RotateShipInZWithXMovement = true;
	public bool RotateShipInYWithXMovement = true;
	public bool RotateShipInXWithXMovement = true;
	public bool RotateShipInZWithYMovement = true;
	public bool RotateShipInYWithYMovement = true;
	public bool RotateShipInXWithYMovement = true;
	//public PlayerMovementCurveTypes PlayerMovementCurve = PlayerMovementCurveTypes.Log_Time;

	[Header("Movement Settings")]
	public float PlayerSpeedModifier = 1;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithInput =.5f;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithoutInput =.5f;
	[Range (0,1)]
	public float ControllerDeadZone =.1f;
	
	[Header("Weapon Settings")]
	public bool FollowMouse = false;
	public bool RightAnaloStick = true;
	public float WeaponCooldown;
	public float BulletSpeed;
	private float WeaponTimer;
	private GameObject TempBullet;
	private Vector3 BulletLookAtLocation;
	private Vector3 OriginalRotation;
	
	//public float PlayerLinearTimeToReachSpeed;
	
	// Use this for initialization
	void Start () {
		EnemyManager.Instance.SpawnEnemy ();
		PlayerRotation = this.transform.eulerAngles;
		OriginalRotation = PlayerRotation;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMovement ();
		PlayerWeapons ();
		PlayerAbilities ();
		PlayerFireStandard ();
	}
	void PlayerMovement(){
		PlayerPosition = this.transform.localPosition;

		MoveShip ();
		CheckInPlayArea ();
		this.transform.localPosition = PlayerPosition;
		this.transform.eulerAngles = PlayerRotation;
		PlayerPreviousSpeed = PlayerSpeed;
	}
	void PlayerFireStandard(){

		if (WeaponTimer <= 0 && (Input.GetButton ("Fire1"))) {
			WeaponTimer = WeaponCooldown;
			TempBullet = (GameObject)Instantiate(Resources.Load("Bullet"),this.transform.position, this.transform.rotation);
			//TempBullet.transform.parent = this.transform;
			TempBullet.GetComponent<MoveOnZ>().BulletSpeed = BulletSpeed;
			TempBullet.transform.forward=(this.transform.right);



		}
	}
	void PlayerWeapons(){
		WeaponTimer -= Time.deltaTime;
		PlayerFireStandard ();
	}
	void PlayerAbilities (){
		PlayerFireAbility ();
	}
	void PlayerFireAbility(){
		if (WeaponCooldown < 0 && Input.GetAxis ("Fire1RightTrigger") > .2f) {
			if(FollowMouse){
				BulletLookAtLocation = MainCamera.ScreenToWorldPoint(Input.mousePosition);
				BulletLookAtLocation.z =this.transform.position.z;
				
				TempBullet.transform.LookAt(BulletLookAtLocation);
				
				
			}
			else {
				BulletLookAtLocation.x = this.transform.position.x + Input.GetAxis ("HorizontalRightStick");
				BulletLookAtLocation.y = this.transform.position.y + Input.GetAxis("VerticalRightStick");
				BulletLookAtLocation.z = 0;
				
				TempBullet.transform.LookAt(BulletLookAtLocation);
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
		RotateShip ();
	}
	void RotateShip(){
		if (RotateShipInZWithYMovement)	PlayerRotation.z = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.z;
		if (RotateShipInYWithYMovement)	PlayerRotation.y = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.y;
		if (RotateShipInXWithYMovement)	PlayerRotation.x = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.x;
		if (RotateShipInZWithXMovement)	PlayerRotation.z = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.z;
		if (RotateShipInYWithXMovement)	PlayerRotation.y = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.y;
		if (RotateShipInXWithXMovement)	PlayerRotation.x = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.x;
	}
	void CheckInPlayArea(){
		PlayerPosition.x = PlayerPosition.x < -.5f ? -.5f : PlayerPosition.x;
		PlayerPosition.x = PlayerPosition.x > .5f ? .5f : PlayerPosition.x;
		PlayerPosition.y = PlayerPosition.y < -.5f ? -.5f : PlayerPosition.y;
		PlayerPosition.y = PlayerPosition.y > .5f ? .5f : PlayerPosition.y;
	}
}
