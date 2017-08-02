using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnOptionChecker : OptionChecker {
	public bool correctFlag = false;
	public bool userInputFlag = false;
//	public GameObject SelBtnHolderGO;
	public string DisplayText = "";
	public bool HighlightOnCorrectSelection = true;
	AnimationManager animManager;

	public void changeInputFlag(){
		userInputFlag = !userInputFlag;
		(ParentChecker as SelBtnQuestionChecker).optionSelected ();
	}
	override public void activateAnim(){
		//Animation for activate select button option
		Debug.Log ("activateAnim");
		if (itemAttemptState != AttemptState.Activated) {
			base.activateAnim ();
		}
	}
//	override public void activateAnimWithDelegate(EventDelegate _nextEvent){
//		//Animation for selecting the correct option
//		Debug.Log("activateAnim Delegate");
//		if (itemAttemptState != AttemptState.Activated) {
//			base.activateAnimWithDelegate ();
//			if (_nextEvent != null)
//				_nextEvent.Execute ();
//		}
//	}
	override public void deactivateAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.deactivateAnim();
	}
//	override public void deactivateAnimWithDelegate(EventDelegate _nextEvent){
//		//Animation for selecting the correct option
//		Debug.Log("correctAnim Delegate");
//		base.deactivateAnimWithDelegate (_nextEvent);
//		if (_nextEvent != null)
//			_nextEvent.Execute ();
//	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.correctAnim();
		animManager.correctAnim (1, gameObject, nextEvent);
	}
//	override public void correctAnimWithDelegate(EventDelegate _nextEvent){
//		//Animation for selecting the correct option
//		Debug.Log("correctAnim Delegate");
//		base.correctAnimWithDelegate (_nextEvent);
//		if (_nextEvent != null)
//			_nextEvent.Execute ();
//	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		base.incorrectAnim();
		animManager.incorrectAnim (1, gameObject, nextEvent,false);
	}
//	override public void incorrectAnimWithDelegate(EventDelegate _nextEvent){
//		//Animation for selecting the wrong option
//		Debug.Log("incorrectAnim Delegate");
//		base.incorrectAnimWithDelegate (_nextEvent);
//		animManager.correctAnim (1, gameObject, _nextEvent);
//	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
		if ((itemAttemptState == AttemptState.Activated) || (itemAttemptState == AttemptState.Checked)) {
			base.correctionAnim ();
			animManager.correctAnim (1, gameObject, nextEvent);
		}
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
