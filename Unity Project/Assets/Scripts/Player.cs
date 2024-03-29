﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private static Player instance;
	private Player(){}
	
	
	public static Player Instance {
		get {
			if (instance == null) {
				
				
				instance = GameObject.Find("Player").GetComponent<Player>();
				
				
			}
			return instance;
		}
	}
	

	public GameObject ShipMesh;
	Vector2 PlayerPosition;
	Vector3 PlayerRotation;
	Vector2 PlayerSpeed;
	Vector2 PlayerPreviousSpeed;
	public bool RotationAllowed = true;

	[Header("Hitbox Settings")]
	public Collider PlayerHitBox;

	[Header("Rotation Setings")]
	public float PlayerXRotationAmount = 5;
	public float PlayerYRotationAmount = 30;
	[Space (10)]
	public bool RotateShipInZWithXMovement = true;
	public bool RotateShipInYWithXMovement = true;
	public bool RotateShipInXWithXMovement = true;
	public bool RotateShipInZWithYMovement = true;
	public bool RotateShipInYWithYMovement = true;
	public bool RotateShipInXWithYMovement = true;
	//public PlayerMovementCurveTypes PlayerMovementCurve = PlayerMovementCurveTypes.Log_Time;

	[Header("Movement Settings")]
	public float PlayerSpeedModifierX = 1;
	public float PlayerSpeedModifierY = 1;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithInput =.5f;
	[Range (0,1)]
	public float PlayerLogarithmicSmoothingWithoutInput =.5f;
	[Range (0,1)]
	public float ControllerDeadZone =.1f;
	
	[Header("Weapon Settings")]
	public bool UseMouse = false;
	public float WeaponCooldown;
	public float BulletSpeed;
	private float WeaponTimer;
	private GameObject TempBullet;

	private Vector3 OriginalRotation;

	

	//public float PlayerLinearTimeToReachSpeed;
	
	// Use this for initialization
	void Start () {
		
		PlayerRotation = this.transform.eulerAngles;
		OriginalRotation = PlayerRotation;
		//EnemyManager.Instance.SpawnEnemy ();
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
			TempBullet.transform.forward=Vector3.right;



		}
	}
	void PlayerWeapons(){
		WeaponTimer -= Time.deltaTime;
		PlayerFireStandard ();
	}
	void PlayerAbilities (){
		
	}
	
	
	void MoveShip(){
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > ControllerDeadZone || Mathf.Abs (Input.GetAxis ("Horizontal")) > ControllerDeadZone) {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, Input.GetAxis ("Horizontal"), PlayerLogarithmicSmoothingWithInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, Input.GetAxis ("Vertical"), PlayerLogarithmicSmoothingWithInput);

		} else {
			PlayerSpeed.x = Mathf.Lerp (PlayerPreviousSpeed.x, 0, PlayerLogarithmicSmoothingWithoutInput);
			PlayerSpeed.y = Mathf.Lerp (PlayerPreviousSpeed.y, 0, PlayerLogarithmicSmoothingWithoutInput);

		}
		PlayerPosition.x += PlayerSpeed.x * PlayerSpeedModifierX * Time.deltaTime;
		PlayerPosition.y += PlayerSpeed.y * PlayerSpeedModifierY * Time.deltaTime;
		RotateShip ();
	}
	void RotateShip(){
		if(RotationAllowed){
			if (RotateShipInZWithYMovement)	PlayerRotation.z = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.z;
			if (RotateShipInYWithYMovement)	PlayerRotation.y = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.y;
			if (RotateShipInXWithYMovement)	PlayerRotation.x = PlayerSpeed.y * PlayerYRotationAmount + OriginalRotation.x;
			if (RotateShipInZWithXMovement)	PlayerRotation.z = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.z;
			if (RotateShipInYWithXMovement)	PlayerRotation.y = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.y;
			if (RotateShipInXWithXMovement)	PlayerRotation.x = PlayerSpeed.x * PlayerXRotationAmount + OriginalRotation.x;
		}
	}
	void CheckInPlayArea(){
		if(PlayerPosition.x>.5f){
			AbilityManager.Instance.SpinCharge_Active = false;
			SetPlayerRotation(Vector3.zero);
			AbilityManager.Instance.SpinCharge_CooldownTimer = 0;
			RotationAllowed = true;
		}
		PlayerPosition.x = PlayerPosition.x < -.5f ? -.5f : PlayerPosition.x;
		PlayerPosition.x = PlayerPosition.x > .5f ? .5f : PlayerPosition.x;
		PlayerPosition.y = PlayerPosition.y < -.5f ? -.5f : PlayerPosition.y;
		PlayerPosition.y = PlayerPosition.y > .5f ? .5f : PlayerPosition.y;
	}
	
	public Vector3 GetPlayerPosition(){
		return PlayerPosition;
	}
	public void SetPlayerOffset(Vector3 FlashOffset){
	
		this.transform.position += FlashOffset;
		PlayerPosition = this.transform.position;
	}
	public Vector3 GetPlayerRotation(){
		return PlayerRotation;
	}
	public void SetPlayerRotation(Vector3 Rotation){
		
		PlayerRotation = Rotation;
		
	}


	
		
}
