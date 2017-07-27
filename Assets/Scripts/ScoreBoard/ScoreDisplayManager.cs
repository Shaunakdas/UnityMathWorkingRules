using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayManager : MonoBehaviour {
	public GameObject ScoreCounterGO, LiveCounterGO, TimerGO;
	//Display Values
	public float currentScore = 0f;
	public float timePending = 0f;
	public int livesPending = 0;
	public void updateScore(int scoreDelta){
		//Add/Subtract Score
	}

	public void updateTimer(int timeDelta){
		//Add/Subtract Timer Value
	}

	public void updateLives(int liveDelta){

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
