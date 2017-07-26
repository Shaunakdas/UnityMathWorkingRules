using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneItemChecker : TargetOptionChecker {
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
				string childFilledText = childTransform.gameObject.GetComponent<DropZoneItemChecker> ().filledText;
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

	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>

	override public void activateAnim(){
		//Animation for selecting the correct option
		Debug.Log("activateAnim");
		base.activateAnim();
		gameObject.GetComponent<TweenColor>().enabled = true;
	}
	override public void activateAnimWithDelegate(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("activateAnim Delegate");
		base.activateAnimWithDelegate (_nextEvent);
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	override public void deactivateAnim(){
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		base.deactivateAnim();
		gameObject.GetComponent<TweenColor>().enabled = false;
	}
	override public void deactivateAnimWithDelegate(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		base.deactivateAnimWithDelegate (_nextEvent);
		if (_nextEvent != null)
			_nextEvent.Execute ();
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.correctAnim();
		deactivateAnim ();
		animManager.correctAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem>().gameObject, nextEvent);
	}
	override public void correctAnimWithDelegate(EventDelegate _nextEvent){
		//Animation for selecting the correct option
		Debug.Log("correctAnim Delegate");
		base.correctAnimWithDelegate (_nextEvent);
		animManager.correctAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem>().gameObject, _nextEvent);
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		base.incorrectAnim();
		deactivateAnim ();
		animManager.incorrectAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem>().gameObject, new EventDelegate(correctionAnim),true);
	}
	override public void incorrectAnimWithDelegate(EventDelegate _nextEvent){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim Delegate");
		base.incorrectAnimWithDelegate (_nextEvent);
		animManager.correctAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem>().gameObject, _nextEvent);
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
		base.correctionAnim();
		DropZoneHolderGO.GetComponent<DropZoneHolder> ().correctionAnim (this, nextEvent);
	}
	override public void correctionAnimWithDelegate(EventDelegate _nextEvent){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim Delegate");
		base.correctionAnimWithDelegate (_nextEvent);
		animManager.correctAnim (3, gameObject, _nextEvent);
		//			_nextEvent.Execute ();
	}

}
