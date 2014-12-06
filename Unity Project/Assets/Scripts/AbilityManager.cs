﻿using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {
	private static AbilityManager instance;
	private AbilityManager(){}
	
	
	public static AbilityManager Instance {
		get {
			if (instance == null) {
				
				
				instance = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
				
				
			}
			return instance;
		}
	}
	
	public enum AbilityTypes {Flash, SpinCharge, LockOn, LaserBurst, WallConstruct, NumberOfTypes};
	private  delegate void AbilityDelegate();
	private static AbilityDelegate[] AbilityDelegates = {
		Flash, SpinCharge, LockOn, LaserBurst, WallConstruct};
	
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
	
	void Start(){
		SelectedAbilityInt = (int)SelectedAbility;
	}
	void Update () {
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
		
		SelectedAbility = (AbilityTypes)SelectedAbilityInt;
		
	}
	void PlayerFireAbility(){
		
		if ( Input.GetAxis ("RightTrigger") > .2f) {
			
			AbilityDelegates[SelectedAbilityInt]();
			/*WeaponTimer = WeaponCooldown;
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
			*/
		}
	}
	
	/**********************************
	 * Abilities
	 * Flash, SpinCharge, LockOn, LaserBurst, WallConstruct
	 * ********************************/
	static void Flash(){
		Debug.Log ("Flash" + AbilityManager.Instance.Flash_CooldownTimer.ToString());
		AbilityManager.Instance.Flash_CooldownTimer++;
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
