﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionChecker : MonoBehaviour {
	public enum AttemptState{Unseen,Seen,Activated,Attempted,Checked,Corrected,Deactivated}
	//Target Text
	AttemptState _itemAttemptState= AttemptState.Unseen;
	public AttemptState ItemAttemptState{ 
		get { return _itemAttemptState; }
		set { _itemAttemptState = value; scoreTracker.state = value; }
	}
	public enum TargetType{Option,Question,Holder,HolderParent}
	public TargetType ItemTargetType=TargetType.Option;

	//Animation Variables
	public EventDelegate nextEvent;
	public GameObject TimerAnimGO = null;

	//Reference Variables
	public int AnalyticsId = 0;
	public BaseElement ContainerElem;
	public OptionChecker ParentChecker;
	public List<OptionChecker> ChildList = new List<OptionChecker>();

	//Score Variables
	public ScoreTracker scoreTracker = new ScoreTracker();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	virtual public void addParentChecker(OptionChecker _parentChecker){
		ParentChecker = _parentChecker;
		ParentChecker.ChildList.Add (this);
	}
	virtual public void setChildAnalyticsId(){
		int optionIndex = 1;
		foreach (OptionChecker option in ChildList) {
			option.AnalyticsId = optionIndex;
			optionIndex++;
		}
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void activateAnim(){
		ItemAttemptState = AttemptState.Activated;
		scoreTracker.startTimestamp = System.DateTime.Now;
	}
	virtual public void activateAnimWithDelegate(EventDelegate _nextEvent){
		ItemAttemptState = AttemptState.Activated;
	}

	/// <summary>
	/// Start Item Timer.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void startTimerAnim(){
	}
	virtual public void attemptEvent(){
		ItemAttemptState = AttemptState.Attempted;
		scoreTracker.attemptTimestamp = System.DateTime.Now;
		scoreTracker.attemptTime = scoreTracker.attemptTimestamp - scoreTracker.startTimestamp;
		Debug.Log ("scoreTracker.maxScore"+scoreTracker.maxScore+scoreTracker.attemptTime.ToString());
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void deactivateAnim(){
		ItemAttemptState = AttemptState.Deactivated;
		scoreTracker.endTimestamp = System.DateTime.Now;
	}
	virtual public void deactivateAnimWithDelegate(EventDelegate _nextEvent){
		ItemAttemptState = AttemptState.Deactivated;
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	virtual public void correctAnim(){
		ItemAttemptState = AttemptState.Checked;
	}
	virtual public void correctAnimWithDelegate(EventDelegate _nextEvent){
		ItemAttemptState = AttemptState.Checked;
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	virtual public void incorrectAnim(){
		ItemAttemptState = AttemptState.Checked;
	}
	virtual public void incorrectAnimWithDelegate(EventDelegate _nextEvent){
		ItemAttemptState = AttemptState.Checked;
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	virtual public void correctionAnim(){
		ItemAttemptState = AttemptState.Corrected;
	}
	virtual public void correctionAnimWithDelegate(EventDelegate _nextEvent){
		ItemAttemptState = AttemptState.Corrected;
	}
	/// <summary>
	/// Triggers the next Animation.
	/// </summary>
	virtual public void nextAnimTrigger(){
	}


	//----------------------Score Values ----------------------------

	virtual public void setChildScoreValues(){
		setupScoreValues ();
	}

	virtual public void setupScoreValues(){
	}
}
