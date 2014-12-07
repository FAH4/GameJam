using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
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
	
	}

	public void SpawnEnemySquad(){
		EnemySquads[0] = new EnemySquad_Basic();
		EnemySquads[0].InitializeSquad();
		EnemySquads[0].SetMovePosition(Player.Instance.transform.position);


	}

	public void DestroyEnemy(){
		
	}
	
	public void MoveEnemySquad(){
		EnemySquads[0].SetMovePosition(Player.Instance.transform.position);
	}
}
