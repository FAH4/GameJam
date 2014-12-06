using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public float TimeSpentOnLevel;
	public float LevelLength;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TimeSpentOnLevel += Time.deltaTime;
		if(TimeSpentOnLevel> LevelLength){
		}
	}
}
