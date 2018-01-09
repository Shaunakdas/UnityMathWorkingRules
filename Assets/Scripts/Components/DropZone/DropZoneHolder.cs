using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHolder : QuestionChecker {

	//----------------------Common attributes ----------------------------
	public bool idCheck{ get; set; }
	public bool multipleHolderCheck;
	public GameObject holderListParentGO;
	public List<string> TargetTextList{get; set;}
	public int TargetPending{ get; set; }
	/// <summary>
	/// list of list of DropZoneItemChecker for managing animatiojn after user input. 
	/// </summary>
	public List<List<DropZoneOptionChecker>> ItemCheckerMasterList{ get; set; }
	void Awake(){
		ItemTargetType=TargetType.Holder;
		multipleHolderCheck = false;
		ItemCheckerMasterList = new List<List<DropZoneOptionChecker>> ();
	}
	// Use this for initialization
	void Start () {
//		TargetTextList = new List<string> ();
	}

	//----------------------Target Matching and checking mechanism ----------------------------

	public bool checkDropZoneItem( DropZoneQuestionChecker _ques){
		List<string> inputTextList = new List<string>();
//		foreach (OptionChecker _option in _ques.ChildList) {
//			inputTextList.Add ((_option as DropZoneOptionChecker).filledText);
//		}
		addOptionText(inputTextList, _ques);
		Debug.Log ("Holder: checkDropZoneItem for checking "+inputTextList[0]);
		bool inputCorrect = false;
		if (idCheck) {
			if (TargetTextList.IndexOf(inputTextList[0])!= -1) {
				TargetTextList.Remove(inputTextList[0]);
				inputCorrect = true;
			}
		} else {
			if (checkCompositeText(inputTextList)) {
				inputCorrect = true;
			}
		}
		if (multipleHolderCheck) {
			holderListParentGO.GetComponent<DropZoneHolderParent> ().dropEvent(inputCorrect);
		}
		//		resultAnim (inputCorrect, itemChecker);
		return inputCorrect;
	}
	public bool checkCompositeText(List<string> inputTextList){

		//Checking through TargetTextList;
		foreach (string targetText in TargetTextList) {
			Debug.Log("inputText"+inputTextList[0]+"targetText"+targetText);
			//Checking for TriedText with same length;
			if (targetText.Length == inputTextList.Count) {


				//Checking for matching exact text
				if ((noNullElement(inputTextList))&&(string.Concat(inputTextList.ToArray()) == targetText)) {
					//Removing targetText from targetTextList
					TargetTextList.Remove(targetText);
					return true;
				}
				int inputCharIndex = 0;
				bool foundMisMatch = true;
				//Iterating through whole list of characters of targetText
				foreach (string inputString in inputTextList) {
					foundMisMatch = false;

					// Checking for matching text after removing space
					if ((inputString != null)&&(inputString != targetText.ToCharArray()[inputCharIndex].ToString())) {
						foundMisMatch = true;
					}
					Debug.Log (foundMisMatch.ToString()+inputString);
					inputCharIndex ++;
					if (foundMisMatch) {
						break;
					}
				}
				if (!foundMisMatch) {
					return true;
				}
			}
		}
		return false;
	}
	bool noNullElement(List<string> inputList){
		foreach (string input in inputList) {
			if (input == null) {
				return false;
			}
		}
		return true;
	}
	List<string> getCorrectionOptions(DropZoneOptionChecker _option, DropZoneQuestionChecker _ques){
		List<string> correctionOptions = new List<string> ();
		if (idCheck) {
			return TargetTextList;
		} else {
			List<string> inputTextList = new List<string>();
			addOptionText(inputTextList, _ques);
			//Checking through TargetTextList;
			foreach (string targetText in TargetTextList) {
				Debug.Log ("inputText" + inputTextList [0] + "targetText" + targetText);
				//Checking for TriedText with same length;
				if (targetText.Length == inputTextList.Count) {
					int inputCharIndex = 0;
					bool foundMisMatch = true;
					//Iterating through whole list of characters of targetText
					foreach (string inputString in inputTextList) {
						foundMisMatch = false;
						// Checking for matching text after removing space
						if ((inputString != null) && (inputString != targetText.ToCharArray () [inputCharIndex].ToString ())) {
							foundMisMatch = true;
						}
						inputCharIndex++;
						if (foundMisMatch) {
							break;
						}
					}
					if (!foundMisMatch) {
						correctionOptions.Add (targetText.ToCharArray () [_option.getSiblingIndex ()].ToString());
					}
				}
			}
		}

		return correctionOptions;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void addOptionText(List<string> inputTextList, DropZoneQuestionChecker _ques){
		foreach (OptionChecker _option in _ques.ChildList) {
			if (_option.GetType () == typeof(DropZoneOptionChecker)) {
				inputTextList.Add ((_option as DropZoneOptionChecker).filledText);
			} else if (_option.GetType () == typeof(SelSignOptionChecker)) {
				inputTextList.Add ((_option as SelSignOptionChecker).filledText);
			}
		}
	}
	//----------------------Animations ----------------------------


	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void activateAnim(){
		Debug.Log ("activateAnim of DropZoneRowCell");
		List<DropZoneOptionChecker> DropZoneItemCheckerList = new List<DropZoneOptionChecker> ();
		foreach(DropZoneOptionChecker itemChecker in gameObject.GetComponentsInChildren<DropZoneOptionChecker>()){
			Debug.Log (itemChecker.gameObject.name);
			DropZoneItemCheckerList.Add (itemChecker); 
		}
		for (int i = 0; i < DropZoneItemCheckerList.Count; i++) {
			EventDelegate nextItemEvent = nextEvent;
			if (i < DropZoneItemCheckerList.Count - 1) {
				nextItemEvent = new EventDelegate (DropZoneItemCheckerList [i + 1].activateAnim);
			}
			DropZoneItemCheckerList[i].nextEvent = nextItemEvent;
		}
		DropZoneItemCheckerList [0].activateAnim();
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void deactivateAnim(){
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		foreach (DropZoneOptionChecker itemChecker in ChildList) {
			itemChecker.deactivateAnim ();
		}
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	override public void correctAnim(){
		Debug.Log ("DropZoneRowCell CorrectAnim");
	}
	public void correctAnim(DropZoneOptionChecker itemChecker){
		Debug.Log ("DropZoneRowCell CorrectAnim");
//		if (!nextItemChecker (itemChecker))
//			nextTargetTrigger ();
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		Debug.Log ("DropZoneRowCell InCorrectAnim"+ContainerElem.ParagraphRef.ElementGO.name);
	}
//	public void incorrectAnim(DropZoneOptionChecker itemChecker){
//		Debug.Log ("DropZoneRowCell InCorrectAnim"+ContainerElem.ParagraphRef.ElementGO.name);
//		correctionAnim (itemChecker);
//	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	override public void correctionAnim(){
		Debug.Log ("DropZoneRowCell correctionAnim");
	}
//	public void correctionAnim(DropZoneOptionChecker itemChecker){
//		Debug.Log ("DropZoneRowCell correctionAnim");
//		DragSourceCell targetCell = findCorrectDragItem (itemChecker);
//		targetCell.dragToDropZone (itemChecker.gameObject,null);
//	}
	public void correctionAnim(DropZoneOptionChecker _option,DropZoneQuestionChecker _ques,EventDelegate _nextEvent){
		Debug.Log ("DropZoneRowCell correctionAnim");
		DragSourceCell targetCell = findCorrectDragItem(_option,_ques);
		Debug.Log(_option.ItemAttemptState.ToString());
		targetCell.dragToDropZone (_option.gameObject,_nextEvent);
	}

	/// <summary>
	/// Finds the if another DropZoneItemChecker exists in masterList of list of DropZoneItemChecker in current DropZoneHolder
	/// </summary>
	/// <returns><c>true</c>, if item checker was nexted, <c>false</c> otherwise.</returns>
	/// <param name="_attemptedItemChecker">Attempted item checker.</param>
	public bool nextItemChecker(DropZoneOptionChecker _attemptedItemChecker){
		bool nextFound = false;
		List<DropZoneOptionChecker> attemptedItemCheckerList = new List<DropZoneOptionChecker>();
		foreach (List<DropZoneOptionChecker> itemCheckerList in ItemCheckerMasterList) {
			foreach(DropZoneOptionChecker itemChecker in itemCheckerList){
				if (_attemptedItemChecker == itemChecker) {
					attemptedItemCheckerList = itemCheckerList;
				}
			}
		}
		//Deactivating animation of attempted dropzone
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
//		ContainerElem.ParagraphRef.nextTargetTrigger (this);

	}
	public void nextTargetTrigger(DragSourceCell dragCell){
		
		Debug.Log ("TargetPending"+TargetPending);
//		ContainerElem.ParagraphRef.nextTargetTrigger (this);

	}
	List<string> allowedString(DropZoneOptionChecker _option,DropZoneQuestionChecker _ques){
		List<string> allowedList = new List<string> ();
		if (idCheck) {


		} else {

		}
		return allowedList;
	}
	/// <summary>
	/// Returns the correct drag item.
	/// </summary>
	/// <returns>The correct drag item.</returns>
	/// <param name="dropZoneItem">Drop zone item</param>
	public DragSourceCell findCorrectDragItem(DropZoneOptionChecker _option, DropZoneQuestionChecker _ques){
		//Find the target drag cell
		DragSourceCell targetCell;
//		Debug.Log ("Inside findCorrectDragItem"+_ques.getSiblingIndex()+_option.getSiblingIndex());
//		Debug.Log ("Inside findCorrectDragItem"+TargetTextList.Count);
//		Debug.Log ("Inside findCorrectDragItem"+TargetTextList[_ques.getSiblingIndex()].ToCharArray()[_option.getSiblingIndex()].ToString());
		foreach (Line line in ContainerElem.ParagraphRef.LineList) {
			foreach (Row row in line.RowList) {
				if (row.Type == Row.RowType.DragSourceLine) {
					foreach (Cell cell in row.CellList) {
						string cellText = cell.ElementGO.GetComponentInChildren<UILabel> ().text;
						if((getCorrectionOptions(_option,_ques).IndexOf(cellText))>-1){
							Debug.Log ("Found correct DragCell "+ TargetTextList[0]);
							return (cell as DragSourceCell);
						}

					}
				}
			}
		}
		return null;
	}
}
