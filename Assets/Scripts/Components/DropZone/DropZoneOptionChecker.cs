using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneOptionChecker : OptionChecker {
	public BaseElement element;
	public bool idCheck{ get; set; }
	public GameObject DropZoneHolderGO;
	public string filledText{ get; set; }
	public bool attempted{ get; set; }
	AnimationManager animManager;
	void Awake(){
		ItemTargetType=TargetType.Option;
		if (gameObject.GetComponent<AnimationManager> () == null)
			gameObject.AddComponent<AnimationManager> ();
		animManager = gameObject.GetComponent<AnimationManager> ();
	}
	// Use this for initialization
	void Start () {
		idCheck = false;
		attempted = false;
	}
	//Checker function called after the item is dropped in drop zone.
	public bool checkDropZoneItem(string text){
		attempted = true;
		Debug.Log ("checkDropZoneItem for checking "+text);
		if (idCheck) {
			if (DropZoneHolderGO.GetComponent<DropZoneHolder> ().checkDropZoneItem (text,this)) {
				filledText = text;return true;
			}
		} else {
			if (DropZoneHolderGO.GetComponent<DropZoneHolder> ().checkDropZoneItem (compositeText(text),this)) {
				filledText = text;return true;
			}
		}
		return false;
		
	}
	public string compositeText(string text){
		string textToCheck="";
		foreach (Transform childTransform in gameObject.transform.parent) {
			if (childTransform.gameObject != gameObject) {
				//Getting char of sibling dropzone
				string childFilledText = childTransform.gameObject.GetComponent<DropZoneOptionChecker> ().filledText;
				textToCheck += (childFilledText != null) ? childFilledText : " ";
			} else {
				//Getting char of current dropzone
				textToCheck += text;
			}
		}
		return textToCheck;
	}


	// Update is called once per frame
	void Update () {
		if ((TimerAnimGO != null) && (TimerAnimGO.activeSelf)) {
			if (itemAttemptState != AttemptState.Activated) {
				NGUITools.Destroy (TimerAnimGO);
			}
		}
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>

	override public void activateAnim(){
		//Animation for selecting the correct option
		Debug.Log("activateAnim");
		if (itemAttemptState != AttemptState.Activated) {
			base.activateAnim ();
			gameObject.GetComponent<TweenColor> ().enabled = true;
			startTimerAnim ();
		}
	}
	override public void startTimerAnim(){
		//Start Item Timer
		if (itemAttemptState == AttemptState.Activated) {
			TimerAnimGO = animManager.startTimerAnim(1,DropZoneHolderGO,new EventDelegate(correctionAnim),true);
		}

	}
	override public void deactivateAnim(){
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		base.deactivateAnim();
		gameObject.GetComponent<TweenColor>().enabled = false;
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		if (itemAttemptState == AttemptState.Attempted) {
			base.correctAnim ();
			deactivateAnim ();
			animManager.correctAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem> ().gameObject, nextEvent);
		}
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		if (itemAttemptState == AttemptState.Attempted) {
			base.incorrectAnim ();
			animManager.incorrectAnim (2, gameObject, new EventDelegate (correctionAnim), true);
		}
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim"+itemAttemptState.ToString());

		if ((itemAttemptState == AttemptState.Activated)||(itemAttemptState == AttemptState.Checked)) {
			base.correctionAnim ();
			DropZoneHolderGO.GetComponent<DropZoneHolder> ().correctionAnim (this, nextEvent);
		}
	}

}
