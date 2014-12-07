using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
			LevelManager.Instance.AddPatternToTimeStamps(1,1,2);
				Parameters from left to right: Time, Pattern#, #ofEnemies
				Creates a new group of enemies at time and sends them on associated pattern
				
			LevelManager.Instance.StartGame();
				Must be the last call!!
				Reorganizes the timeline and starts the level
				*/
		LevelManager.Instance.AddPatternToTimeStamps(1,1,EnemyTypes.ChargeBeamers,EnemyTypes.LootShip);
		LevelManager.Instance.AddPatternToTimeStamps(1,1,EnemyTypes.SingleShooter);
		
		LevelManager.Instance.StartGame();
		//LevelManager.Instance.AddPatternToTimeStamps(3,12,5);//
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
