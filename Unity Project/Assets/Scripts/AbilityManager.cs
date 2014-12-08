using UnityEngine;
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
	public GameObject PlayArea;
	
	[Header("Flash Settings")]
	public float Flash_Cooldown;
	public float Flash_CooldownTimer;
	public float Flash_Duration;
	public float Flash_DurationTimer;
	public bool Flash_Earned;
	public bool Flash_Active;
	public float Flash_DistancePercent;
	
	
	[Header("SpinCharge Settings")]
	public float SpinCharge_Cooldown;
	public float SpinCharge_CooldownTimer;
	public float SpinCharge_Duration;
	public float SpinCharge_DurationTimer;
	public bool SpinCharge_Earned;
	public bool SpinCharge_Active;
	public Collider SpinCharge_AttackArea;
	public float SpinCharge_DistancePercent;
	public float SpinCharge_SpinsPerCall;
	private float SpinCharge_SpinAngle;
	//public float SpinCharge_SteeringAngleMax;
	
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
		UpdateAbilityEffects();
		PlayerFireAbility ();
	}
	void UpdateAbilityEffects(){
		UpdateFlashEffects();
		UpdateSpinChargeEffects();
	}
	void UpdateFlashEffects(){
		if(Flash_Earned){
			if(Flash_Active){
				if(Flash_DurationTimer >= Flash_Duration){
					Flash_DurationTimer = Flash_Duration;
					Flash_Active = false;
					Flash_CooldownTimer = 0;
					
					Vector3 FlashPositionOffset = Vector3.zero;
					if(Mathf.Abs(Input.GetAxis ("HorizontalRightStick")) < 0.1f && Mathf.Abs(Input.GetAxis ("VerticalRightStick")) < 0.1f) 
					{
						FlashPositionOffset.x = 1.0f * Flash_DistancePercent * PlayArea.transform.localScale.x;
						FlashPositionOffset.y = 0.0f;
					}
					else
					{
						FlashPositionOffset.x = Input.GetAxis ("HorizontalRightStick") * Flash_DistancePercent * PlayArea.transform.localScale.x;
						FlashPositionOffset.y = Input.GetAxis ("VerticalRightStick") * Flash_DistancePercent *  PlayArea.transform.localScale.x;
					}
					
					Player.Instance.SetPlayerOffset(FlashPositionOffset);
				}
				Flash_DurationTimer += Time.deltaTime;
				//Player.Instance.WeaponCooldown = .1f;
			}
			else{
				//Player.Instance.WeaponCooldown = .4f;
				Flash_CooldownTimer +=Time.deltaTime;
				Flash_CooldownTimer = Flash_CooldownTimer>Flash_Cooldown?Flash_Cooldown: Flash_CooldownTimer;
			}
		}	
	}
	
	void UpdateSpinChargeEffects(){
		if(SpinCharge_Earned){
			if(SpinCharge_Active){
				if(SpinCharge_DurationTimer >= SpinCharge_Duration){
					SpinCharge_DurationTimer = SpinCharge_Duration;
					SpinCharge_Active = false;
					SpinCharge_CooldownTimer = 0;
					
					
				}
				Vector3 SpinChargePositionOffset = Vector3.zero;
				
				SpinChargePositionOffset.x = Time.deltaTime/SpinCharge_Duration * SpinCharge_DistancePercent * PlayArea.transform.localScale.x ;
				SpinChargePositionOffset.y = 0.0f;
				
				
				
				
				Player.Instance.SetPlayerOffset(SpinChargePositionOffset);
				SpinCharge_DurationTimer += Time.deltaTime;
				SpinCharge_SpinAngle += SpinCharge_SpinsPerCall* Time.deltaTime/SpinCharge_Duration;
				Vector3 SpinRotation = new Vector3(SpinCharge_SpinAngle,0,0);
				Player.Instance.SetPlayerRotation(SpinRotation);
				Player.Instance.RotationAllowed = false;
				//Player.Instance.WeaponCooldown = .1f;
			}
			else{
				Player.Instance.RotationAllowed = true;
				//Player.Instance.WeaponCooldown = .4f;
				SpinCharge_CooldownTimer +=Time.deltaTime;
				SpinCharge_CooldownTimer = Flash_CooldownTimer>Flash_Cooldown?Flash_Cooldown: Flash_CooldownTimer;
			}
		}	
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
		
		if ( Input.GetAxis ("RightTrigger") > .2f || Input.GetKeyDown(KeyCode.Space)) {
			
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
		if(!AbilityManager.Instance.Flash_Active && AbilityManager.Instance.Flash_CooldownTimer >= AbilityManager.Instance.Flash_Cooldown){
			AbilityManager.Instance.Flash_Active = true;
			AbilityManager.Instance.Flash_DurationTimer = 0;
			Debug.Log ("Flash used");
		}
		
		/*
		ScreenPosition.x =percent.x * Screen.width;
		ScreenPosition.y =percent.y * Screen.height;
		ScreenPosition.z = 0;
		ScreenPosition = MainCamera.camera.ScreenToWorldPoint(ScreenPosition);
		ScreenPosition.z = 0;
		*/
		
		
	}
	static void SpinCharge(){
		if(!AbilityManager.Instance.SpinCharge_Active && AbilityManager.Instance.SpinCharge_CooldownTimer >= AbilityManager.Instance.SpinCharge_Cooldown){
			AbilityManager.Instance.SpinCharge_Active = true;
			AbilityManager.Instance.SpinCharge_DurationTimer =0;
			Debug.Log ("SpinCharge used");
		}
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
