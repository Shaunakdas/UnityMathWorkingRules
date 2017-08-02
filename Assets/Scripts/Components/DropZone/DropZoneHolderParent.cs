using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHolderParent : QuestionChecker {
	public BaseElement Element;
	List<GameObject> dropZoneHolderList;
	public void addDropZoneHolder(GameObject dropZoneHolderGO){
		dropZoneHolderList.Add (dropZoneHolderGO);
	}
	public void dropEvent(bool inputCorrect){
		if (inputCorrect) {
			Debug.Log ("Drag Item was correctly dropped");
			correctAnim ();
		} else {
			Debug.Log ("Drag Item was incorrecly dropped.");

			searchCorrectHolder ();
			incorrectAnim ();
		}

	}
	public void searchCorrectHolder(){
		foreach (GameObject dropZoneholderItem in dropZoneHolderList) {
			if (dropZoneholderItem.GetComponent<DropZoneHolder> ().TargetTextList [0].Length > 0) {
				Debug.Log ("Correct DropZone Holder reached");
			}
		}
	}
	void Awake(){
		ItemTargetType=TargetType.HolderParent;
		dropZoneHolderList = new List<GameObject> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	override public void addToQuestionList(){
		Paragraph.QuestionList.Add (this);
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
		//Empty Paragraph Obje ct to access Paragraph methods
		ContainerElem.ParagraphRef.nextTargetTrigger (this);
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		Debug.Log ("DropZoneRowCell InCorrectAnim");
		deactivateAnim ();
		ContainerElem.ParagraphRef.nextTargetTrigger (this);
	}
}
