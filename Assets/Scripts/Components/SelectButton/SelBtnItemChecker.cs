using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnItemChecker : MonoBehaviour {
	public bool correctFlag{get; set;}
	public bool userInputFlag;
	public GameObject SelBtnHolderGO;

	public void changeInputFlag(){
		userInputFlag = !userInputFlag;
	}
	public void incorrectOptionSelected(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectOptionSelected");
	}
	public void correctOptionSelected(){
		//Animation for selecting the correct option
		Debug.Log("correctOptionSelected");
	}
	public void correctOptionIgnored(){
		//Animation for ignoring the correct option
		Debug.Log("correctOptionIgnored");
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
