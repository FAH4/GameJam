using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager instance;
	private EnemyManager(){}
	

	public static EnemyManager Instance {
			get {
					if (instance == null) {
							
							instance = GameObject.Find("EnemyManager").AddComponent<EnemyManager> ();
								

					}
				return instance;
			}
	}


	private GameObject MyEnemy;
	private Vector3 EnemyLocation;
	private int EnemyCount;
	

	// Use this for initialization
	void Start () {
		//EnemyManager.Instance.SpawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnEnemySquad(){
		


	}

	public void DestroyEnemy(){
		
	}
}
