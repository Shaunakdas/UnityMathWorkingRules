using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnItemChecker : TargetOptionChecker {
	public bool correctFlag = false;
	public bool userInputFlag = false;
	public GameObject SelBtnHolderGO;
	public string DisplayText = "";
	public bool HighlightOnCorrectSelection = true;
	AnimationManager animManager;

	public void changeInputFlag(){
		userInputFlag = !userInputFlag;
		SelBtnHolderGO.GetComponent<SelBtnHolder> ().optionSelected ();
	}
	override public void activateAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.activateAnim();
	}
	override public void activateAnim(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		base.activateAnim (_nextEvent);
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	override public void deactivateAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.deactivateAnim();
	}
	override public void deactivateAnim(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		base.deactivateAnim (_nextEvent);
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.correctAnim();
	}
	override public void correctAnim(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		base.correctAnim (_nextEvent);
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		base.incorrectAnim();
	}
	override public void incorrectAnim(EventDelegate _nextEvent){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim Delegate");
		base.incorrectAnim (_nextEvent);
		animManager.correctAnim (1, gameObject, _nextEvent);
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
		base.correctionAnim();
	}
	override public void correctionAnim(EventDelegate _nextEvent){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim Delegate");
		base.correctionAnim (_nextEvent);
		animManager.correctAnim (3, gameObject, _nextEvent);
//			_nextEvent.Execute ();
	}
	void Awake(){
		ItemTargetType=TargetType.Option;
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
