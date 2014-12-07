using UnityEngine;
using System.Collections;

public abstract class Enemy : Object {
	private GameObject MyGameObject;
	private Vector3 MyPosition;
	public EnemySquad MySquad;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		MyGameObject.transform.position = Vector3.Lerp(MyGameObject.transform.position, MyPosition, .5f);
	}
	public void InitializeEnemy(){
		MyGameObject = (GameObject)Instantiate (Resources.Load ("Enemy"));
		MyPosition.x = .7f;
		MyPosition.y = 0;
		MyPosition.z = 0;
		MyGameObject.transform.localPosition = MyPosition;
	}
	public void Fire(){}
	public void Ability(){}
	public void Explode(){
		
		Destroy(MyGameObject.transform);
	}
	public void MoveTo(Vector3 position){
		MyPosition = position;
	}
}
