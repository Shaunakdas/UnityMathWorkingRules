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
		ItemTargetType=TargetType.Question;
		multipleHolderCheck = false;
		ItemCheckerMasterList = new List<List<DropZoneOptionChecker>> ();
	}
	// Use this for initialization
	void Start () {
//		TargetTextList = new List<string> ();
	}

	//----------------------Target Matching and checking mechanism ----------------------------
	override public void addToQuestionList(){
		Paragraph.QuestionList.Add (this);
	}
	public bool checkDropZoneItem(string inputText, DropZoneOptionChecker itemChecker){
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
	public void resultAnim(bool inputCorrect, DropZoneOptionChecker itemChecker){
		itemChecker.itemAttemptState = AttemptState.Attempted;
		EventDelegate nextItemEvent = nextEvent;
		int itemIndex = ChildList.IndexOf (itemChecker);
		if(inputCorrect){
			itemChecker.correctAnim ();
		}else{
			itemChecker.incorrectAnim();
		}
		//If DropZone Holder doesn't has pending DropZoneItemChecker

	}

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
	public void incorrectAnim(DropZoneOptionChecker itemChecker){
		Debug.Log ("DropZoneRowCell InCorrectAnim"+ContainerElem.ParagraphRef.ElementGO.name);
		correctionAnim (itemChecker);
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	override public void correctionAnim(){
		Debug.Log ("DropZoneRowCell correctionAnim");
	}
	public void correctionAnim(DropZoneOptionChecker itemChecker){
		Debug.Log ("DropZoneRowCell correctionAnim");
		DragSourceCell targetCell = findCorrectDragItem(itemChecker);
//		targetCell.DroppedOnSurface += delegate {
//			if (!nextItemChecker (itemChecker))
//				nextTargetTrigger ();
//		};

		targetCell.dragToDropZone (itemChecker.gameObject,null);
	}
	public void correctionAnim(DropZoneOptionChecker itemChecker,EventDelegate _nextEvent){
		Debug.Log ("DropZoneRowCell correctionAnim");
		DragSourceCell targetCell = findCorrectDragItem(itemChecker);
//		targetCell.DroppedOnSurface += delegate {
//			if (!nextItemChecker (itemChecker))
//				nextTargetTrigger ();
//		};
		Debug.Log(targetCell.ElementGO.name);
		targetCell.dragToDropZone (itemChecker.gameObject,_nextEvent);
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
		ContainerElem.ParagraphRef.nextTargetTrigger (this);

	}
	public void nextTargetTrigger(DragSourceCell dragCell){
		
		Debug.Log ("TargetPending"+TargetPending);
		ContainerElem.ParagraphRef.nextTargetTrigger (this);

	}
	/// <summary>
	/// Returns the correct drag item.
	/// </summary>
	/// <returns>The correct drag item.</returns>
	/// <param name="dropZoneItem">Drop zone item</param>
	public DragSourceCell findCorrectDragItem(DropZoneOptionChecker dropZoneItem){
		//Find the target drag cell
		DragSourceCell targetCell;
		Debug.Log ("Inside findCorrectDragItem");
		foreach (Line line in ContainerElem.ParagraphRef.LineList) {
			foreach (Row row in line.RowList) {
				if (row.Type == Row.RowType.DragSourceLine) {
					foreach (Cell cell in row.CellList) {
						if (idCheck) {
							if(cell.ElementGO.GetComponentInChildren<UILabel>().text == TargetTextList[0]){
								Debug.Log ("Found correct DragCell "+ TargetTextList[0]);
								return (cell as DragSourceCell);
							}
						} else {
							if(cell.ElementGO.GetComponentInChildren<UILabel>().text == TargetTextList[0]){
								Debug.Log ("Found correct DragCell "+ TargetTextList[0]);
								return (cell as DragSourceCell);
							}
						}

					}
				}
			}
		}
		return null;
	}
}
