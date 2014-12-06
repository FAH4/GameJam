using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager instance;
	private EnemyManager(){}
	private int EnemyCount;

	public static EnemyManager Instance {
			get {
					if (instance == null) {
							GameObject go = new GameObject ();
							go.name = "EnemyManager";
							instance = go.AddComponent<EnemyManager> ();
							go.transform.parent = GameObject.Find("PlayArea").transform;
							go.transform.localScale = new Vector3(1,1,1);

					}
				return instance;
			}
	}


	private GameObject MyEnemy;
	private Vector3 EnemyLocation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnEnemy(){
		MyEnemy = (GameObject)Instantiate (Resources.Load ("Enemy"));
		MyEnemy.transform.parent = this.transform;
		EnemyLocation.x = Random.Range (-.5f, .5f);
		EnemyLocation.y = Random.Range (-.5f, .5f);
		EnemyLocation.z = 0;
		MyEnemy.transform.localPosition = EnemyLocation;

		EnemyCount++;
		MyEnemy.name = "Enemy" + EnemyCount;


	}

	public void DestroyEnemy(){
		Destroy (MyEnemy);
	}
}
