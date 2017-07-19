﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnItemChecker : MonoBehaviour {
	public bool correctFlag{get; set;}
	public bool userInputFlag;
	public GameObject SelBtnHolderGO;

	public void changeInputFlag(){
		userInputFlag = !userInputFlag;
		SelBtnHolderGO.GetComponent<SelBtnHolder> ().optionSelected ();
	}
	public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
	}
	public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
	}
	public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
	}
	void Awake(){
		correctFlag = false; userInputFlag = false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
