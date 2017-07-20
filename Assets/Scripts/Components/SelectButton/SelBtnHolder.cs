using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnHolder : TargetItemChecker {
	public List<GameObject> SelBtnGOList;
	public int correctCount;
	public int depth;
	public void addSelectBtn(GameObject selBtnGO, bool correctFlag){
		if (correctFlag)
			correctCount++;
		depth = selBtnGO.GetComponent<UIWidget> ().depth;
		Debug.Log (selBtnGO.GetComponent<SelBtnItemChecker>().correctFlag);
		if(SelBtnGOList.IndexOf(selBtnGO)== -1)
			SelBtnGOList.Add (selBtnGO);
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
		foreach (GameObject selBtnGO in SelBtnGOList) {
			
			SelBtnItemChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
			Debug.Log (itemChecker.userInputFlag);
			Debug.Log (itemChecker.correctFlag);
			if (itemChecker.userInputFlag) {
				//CheckAnimation if userinputFlag is correct. Wait
				if (itemChecker.correctFlag) {
					itemChecker.correctAnim ();
				} else {
					itemChecker.incorrectAnim ();
				}
			}
		}
		correctionAnim ();
		deactivateAnim ();
		nextAnimTrigger ();

	}
	void Awake(){
		Debug.Log ("Awake of SelBtnHolder called");
		correctCount = 0;SelBtnGOList = new List<GameObject> ();
	}
	// Use this for initialization
	void Start () {
		Debug.Log ("Start of SelBtnHolder called");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	override public void addToTargetList(){
		if(Paragraph.targetItemCheckerList.IndexOf(this) < 0) Paragraph.targetItemCheckerList.Add (this);
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
		nextAnimTrigger ();
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		Debug.Log ("DropZoneRowCell InCorrectAnim");
		deactivateAnim ();
		nextAnimTrigger ();
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	override public void correctionAnim(){
		Debug.Log ("DropZoneRowCell CorrectionAnim");
		//check in not user selected buttons
		foreach (GameObject selBtnGO in SelBtnGOList) {
			SelBtnItemChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
			if ((!itemChecker.userInputFlag)&&(itemChecker.correctFlag)) {
				//Animation for showing the correct options
				itemChecker.correctionAnim();
			}
		}
		deactivateAnim ();
		nextAnimTrigger ();
	}
	override public void nextAnimTrigger(){
		Debug.Log ("nextAnimTrigger"+nextEvent.ToString());
		nextEvent.Execute ();
		//		ParagraphRef.nextTargetTrigger (this);
	}
}
