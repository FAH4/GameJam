using UnityEngine;
using System.Collections;

public enum ActionType {Spawn,Move, Fire};
public class LevelManager : MonoBehaviour {
	private static LevelManager instance;
	private LevelManager(){}
	private bool ReadyToStart = false;
	
	
	public static LevelManager Instance {
		get {
			if (instance == null) {
				
				
				instance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
				
				
			}
			return instance;
		}
	}
	
	public float TimeSpentOnLevel;
	public float LevelLength;
	
	private TimeStamp[] TimeStamps = new TimeStamp[1000];
	private int TimeStampIndex = 0;
	private int TimeStampPlacementIndex = 0;
	private int GroupPlacementIndex = -1;
	private float TimeStartOfOrders;
	private int NumberOfEnemiesToSpawn;
	private EnemyTypes CurrentPrimaryEnemyType;
	private EnemyTypes CurrentSecondaryEnemyType;
	

	// Use this for initialization
	void Start () {
		
		/*for(int i = 0; i<TimeStamps.Length;i++){
			TimeStamps[i] = new TimeStamp(1000f,ActionType.Spawn,100);
		}*/
	}
	
	// Update is called once per frame
	void Update () {//
		if(ReadyToStart){
		
		TimeSpentOnLevel += Time.deltaTime;
		CheckTimeStamp();
		}
		
	}
	private void CheckTimeStamp(){
		if(TimeStampIndex < TimeStampPlacementIndex && TimeSpentOnLevel> TimeStamps[ TimeStampIndex].SetOffTime){// && TimeStamps[ i].Active){
			TimeStamps[TimeStampIndex].Active = false;
			
			if(TimeStamps[ TimeStampIndex].ThisActionType ==ActionType.Spawn){
				EnemyManager.Instance.SpawnEnemySquad(TimeStamps[ TimeStampIndex].SquadNumber,EnemyTypes.SingleShooter, TimeStamps[TimeStampIndex].NumberOfEnemiesToSpawn, TimeStamps[TimeStampIndex].MyEnemyType);
			}
			else if(TimeStamps[ TimeStampIndex].ThisActionType ==ActionType.Move){
				EnemyManager.Instance.MoveEnemySquad(TimeStamps[ TimeStampIndex].SquadNumber,TimeStamps[TimeStampIndex].PercentPositionOnScreen,TimeStamps[TimeStampIndex].TimeToMove);
			}
			else if(TimeStamps[ TimeStampIndex].ThisActionType ==ActionType.Fire){
				EnemyManager.Instance.OrderVolley(TimeStamps[ TimeStampIndex].SquadNumber);
			}
			TimeStampIndex++;
			CheckTimeStamp();
		}
		
	}
	public void AddPatternToTimeStamps(float TimeStart, int PatternNumber, EnemyTypes _CurrenEnemyTypes){
		AddPatternToTimeStamps( TimeStart, PatternNumber,  1,  _CurrenEnemyTypes, _CurrenEnemyTypes);
	}
	public void AddPatternToTimeStamps(float TimeStart, int PatternNumber, EnemyTypes _CurrentPrimaryEnemyTypes, EnemyTypes _CurrentSecondaryEnemyTypes){
		AddPatternToTimeStamps( TimeStart, PatternNumber,  1,  _CurrentPrimaryEnemyTypes, _CurrentSecondaryEnemyTypes);
	}
	private void AddPatternToTimeStamps(float TimeStart,int PatternNumber,  int _NumberOfEnemiesToSpawn, EnemyTypes _CurrentPrimaryEnemyType, EnemyTypes _CurrentSecondaryEnemyType){
		if(!ReadyToStart){
			TimeStartOfOrders = TimeStart;
			NumberOfEnemiesToSpawn = _NumberOfEnemiesToSpawn;
			CurrentPrimaryEnemyType = _CurrentPrimaryEnemyType;	
			CurrentSecondaryEnemyType = _CurrentSecondaryEnemyType;
			/*
				SquadOrder_Spawn(0);
					Spawns a group of enemies at time offset 0
					You then place other orders following this command
					Once a new SquadOrder_Spawn is called all squads created previously will no longer be gettning new orders
					
				SquadOrder_Move(0,.75f,.25f,.5f);
					Moves the current squad to a location over time
					A new SquadOrder_Move will not blend movement, it will overwrite a previous one if the timing overlaps
					Parameters left to right: Time Offset, Percent of ScreenFromLeft, Percent of Screen from Bottom, Time To Move
				
				SquadOrder_Fire(.6f);
					Orders the current squad to fire at time offset
					
					*/
			switch(PatternNumber){
			case 0:
				
				SquadOrder_SpawnPrimary(0);
				SquadOrder_Move(0,.75f,.25f,.5f);
				SquadOrder_Fire(.6f);
				SquadOrder_Move(.75f,-.25f,.25f,2f);
				SquadOrder_Fire(2f);
				
				
				break;
			case 1:
				SquadOrder_SpawnPrimary(0);
				SquadOrder_Move(0,.75f,.25f,0f);//
				SquadOrder_Move(.01f,.75f,.75f,1f);
				SquadOrder_Fire(1.1f);
				SquadOrder_Move(2f,-.25f,.25f,2f);
				SquadOrder_Fire(2f);
				
				
				SquadOrder_SpawnSecondary(-1);
				SquadOrder_Move(-1,.75f,1.25f,0f);
				SquadOrder_Move(1.01f,.75f,.25f,1f);
				SquadOrder_Fire(2.1f);
				SquadOrder_Move(3f,-.25f,.25f,2f);
				SquadOrder_Fire(3f);
				break;
			
			case 2:
				SquadOrder_SpawnPrimary(-1);
				SquadOrder_Move(0.5f, 1f,.15f,0.0f);
				SquadOrder_Move(0.55f,.75f,.15f,1.0f);
				SquadOrder_Fire(1.75f);
				SquadOrder_Fire(1.85f);
				SquadOrder_Fire(1.95f);
				SquadOrder_Move(2.5f,-0.25f,.15f,3.0f);
				
				break;
				
			}
		}
	}
	public void StartGame(){
		ReorganizeTimeStamps();
		ReadyToStart = true;
	}
	private void ReorganizeTimeStamps(){
		bool ChangeMade = true;
		TimeStamp TempTimeStamp;
		while(ChangeMade){
			ChangeMade = false;
			for(int i = 1; i < TimeStampPlacementIndex; i++){
				if(TimeStamps[i].SetOffTime< TimeStamps[i-1].SetOffTime){
					TempTimeStamp = TimeStamps[i];
					TimeStamps[i] = TimeStamps[i-1];
					TimeStamps[i-1] = TempTimeStamp;
					ChangeMade = true;
				}
			}
		}
	}
	private void SquadOrder_SpawnPrimary(float TimeOffset){
		GroupPlacementIndex++;
		TimeStamps[TimeStampPlacementIndex] = new TimeStamp(TimeStartOfOrders + TimeOffset,ActionType.Spawn,GroupPlacementIndex,NumberOfEnemiesToSpawn, CurrentPrimaryEnemyType);
		TimeStampPlacementIndex ++;
	}
	private void SquadOrder_SpawnSecondary(float TimeOffset){
		GroupPlacementIndex++;
		TimeStamps[TimeStampPlacementIndex] = new TimeStamp(TimeStartOfOrders + TimeOffset,ActionType.Spawn,GroupPlacementIndex,NumberOfEnemiesToSpawn, CurrentSecondaryEnemyType);
		TimeStampPlacementIndex ++;
	}
	private void SquadOrder_Move(float TimeOffset,float XPercentOfScreen, float YPercentOfScreen, float TimeToMove){
		TimeStamps[TimeStampPlacementIndex] = new TimeStamp(TimeStartOfOrders + TimeOffset,ActionType.Move,GroupPlacementIndex, XPercentOfScreen,YPercentOfScreen, TimeToMove);
		TimeStampPlacementIndex++;
	}
	private void SquadOrder_Fire(float TimeOffset){
		TimeStamps[TimeStampPlacementIndex] = new TimeStamp(TimeStartOfOrders + TimeOffset,ActionType.Fire,GroupPlacementIndex);
		TimeStampPlacementIndex++;
	}
	
