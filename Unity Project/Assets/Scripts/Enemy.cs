using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {
	private GameObject MyGameObject;
	private Vector3 MyPosition;
	public EnemySquad MySquad;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.Lerp(this.transform.position, MyPosition, .5f);
	}
	public void InitializeEnemy(){
		MyGameObject = (GameObject)Instantiate (Resources.Load ("Enemy"));
		MyGameObject.transform.parent = this.transform;
		MyPosition.x = .7f;
		MyPosition.y = 0;
		MyPosition.z = 0;
		MyGameObject.transform.localPosition = MyPosition;
	}
	public void Fire(){}
	public void Ability(){}
	public void Explode(){
		
		Destroy(this.transform);
	}
	public void MoveTo(Vector3 position){
		MyPosition = position;
	}
}
