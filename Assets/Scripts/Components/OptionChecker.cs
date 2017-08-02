using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionChecker : MonoBehaviour {
	public enum AttemptState{Unseen,Seen,Activated,Attempted,Checked,Corrected,Deactivated}
	public AttemptState itemAttemptState = AttemptState.Unseen;
	public EventDelegate nextEvent;
//	public Paragraph ParagraphRef;
	public BaseElement ContainerElem;
	public GameObject TimerAnimGO = null;
	public enum TargetType{Option,Question,Holder,HolderParent}
	public TargetType ItemTargetType=TargetType.Option;

	public int AnalyticsId = 0;

	public OptionChecker ParentChecker;
	public List<OptionChecker> ChildList = new List<OptionChecker>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	virtual public void addToQuestionList(){
		itemAttemptState = AttemptState.Unseen;
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void activateAnim(){
		itemAttemptState = AttemptState.Activated;
	}
	virtual public void activateAnimWithDelegate(EventDelegate _nextEvent){
		itemAttemptState = AttemptState.Activated;
	}

	/// <summary>
	/// Start Item Timer.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void startTimerAnim(){
	}

	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void deactivateAnim(){
		itemAttemptState = AttemptState.Deactivated;
	}
	virtual public void deactivateAnimWithDelegate(EventDelegate _nextEvent){
		itemAttemptState = AttemptState.Deactivated;
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	virtual public void correctAnim(){
		itemAttemptState = AttemptState.Checked;
	}
	virtual public void correctAnimWithDelegate(EventDelegate _nextEvent){
		itemAttemptState = AttemptState.Checked;
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	virtual public void incorrectAnim(){
		itemAttemptState = AttemptState.Checked;
	}
	virtual public void incorrectAnimWithDelegate(EventDelegate _nextEvent){
		itemAttemptState = AttemptState.Checked;
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	virtual public void correctionAnim(){
		itemAttemptState = AttemptState.Corrected;
	}
	virtual public void correctionAnimWithDelegate(EventDelegate _nextEvent){
		itemAttemptState = AttemptState.Corrected;
	}
	/// <summary>
	/// Triggers the next Animation.
	/// </summary>
	virtual public void nextAnimTrigger(){
	}
}
