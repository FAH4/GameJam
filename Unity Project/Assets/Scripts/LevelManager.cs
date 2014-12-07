using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float TimeSpentOnLevel;
	public float LevelLength;
	private bool Time15 = false;
	private bool Time10 = false;
	private bool Time5 = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TimeSpentOnLevel += Time.deltaTime;
		if(TimeSpentOnLevel> LevelLength){
			
		}
		if(TimeSpentOnLevel>15){
			if(!Time15){
				Time15 = true;
				EnemyManager.Instance.MoveEnemySquad();
			}
		}
		if(TimeSpentOnLevel>10){
			if(!Time10){
				Time10 = true;
				EnemyManager.Instance.MoveEnemySquad();
			}
		}
		if(TimeSpentOnLevel>5){
			if(!Time5){
				Time5 = true;
				EnemyManager.Instance.SpawnEnemySquad();
			}
		}
		
	}
}
