using UnityEngine;
using System.Collections;

public abstract class EnemySquad : Object {
	private Enemy[] SquadEnemies;
	private float Spacing = .3f;
	private Vector3 TempEnemyPostion;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is ca//lled once per frame
	public void Update () {
		
		for(int i = 0; i < SquadEnemies.Length;i++){
			if(SquadEnemies[i].Alive){
				SquadEnemies[i].Update();
			}
		}
	}
	
	public void InitializeSquad(int EnemiesInSquad, EnemyTypes EnemyTypeToSpawn){
		SquadEnemies = new Enemy[EnemiesInSquad];
		for(int i = 0; i < SquadEnemies.Length;i++){
			//if(SquadEnemies[i].Alive){
				SquadEnemies[i] = new Enemy_Basic();
				SquadEnemies[i].InitializeEnemy(this,EnemyTypeToSpawn);//
			//}
		}
	}
	public void DestroySquad(){
		if(SquadEnemies.Length>0){
			
			for(int i = 0; i < SquadEnemies.Length;i++){
				if(SquadEnemies[i].Alive){
					SquadEnemies[i].Explode();
				}
			}
		}
	}
	
	public void SetMovePosition(Vector3 position, float TimeToMove){
		
		for(int i = 0; i < SquadEnemies.Length;i++){//
			if(SquadEnemies[i].Alive){
				TempEnemyPostion = position;
				TempEnemyPostion.y += (i - Mathf.FloorToInt(SquadEnemies.Length/2f))*Spacing;
				
				SquadEnemies[i].MoveTo(TempEnemyPostion,TimeToMove);//
			}
		}
	}
	public void Volley(){
		for(int i = 0; i < SquadEnemies.Length;i++){
			if(SquadEnemies[i].Alive){
				SquadEnemies[i].Fire();
			}
		}
	}
}
