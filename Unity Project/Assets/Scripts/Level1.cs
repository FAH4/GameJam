using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LevelManager.Instance.AddPatternToTimeStamps(1,1,2);
		
		LevelManager.Instance.StartGame();
		//LevelManager.Instance.AddPatternToTimeStamps(3,12,5);//
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
