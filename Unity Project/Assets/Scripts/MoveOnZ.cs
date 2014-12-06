using UnityEngine;
using System.Collections;

public class MoveOnZ : MonoBehaviour {
	public float BulletSpeed;
	private Vector3 myPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate (0, 0, BulletSpeed);
		myPosition = this.transform.position;
		myPosition.z = 0;
		this.transform.position = myPosition;
	}

	void OnCollisionEnter(Collision Col){
		Debug.Log (Col.transform.name);
		Destroy (this.gameObject);
		EnemyManager.Instance.DestroyEnemy ();
		EnemyManager.Instance.SpawnEnemy ();
	
	}
}