	public class TimeStamp{
		public float SetOffTime;
		public ActionType ThisActionType;
		public int SquadNumber;
		public Vector2 PercentPositionOnScreen = Vector2.zero;
		public float TimeToMove;
		public int NumberOfEnemiesToSpawn;
		public bool Active = true;
		public EnemyTypes MyEnemyType;
		
		public TimeStamp(float _SetOffTime,ActionType _ThisActionType, int _SquadNumber,float _ScreenWidth, float _ScreenHeight, float _TimeToMove){
			SetOffTime = _SetOffTime;
			ThisActionType = ActionType.Move;
			SquadNumber = _SquadNumber;
			PercentPositionOnScreen.x = _ScreenWidth;
			PercentPositionOnScreen.y = _ScreenHeight;
			TimeToMove = _TimeToMove;
			
			
		}
		public TimeStamp(float _SetOffTime, ActionType _ThisActionType, int _SquadNumber){
			SetOffTime = _SetOffTime;
			ThisActionType = _ThisActionType;
			SquadNumber = _SquadNumber;
		}
		public TimeStamp(float _SetOffTime, ActionType _ThisActionType, int _SquadNumber, int _NumberOfEnemiesToSpawn, EnemyTypes _MyEnemyType){
			SetOffTime = _SetOffTime;
			ThisActionType = _ThisActionType;
			SquadNumber = _SquadNumber;
			NumberOfEnemiesToSpawn = _NumberOfEnemiesToSpawn;
			MyEnemyType = _MyEnemyType;
		}
		public TimeStamp(){
			SetOffTime = 0;
			ThisActionType = ActionType.Spawn;
			SquadNumber = 0;
		}
		
		
		
	}
	
}
