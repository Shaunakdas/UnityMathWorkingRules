using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnItemChecker : MonoBehaviour {
	public bool correctFlag = false;
	public bool userInputFlag = false;
	public GameObject SelBtnHolderGO;
	public string DisplayText = "";
	public bool HighlightOnCorrectSelection=true;
	AnimationManager animManager;

	public void changeInputFlag(){
		userInputFlag = !userInputFlag;
		SelBtnHolderGO.GetComponent<SelBtnHolder> ().optionSelected ();
	}
	public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
	}
	public void correctAnim(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
	}
	public void incorrectAnim(EventDelegate _nextEvent){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim Delegate");
		animManager.correctAnim (1, gameObject, _nextEvent);
	}
	public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
	}
	public void correctionAnim(EventDelegate _nextEvent){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim Delegate");
		animManager.correctAnim (3, gameObject, _nextEvent);
//			_nextEvent.Execute ();
	}
	void Awake(){
		correctFlag = false; userInputFlag = false;
		if (gameObject.GetComponent<AnimationManager> () == null)
			gameObject.AddComponent<AnimationManager> ();
		animManager = gameObject.GetComponent<AnimationManager> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
