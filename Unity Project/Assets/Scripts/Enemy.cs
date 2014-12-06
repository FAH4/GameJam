using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {
	private GameObject MyGameObject;
	private Vector3 MyPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void InitializeEnemy(){
		MyGameObject = (GameObject)Instantiate (Resources.Load ("Enemy"));
		MyGameObject.transform.parent = this.transform;
		MyPosition.x = Random.Range (-.5f, .5f);
		MyPosition.y = Random.Range (-.5f, .5f);
		MyPosition.z = 0;
		MyGameObject.transform.localPosition = MyPosition;
	}
	public void Fire(){}
	public void Ability(){}
	public void Explode(){
		Destroy(this.transform);
	}
	public void MoveTo(Vector3 position){}
}
