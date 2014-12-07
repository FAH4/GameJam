using UnityEngine;
using System.Collections;


public enum EnemyTypes {SingleShooter, ChargeBeamers, MissileBattery, Mothership, LootShip}
public class EnemyManager : MonoBehaviour {
	private ArrayList ActiveEnemySquads = new ArrayList();
	private Vector2 OffScreenSpawnPoint = new Vector2(1.2f,.5f);
	private Vector3 ScreenPosition;
	public GameObject MainCamera;
	private static EnemyManager instance;
	private EnemyManager(){}
	

	public static EnemyManager Instance {
			get {
					if (instance == null) {
							
							instance = GameObject.Find("EnemyManager").GetComponent<EnemyManager> ();
								

					}
				return instance;
			}
	}


	private EnemySquad[] EnemySquads = new EnemySquad[100];
	private Vector3 EnemyLocation;
	private int EnemyCount;
	

	// Use this for initialization
	void Start () {
		//EnemyManager.Instance.SpawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(EnemySquad squad in ActiveEnemySquads){
			
			squad.Update();
		}
		
	}

	public void SpawnEnemySquad(int SquadNumber, EnemyTypes SquadEnemyType, int NumberOfEnemies, EnemyTypes EnemyTypeToSpawn){
		EnemySquads[SquadNumber] = new EnemySquad_Basic();
		EnemySquads[SquadNumber].InitializeSquad(NumberOfEnemies);
		ActiveEnemySquads.Add (EnemySquads[SquadNumber]);
		
		MoveEnemySquad(SquadNumber,OffScreenSpawnPoint, .000001f);
		Debug.Log(EnemyTypeToSpawn.ToString());
	}

	public void DestroyEnemy(){
		
	}
	
	public void MoveEnemySquad(int SquadNumber, Vector2 percent, float TimeToMove){
		ScreenPosition.x =percent.x * Screen.width;
		ScreenPosition.y =percent.y * Screen.height;
		ScreenPosition.z = 0;
		ScreenPosition = MainCamera.camera.ScreenToWorldPoint(ScreenPosition);
		ScreenPosition.z = 0;
		EnemySquads[SquadNumber].SetMovePosition(ScreenPosition,TimeToMove);
		
	}
	
	public void OrderVolley(int SquadNumber){
		EnemySquads[SquadNumber].Volley();
	}
}
