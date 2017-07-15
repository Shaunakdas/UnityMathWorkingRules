using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHolder : TargetItemChecker {
	public bool idCheck{ get; set; }
	public bool multipleHolderCheck;
	public GameObject holderListParentGO;
	public List<string> TargetTextList{get; set;}
	public int TargetPending{ get; set; }
	public List<List<DropZoneItemChecker>> ItemCheckerMasterList{ get; set; }
	void Awake(){
		multipleHolderCheck = false;
		ItemCheckerMasterList = new List<List<DropZoneItemChecker>> ();
	}
	// Use this for initialization
	void Start () {
//		TargetTextList = new List<string> ();
	}
	override public void addToTargetList(){
		Paragraph.targetItemCheckerList.Add (this);
	}
	public bool checkDropZoneItem(string inputText, DropZoneItemChecker itemChecker){
		Debug.Log ("Holder: checkDropZoneItem for checking "+inputText);
		bool inputCorrect = false;
		if (idCheck) {
			if (TargetTextList.IndexOf(inputText)!= -1) {
				inputCorrect = true;
			}
		} else {
			if (checkCompositeText(inputText)) {
				inputCorrect = true;
			}
		}
		if (multipleHolderCheck) {
			holderListParentGO.GetComponent<DropZoneHolderParent> ().dropEvent(inputCorrect);
		}
		resultAnim (inputCorrect, itemChecker);
//		if (inputCorrect) {
//			correctAnim (itemChecker);
//		} else {
//			incorrectAnim (itemChecker);
//		}
		return inputCorrect;
	}
	public bool checkCompositeText(string inputText){
		
		//Checking through TargetTextList;
		foreach (string targetText in TargetTextList) {
			Debug.Log("inputText"+inputText+"targetText"+targetText);
			//Checking for TriedText with same length;
			if (targetText.Length == inputText.Length) {


				//Checking for matching exact text
				if ((inputText.IndexOf(' ') == -1)&&(inputText == targetText)) {
					//Removing targetText from targetTextList
					TargetTextList.Remove(inputText);
					return true;
				}
				int inputCharIndex = 0;
				//Iterating through whole list of characters of targetText
				foreach (char inputChar in inputText.ToCharArray()) {
					// Checking for matching text after removing space
					if ((inputChar != ' ')&&(inputChar == targetText.ToCharArray()[inputCharIndex])) {
						
						return true;
					}
					inputCharIndex ++;
				}
			}
		}
		return false;
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
		Debug.Log ("activateAnim of DropZoneRowCell");
		foreach (TweenColor itemColor in this.gameObject.GetComponentsInChildren<TweenColor>()) {
			itemColor.enabled = true;
		}
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void deactivateAnim(){
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		foreach (TweenColor itemColor in this.gameObject.GetComponentsInChildren<TweenColor>()) {
			itemColor.enabled = false;
		}
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	override public void correctAnim(){
		Debug.Log ("DropZoneRowCell CorrectAnim");
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		Debug.Log ("DropZoneRowCell InCorrectAnim"+ParagraphRef.ElementGO.name);
	}

	public void resultAnim(bool inputCorrect, DropZoneItemChecker itemChecker){
		if(inputCorrect){
			correctAnim ();
		}else{
			incorrectAnim();
		}
		if (!nextItemChecker (itemChecker))
			nextTargetTrigger ();
	}
	public bool nextItemChecker(DropZoneItemChecker _attemptedItemChecker){
		bool nextFound = false;
		List<DropZoneItemChecker> attemptedItemCheckerList = new List<DropZoneItemChecker>();
//		for (int i =0; i<ItemCheckerMasterList.Count; i++){
//			for (int j=0; j< ItemCheckerMasterList[i].Count; j++){
		foreach (List<DropZoneItemChecker> itemCheckerList in ItemCheckerMasterList) {
			foreach(DropZoneItemChecker itemChecker in itemCheckerList){
				if (_attemptedItemChecker == itemChecker) {
					attemptedItemCheckerList = itemCheckerList;
				}
			}
		}

		_attemptedItemChecker.deactivateAnim ();
		attemptedItemCheckerList.Remove (_attemptedItemChecker);
		if (attemptedItemCheckerList.Count > 0)
			nextFound = true;
		else {
			ItemCheckerMasterList.Remove (attemptedItemCheckerList);
		}
		if ((!nextFound)&&(ItemCheckerMasterList.Count > 0)) {
			nextFound = true;
		}
		return nextFound;
	}
	public void nextTargetTrigger(){
		Debug.Log ("TargetPending"+TargetPending);
		ParagraphRef.nextTargetTrigger (this);
		
	}
}
