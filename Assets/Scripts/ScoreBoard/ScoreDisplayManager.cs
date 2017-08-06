﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayManager : MonoBehaviour {
	public GameObject ScoreCounterGO, LiveCounterGO, TimerGO, LeftTableGO, RightTableGO;
	public List<GameObject> liveGOList;
	public GameObject livePF;
	//Display Values
	public float currentScore = 0f;
	public float timePending = 0f;
	public int livesPending = 0;
	void repositionTable(GameObject _tableGO){
		BasicGOOperation.CheckAndRepositionTable (_tableGO);
	}
	public void updateScore(float scoreDelta){
		//Add/Subtract Score
		currentScore += scoreDelta;
		ScoreCounterGO.GetComponent<UILabel> ().text =((int)currentScore).ToString();
	}

	public void updateTimer(int timeDelta){
		//Add/Subtract Timer Value
	}
	public void setupTimer(float startTime){
		//Setup Timer Value
		timePending = startTime;
		System.TimeSpan time = System.TimeSpan.FromSeconds(timePending);
		TimerGO.GetComponent<UILabel> ().text = System.DateTime.Today.Add(time).ToString("mm:tt");
		repositionTable (RightTableGO);
	}

	public void setupLives(int startLives){
		for (int i = 0; i < startLives; i++) {
			liveGOList.Add(BasicGOOperation.InstantiateNGUIGO(livePF,RightTableGO.transform));
		}
		repositionTable (RightTableGO);
	}
	public void updateLives(int liveDelta){

	}
	void Awake(){
		liveGOList = new List<GameObject> ();
		livePF= Resources.Load (LocationManager.COMPLETE_LOC_SCORE_TYPE + LocationManager.NAME_LIVE_STAR)as GameObject;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
