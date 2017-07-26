using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOptionChecker : MonoBehaviour {
	public enum AttemptState{Unseen,Seen,Activated,Attempted,Checked,Corrected,Deactivated}
	public AttemptState itemAttemptState = AttemptState.Unseen;
	public EventDelegate nextEvent;
	public Paragraph ParagraphRef;

	public enum TargetType{Option,Item,Holder,HolderParent}
	public TargetType ItemTargetType=TargetType.Option;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	virtual public void addToTargetList(){
		itemAttemptState = AttemptState.Unseen;
	}
	//----------------------Animations ----------------------------
	virtual public void trial(){
		Debug.Log ("trial"+gameObject.name);
	}
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
