﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnHolder : TargetItemChecker {
//	public List<GameObject> SelBtnGOList;
	public int correctCount = 0;
	public int depth;
	public void addSelectBtn(GameObject selBtnGO, bool correctFlag){
		if (correctFlag)
			correctCount++;
		depth = selBtnGO.GetComponent<UIWidget> ().depth;
		TargetOptionChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
		if(TargetOptionCheckerList.IndexOf(itemChecker)== -1)
			TargetOptionCheckerList.Add (itemChecker);
	}
	public void setParentCorrectCount(Paragraph para, Line line){
		if (correctCount > 1) {
			para.ParagraphCorrect = Paragraph.CorrectType.MultipleCorrect;
			addSubmitBtn (line);
		}else
			para.ParagraphCorrect = Paragraph.CorrectType.SingleCorrect;
	}
	public void addSubmitBtn(Line line){
		GameObject paraGO = line.Parent.ElementGO;
		BasicGOOperation.CheckAndRepositionTable (line.ElementGO);
		Vector2 lineSize = BasicGOOperation.ElementSize (line.ElementGO);

		GameObject submitPrefab = Resources.Load (LocationManager.COMPLETE_LOC_LINE_TYPE + LocationManager.NAME_SUBMIT_LINE)as GameObject;
		GameObject submitBtnGO = BasicGOOperation.InstantiateNGUIGO (submitPrefab, line.ElementGO.transform.parent);
		//Setting index
		int tableLineIndex = line.ElementGO.transform.GetSiblingIndex();
		submitBtnGO.transform.SetSiblingIndex (tableLineIndex + 1);
		submitBtnGO.GetComponent<UIWidget> ().depth = depth;
		submitBtnGO.GetComponentInChildren<UISprite> ().depth = depth+4;submitBtnGO.GetComponentInChildren<UILabel> ().depth = depth+4;
		submitBtnGO.GetComponent<TweenColor> ().enabled = true;
		BasicGOOperation.CheckAndRepositionTable (line.ElementGO.transform.parent.gameObject);
		EventDelegate.Set(submitBtnGO.GetComponent<UIButton>().onClick, delegate() { checkChildCorrect(); });

		if(Paragraph.ParagraphAlign == Paragraph.AlignType.Horizontal){
		}else{
			submitBtnGO.GetComponent<UIWidget> ().width = (int)lineSize.x-200;
		}
	}
	public void optionSelected(){
		if (correctCount == 1) 
			checkChildCorrect ();
	}
	public void checkChildCorrect(){
		Debug.Log ("checkChildCorrect");
		//Check for user selected Buttons
//		foreach (GameObject selBtnGO in SelBtnGOList) {
//			
//			SelBtnItemChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
//			Debug.Log (itemChecker.userInputFlag);
//			Debug.Log (itemChecker.correctFlag);
//			if (itemChecker.userInputFlag) {
//				//CheckAnimation if userinputFlag is correct. Wait
//				if (itemChecker.correctFlag) {
//					itemChecker.correctAnim ();
//				} else {
//					itemChecker.incorrectAnim ();
//				}
//			}
//		}
		correctionAnim ();
//		deactivateAnim ();
//		nextAnimTrigger ();

	}
	void Awake(){
		ItemTargetType=TargetType.Item;
		Debug.Log ("Awake of SelBtnHolder called");
	}
	// Use this for initialization
	void Start () {
		Debug.Log ("Start of SelBtnHolder called");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	override public void addToTargetList(){
		if(Paragraph.targetItemHolderList.IndexOf(this) < 0) Paragraph.targetItemHolderList.Add (this);
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void activateAnim(){
		Debug.Log ("getActiveAnim of DropZoneRowCell");
		foreach (TweenColor itemColor in this.gameObject.GetComponentsInChildren<TweenColor>()) {
			itemColor.enabled = true;
		}
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void deactivateAnim(){
		Debug.Log ("getActiveAnim of DropZoneRowCell");
		foreach (TweenColor itemColor in this.gameObject.GetComponentsInChildren<TweenColor>()) {
			itemColor.enabled = false;
		}
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	override public void correctAnim(){
		Debug.Log ("DropZoneRowCell CorrectAnim");
		deactivateAnim ();
//		nextAnimTrigger ();
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		Debug.Log ("DropZoneRowCell InCorrectAnim");
		deactivateAnim ();
//		nextAnimTrigger ();
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	override public void correctionAnim(){
		Debug.Log ("DropZoneRowCell CorrectionAnim");
		//check in not user selected buttons
//		foreach (GameObject selBtnGO in SelBtnGOList) {
		for (int i=0;i<TargetOptionCheckerList.Count; i++){
			SelBtnItemChecker itemChecker = TargetOptionCheckerList [i] as SelBtnItemChecker;
			itemChecker.itemAttemptState = AttemptState.Attempted;
			EventDelegate _nextEvent = null;
			if (i == TargetOptionCheckerList.Count - 1)
				_nextEvent = new EventDelegate(nextAnimTrigger);
			if (itemChecker.userInputFlag) {
				//CheckAnimation if userinputFlag is correct. Wait
				if (itemChecker.correctFlag) {
					itemChecker.nextEvent = _nextEvent;itemChecker.correctAnim ();
//					itemChecker.correctAnimWithDelegate (_nextEvent);
				} else {
					itemChecker.nextEvent = _nextEvent;itemChecker.incorrectAnim ();
//					itemChecker.incorrectAnimWithDelegate (_nextEvent);
				}
			} else if (itemChecker.correctFlag) {
				itemChecker.itemAttemptState = AttemptState.Checked;
				//Animation for showing the correct options
				itemChecker.nextEvent = _nextEvent;itemChecker.correctionAnim ();
//				itemChecker.correctionAnimWithDelegate(_nextEvent);
			}
		}
		deactivateAnim ();
//		nextAnimTrigger ();
	}
	override public void nextAnimTrigger(){
		Debug.Log ("nextAnimTrigger"+nextEvent.ToString());
		nextEvent.Execute ();
		//		ParagraphRef.nextTargetTrigger (this);
	}
}
