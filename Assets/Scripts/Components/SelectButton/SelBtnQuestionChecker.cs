using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnQuestionChecker : QuestionChecker {
//	public List<GameObject> SelBtnGOList;
	public int correctCount = 0;
	public int depth;
	public void addSelectBtn(GameObject selBtnGO, bool correctFlag){
		if ((correctFlag != null) && correctFlag) {
			correctCount++;
		}
		depth = selBtnGO.GetComponent<UIWidget> ().depth;
		OptionChecker itemChecker = selBtnGO.GetComponent<SelBtnOptionChecker> ();
		if(ChildList.IndexOf(itemChecker)== -1)
			ChildList.Add (itemChecker);
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
			submitBtnGO.GetComponent<UIWidget> ().width = 280;
		}
	}
	public void optionSelected(){
		if (correctCount == 1) 
			checkChildCorrect ();
	}
	public void checkChildCorrect(){
//		Debug.Log ("checkChildCorrect");
		correctionAnim ();

	}
	void Awake(){
		ItemTargetType=TargetType.Question;
//		Debug.Log ("Awake of SelBtnHolder called");
	}
	// Use this for initialization
	void Start () {
//		Debug.Log ("Start of SelBtnHolder called");
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
		base.activateAnim ();
//		Debug.Log ("getActiveAnim of DropZoneRowCell");
		for (int i = 0; i < ChildList.Count; i++) {
			ChildList[i].activateAnim();
		}
	}
	override public void startTimerAnim (){
		
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void deactivateAnim(){
		base.deactivateAnim ();
		Debug.Log ("getActiveAnim of DropZoneRowCell");
		for (int i = 0; i < ChildList.Count; i++) {
			ChildList[i].deactivateAnim();
		}
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	override public void correctAnim(){
		base.correctAnim ();
		Debug.Log ("DropZoneRowCell CorrectAnim");
		deactivateAnim ();
//		nextAnimTrigger ();
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	override public void incorrectAnim(){
		base.incorrectAnim ();
		Debug.Log ("DropZoneRowCell InCorrectAnim");
		deactivateAnim ();
//		nextAnimTrigger ();
	}
	/// <summary>
	/// Correction animation.
	/// </summary>
	override public void correctionAnim(){
		base.correctionAnim ();
		Debug.Log ("DropZoneRowCell CorrectionAnim");
		Debug.Log (nextEvent.ToString());
		bool quesCorrect = true;
		//check in not user selected buttons
//		foreach (GameObject selBtnGO in SelBtnGOList) {
		for (int i=0;i<ChildList.Count; i++){
			SelBtnOptionChecker option = ChildList [i] as SelBtnOptionChecker;
			option.attemptEvent();
			bool optionCorrect = true;
//			EventDelegate _nextEvent = null;
			if (i == ChildList.Count - 1) {
				option.nextEvent = nextEvent;
			}
			if (option.userInputFlag) {
				//CheckAnimation if userinputFlag is correct. Wait
				if (option.correctFlag) {
					option.correctAnim ();
					optionCorrect = true;
				} else {
					option.incorrectAnim ();
					optionCorrect = false;
				}
			} else if (option.correctFlag) {
				option.ItemAttemptState = AttemptState.Checked;
				optionCorrect = false;
				//Animation for showing the correct options
				option.correctionAnim ();
			} else {
				option.destroyAnim ();
			}
//			Debug.Log ("Before And"+quesCorrect.ToString()+optionCorrect.ToString());
			quesCorrect = quesCorrect && optionCorrect;
//			Debug.Log ("After And"+quesCorrect.ToString()+optionCorrect.ToString());
		}
		ScoreManager.Result _quesResult = (quesCorrect == true) ?  (ScoreManager.Result.Correct) : (ScoreManager.Result.Incorrect);
		notifyManager (_quesResult);
//		deactivateAnim ();
	}
	override public void nextAnimTrigger(){
		Debug.Log ("nextAnimTrigger"+nextEvent.ToString());
//		nextEvent.Execute ();
		//		ParagraphRef.nextTargetTrigger (this);
	}
}
