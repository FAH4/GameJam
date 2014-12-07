using UnityEngine;
using System.Collections;

public abstract class Enemy : Object {
	private GameObject MyGameObject;
	private Vector3 MyPosition;
	public EnemySquad MySquad;
	public float Timer;
	public float TimeToMove;
	public bool Alive = true;
	private bool Active = false;
	private Vector3 StartOfMove;//
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
		if(Active){
			MyGameObject.transform.position = Vector3.Lerp(StartOfMove, MyPosition, Timer/TimeToMove);
			Timer += Time.deltaTime;
			Timer = Timer>TimeToMove? TimeToMove: Timer;
		}
	}
	public void InitializeEnemy(EnemySquad _MySquad, EnemyTypes EnemyTypeToSpawn){
	
		switch(EnemyTypeToSpawn){
			case EnemyTypes.ChargeBeamers:
			MyGameObject = (GameObject)Instantiate (Resources.Load ("ChargeBeamers"));
			break;
			case EnemyTypes.LootShip:
			MyGameObject = (GameObject)Instantiate (Resources.Load ("LootShip"));
			break;
			case EnemyTypes.MissileBattery:
			MyGameObject = (GameObject)Instantiate (Resources.Load ("MissileBattery"));
			break;
			case EnemyTypes.Mothership:
			MyGameObject = (GameObject)Instantiate (Resources.Load ("Mothership"));
			break;
			case EnemyTypes.SingleShooter:
			MyGameObject = (GameObject)Instantiate (Resources.Load ("SingleShooter"));
			break;
			
		}
		

		MyGameObject.transform.Translate(10f,0,0);
		MoveTo(MyGameObject.transform.position,1);
		MyGameObject.GetComponent<EnemyScript>().MyEnemyObject = this;
		MySquad = _MySquad;
		Active = true;
		
		
	}
	public void Fire(){
		GameObject tempGO = (GameObject) Instantiate(Resources.Load("EnemyBullet"));
		tempGO.transform.position = MyGameObject.transform.position;
		tempGO.transform.Translate( - .5f,0,0);
		tempGO.transform.Rotate(0,-90,0);
		tempGO.GetComponent<MoveOnZ>().BulletSpeed = .05f;
		
	}
	public void Ability(){}
	public void Explode(){
		Destroy(MyGameObject);
		Alive = false;
	}
	public void MoveTo(Vector3 position, float OverTime){
		Timer = 0;
		TimeToMove = OverTime;
		if(TimeToMove ==0){
			Timer = 1;
			TimeToMove = 1;
		}
		MyPosition = position;
		StartOfMove = MyGameObject.transform.position;//
	}
}
