using UnityEngine;
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
	public enum AbilityTypes {Flash, SpinCharge, LockOn, LaserBurst, WallConstruct, NumberOfTypes};
	private  delegate void AbilityDelegate();
	private static AbilityDelegate[] AbilityDelegates = {
		Flash, SpinCharge, LockOn, LaserBurst, WallConstruct};

	public Camera MainCamera;
	Vector2 PlayerPosition;
	Vector3 PlayerRotation;
	Vector2 PlayerSpeed;
	Vector2 PlayerPreviousSpeed;

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
	private Vector3 BulletLookAtLocation = Vector3.zero;
	private Vector3 OriginalRotation;

	[Header("Abilities")]
	public AbilityTypes SelectedAbility = AbilityTypes.Flash;
	//private static  AbilityDelegate SelectedAbilityDelegate; 
	private int SelectedAbilityInt;
	
	[Header("Flash Settings")]
	public float Flash_Cooldown;
	public float Flash_CooldownTimer;
	public float Flash_Duration;
	public float Flash_DurationTimer;
	public bool Flash_Earned;
	public bool Flash_Active;

	[Header("SpinCharge Settings")]
	public float SpinCharge_Cooldown;
	public float SpinCharge_CooldownTimer;
	public bool SpinCharge_Earned;
	public bool SpinCharge_Active;
	public Collider SpinCharge_AttackArea;
	public float SpinCharge_SteeringAngleMax;

	[Header("LockOn Settings")]
	public float LockOn_Cooldown;
	public float LockOn_CooldownTimer;
	public float LockOn_Duration;
	public float LockOn_DurationTimer;
	public bool LockOn_Earned;
	public bool LockOn_Active;
	public float LockOn_DamagePerShot;
	public float LockOn_NumberOfShots;

	[Header("LaserBurst Settings")]
	public float LaserBurst_Cooldown;
	public float LaserBurst_CooldownTimer;
	public float LaserBurst_Duration;
	public float LaserBurst_DurationTimer;
	public bool LaserBurst_Earned;
	public bool LaserBurst_Active;
	public float LaserBurst_Damage;
	//public bool LaserBurst_DamageModifiedByProximityToCenter;


	[Header("WallConstruct Settings")]
	public float WallConstruct_Cooldown;
	public float WallConstruct_CooldownTimer;
	public float WallConstruct_Duration;
	public float WallConstruct_DurationTimer;
	public bool WallConstruct_Earned;
	public bool WallConstruct_Active;
	public float WallConstruct_ShieldHealth;


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
		PlayerSelectAbility();
		PlayerFireAbility ();
	}
	void PlayerSelectAbility(){
		if(Input.GetButtonDown("LeftBumper")){
			SelectedAbilityInt --;
		}
		if(Input.GetButtonDown("RightBumper")){
			SelectedAbilityInt++; 
		}
		SelectedAbilityInt = SelectedAbilityInt<0?((int)AbilityTypes.NumberOfTypes-1):SelectedAbilityInt;
		SelectedAbilityInt = SelectedAbilityInt>=(int)AbilityTypes.NumberOfTypes?0:SelectedAbilityInt;
		
	}
	void PlayerFireAbility(){
	
		if ( Input.GetAxis ("RightTrigger") > .2f) {
			
			AbilityDelegates[SelectedAbilityInt]();
			WeaponTimer = WeaponCooldown;
			TempBullet = (GameObject)Instantiate(Resources.Load("Bullet"),this.transform.position, this.transform.rotation);
			//TempBullet.transform.parent = this.transform;
			TempBullet.GetComponent<MoveOnZ>().BulletSpeed = BulletSpeed;
			if(UseMouse){
				BulletLookAtLocation = MainCamera.ScreenToWorldPoint(Input.mousePosition);
				BulletLookAtLocation.z =this.transform.position.z;
				
				TempBullet.transform.LookAt(BulletLookAtLocation);
				//
				
				
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
		PlayerPosition.x += PlayerSpeed.x * PlayerSpeedModifierX * Time.deltaTime;
		PlayerPosition.y += PlayerSpeed.y * PlayerSpeedModifierY * Time.deltaTime;
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


	/**********************************
	 * Abilities
	 * Flash, SpinCharge, LockOn, LaserBurst, WallConstruct
	 * ********************************/
	static void Flash(){
		Debug.Log ("Flash" + Player.Instance.Flash_CooldownTimer.ToString());
		Player.Instance.Flash_CooldownTimer++;
	}
	static void SpinCharge(){
		Debug.Log ("SpinCharge");
	}
	static void LockOn(){
		Debug.Log ("LockOn");
	}
	static void LaserBurst(){
		Debug.Log ("LaserBurst");
	}
	static void WallConstruct(){
		Debug.Log ("WallConstruct");
	}
		
}
