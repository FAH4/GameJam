using UnityEngine;
using System.Collections;

public abstract class EnemySquad : MonoBehaviour {
	private Enemy[] SquadEnemies;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void InitializeSquad(){
		SquadEnemies = new Enemy[3];
		for(int i = 0; i < SquadEnemies.Length;i++){
			SquadEnemies[i].InitializeEnemy();
		}
	}
	void DestroySquad(){
		if(SquadEnemies.Length>0){
			for(int i = 0; i < SquadEnemies.Length;i++){
				SquadEnemies[i].Explode();
			}
		}
	}
}
