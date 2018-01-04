﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Linq;

public class Line : BaseElement{

	//-------------Common Attributes -------------------
	//What is the broad classification of line types?
	public enum LineType {
		Text,PostSubmitText,IncorrectSubmitText,Table,RangeTable,NumberLineDrop,
		NumberLineDropJump,NumberLineSelect,PrimeDivision,CombinationProduct,CombinationSum,CombinationProductSum
	}
	public LineType Type{ get; set; }

	//What is the location of line wrt paragraph content?
	public enum LocationType{
		Default,Top,Center,Bottom,Left,Right
	}
	public LocationType LineLocation = LocationType.Default;

	//List of child rows
	public List<Row> RowList {get; set;}

	public List<QuestionChecker> QuestionList = new List<QuestionChecker>();

	//Score Variables
	public List<ScoreTracker> QuesScoreList = new List<ScoreTracker>();
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Empty Constructor
	public Line(){
		RowList = new List<Row>();
	}

	/// <summary>
	/// Parses the Line Node to generate Row nodes
	/// </summary>
	public void parseLine(HtmlNode line_node){
		IEnumerable<HtmlNode> node_list = line_node.Elements(AttributeManager.TAG_ROW) ;
		if (node_list!=null) {
			foreach (HtmlNode row_node in node_list) {
				Debug.Log ("Content of row_node : " + row_node.InnerHtml);
				Row row = new Row (row_node);
				row.Parent = this;
				RowList.Add(row);
			}
		}
	}
	/// <summary>
	/// Set Location Type
	/// </summary>
	public void getLocationType(string type_text){
		if(type_text != null){
			LineLocation = (LocationType)System.Enum.Parse (typeof(LocationType),StringWrapper.ConvertToPascalCase(type_text),true);
		}
//		Debug.Log ("LineLocation TYPE" + LineLocation.ToString());
	}
	/// <summary>
	/// Initializes a new instance of the Line class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Line(HtmlNode line_node){
		RowList = new List<Row>();htmlNode = line_node;
		string type_text = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;

		setAnalyticsIdFromAttr (line_node);
		parseChildNode (line_node);
		if (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE] != null) {
			getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);
		} else {
			getLocationType (null);
		}
	}
	/// <summary>
	/// Parses the Line Node to generate Row nodes
	/// </summary>
	override public void parseChildNode(HtmlNode line_node){
		IEnumerable<HtmlNode> node_list = line_node.Elements(AttributeManager.TAG_ROW) ;
		if (node_list!=null) {
			foreach (HtmlNode row_node in node_list) {
//				Debug.Log ("Content of row_node : " + row_node.InnerHtml);
				Row row = new Row (row_node);
				row.Parent = this;
				RowList.Add(row);
			}
		}
	}
	override public void  setChildParagraphRef(){
		foreach (Row row in RowList) {
			row.ParagraphRef = this.ParagraphRef;
			row.setChildParagraphRef ();
		}
	}
	override public int siblingIndex(){
		int index=0;
		List<Line> lineList = (Parent as Paragraph).LineList;
//		Debug.Log (lineList.Count);
		for (int i =0; i< lineList.Count ; i++){
//			Debug.Log (lineList[i].GetType());
//			Debug.Log (index + "index");
			if (lineList [i].ElementGO == ElementGO) {
//				Debug.Log (i + "yayy");
				index = i;
			}
		}
//		Debug.Log (index + "final");
		return index;
	}
	public bool lastSibling(){
		int index = 0;
		Paragraph para = (Parent as Paragraph);
		int dragLineCount = para.DragSourceTableList.Count;
		Debug.Log (siblingIndex () == para.LineList.Count - 1 - dragLineCount);
		if (siblingIndex () == para.LineList.Count - 1 - dragLineCount) {
			return true;
		} else {
			return false;
		}
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_LINE_TYPE + prefabName)as GameObject;

		//Instantiate Scroll View to hold center content gameobjects
//		GameObject scrollViewPrefab  = Resources.Load (LocationManager.COMPLETE_LOC_OTHER_TYPE + LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW)as GameObject;
		GameObject CenterContentScrollGO = parentGO ; GameObject CenterContentGO = parentGO;
		GameObject lineGO;
		//Based on line index and line type add line to the top/center/bottom of ContentTableGO
		switch (LineLocation) {
		case LocationType.Top:
			//Adding lineGO to the ContentTableGO at the top
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			lineGO.transform.SetAsFirstSibling ();
			break;
		case LocationType.Default:
			CenterContentScrollGO = BasicGOOperation.getChildGameObject (parentGO, LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW);
			CenterContentGO = BasicGOOperation.getChildGameObject (CenterContentScrollGO, "LineTablePF");
			//Adding QuestionStepParaPF to the root GameObject
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, CenterContentGO.transform);
			break;
		case LocationType.Bottom:
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			lineGO.transform.SetAsLastSibling ();
			break;
		case LocationType.Left:
			//Making Non-reference copy of ParaContentTable
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			lineGO.transform.SetAsFirstSibling ();
			break;
		case LocationType.Right:
			//Making Non-reference copy of ParaContentTable
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			lineGO.transform.SetAsLastSibling ();
			break;
		default:
			CenterContentScrollGO = BasicGOOperation.getChildGameObject (parentGO, LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW);
			CenterContentGO = BasicGOOperation.getChildGameObject (CenterContentScrollGO, "LineTablePF");
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, CenterContentGO.transform);
			break;
		}
		ElementGO = lineGO;
		initGOProp (lineGO);
		foreach (Row row in RowList) {
			row.generateElementGO (lineGO);
		}
		updateGOProp (lineGO);
		setupQuestionRef ();
		BasicGOOperation.CheckAndRepositionTable (lineGO);
		BasicGOOperation.CheckAndRepositionTable (CenterContentGO);
		return lineGO;
	}
	override protected void updateGOProp(GameObject _elementGO){
		Debug.Log ("Updating table of"+_elementGO.name);
		BasicGOOperation.CheckAndRepositionTable (_elementGO);
	}
	override public void setChildAnalyticsId(){
		int index = 1;
		foreach (Row row in RowList) {
			row.AnalyticsId = index;
			row.setChildAnalyticsId ();
			index++;
		}
		int quesIndex = 1;
		foreach (QuestionChecker question in QuestionList) {
			question.AnalyticsId = quesIndex;
			question.setChildAnalyticsId ();
			quesIndex++;
		}

	}

	//----------------------Score Values ----------------------------
	override public void  setChildScoreValues(){

		setupDefaultScoreValues ();
		adjustForWeightage();
		setupScoreValues ();
		foreach (Row row in RowList) {
			row.setChildScoreValues ();
		}
	}
	//Setting up default score values
	override protected void setupScoreValues(){
		if (htmlNode != null) {
			QuesScoreList = new List<ScoreTracker> (QuestionList.Count); 
			//maxQuestionScore
			if (htmlNode.Attributes [AttributeManager.MAX_QUES_SCORE] != null) {
				List<float> maxQuesScoreList = htmlNode.Attributes [AttributeManager.MAX_QUES_SCORE].Value.Split (';').ToList ().Select (s => float.Parse (s)).ToList ();
				for (int i = 0; i < QuestionList.Count; i++) {
					Debug.Log ("NOT NULLLLLL");
					QuestionList [i].scoreTracker.maxScore = repeatIfRequired (maxQuesScoreList, QuestionList.Count) [i];
				}
			} 
			//minQuestionScore
			if (htmlNode.Attributes [AttributeManager.MIN_QUES_SCORE] != null) {
				List<float> minQuesScoreList = htmlNode.Attributes [AttributeManager.MIN_QUES_SCORE].Value.Split (';').ToList ().Select (s => float.Parse (s)).ToList ();
				for (int i = 0; i < QuestionList.Count; i++) {
					QuestionList [i].scoreTracker.minScore = repeatIfRequired (minQuesScoreList, QuestionList.Count) [i];
				}
			} 
			//scoreWeightage
			if (htmlNode.Attributes [AttributeManager.SCORE_WEIGHTAGE] != null) {
				List<float> scoreWeightageList = htmlNode.Attributes [AttributeManager.SCORE_WEIGHTAGE].Value.Split (';').ToList ().Select (s => float.Parse (s)).ToList ();
				for (int i = 0; i < QuestionList.Count; i++) {
					QuestionList [i].scoreTracker.scoreWeightage = repeatIfRequired (scoreWeightageList, QuestionList.Count) [i];
				}
			} 
			//maxQuesTime
			if (htmlNode.Attributes [AttributeManager.MAX_QUES_TIME] != null) {
				List<float> maxQuesTimeList = htmlNode.Attributes [AttributeManager.MAX_QUES_TIME].Value.Split (';').ToList ().Select (s => float.Parse (s)).ToList ();
				for (int i = 0; i < QuestionList.Count; i++) {
					QuestionList [i].scoreTracker.maxTime = repeatIfRequired (maxQuesTimeList, QuestionList.Count) [i];
				}
			} 
			//maxQuestionScore
			if (htmlNode.Attributes [AttributeManager.IDEAL_QUES_TIME] != null) {
				List<float> idealQuesTimeList = htmlNode.Attributes [AttributeManager.IDEAL_QUES_TIME].Value.Split (';').ToList ().Select (s => float.Parse (s)).ToList ();
				for (int i = 0; i < QuestionList.Count; i++) {
					QuestionList [i].scoreTracker.idealTime = repeatIfRequired (idealQuesTimeList, QuestionList.Count) [i];
				}
			} 
		}
	}
	List<float> repeatIfRequired(List<float> fromList, int count){
		List<float> toList = fromList;
		if (fromList.Count == 1) {
			toList = Enumerable.Repeat(fromList[0], count).ToList(); 
		}
		return toList;
	}
	//----------------------Animations ----------------------------
	override public void hideElementGO(){
		BasicGOOperation.hideElementGO (ElementGO);
	}
	override public void displayElementGO(){
		Parent.setCurrentChild (this);

		//Setting Next Event Delegate
		EventDelegate nextEvent = new EventDelegate (ParagraphRef.finishQuestionStep);
		Debug.Log (nextEvent.ToString());
		if (!lastSibling()) {
			nextEvent = new EventDelegate (ParagraphRef.LineList [siblingIndex () + 1].displayElementGO);
		}
//		Debug.Log ("NEXT EVENT"+ElementGO.name+this.siblingIndex ().ToString()+this.lastSibling().ToString());
		if (nextEvent.target != null) {
			Debug.Log (nextEvent.target.name);
		}

		//Check for Interaction elements in Line ElementGO
		if (QuestionList.Count > 0) {
			//checking for QuestonCheckers
//			Debug.Log ("Element Display Anim:Checking for Questions");
			displayDragSourceLine ();
			BasicGOOperation.displayElementGOAnim (ElementGO,null);
			activateQuestionListAnim (nextEvent);

		
		} else if (BasicGOOperation.getFirstButton(ElementGO)!=null) {
			//checking for UIButton
			Debug.Log ("Element Display Anim:Checking for UIButton");
			UIButton btn = BasicGOOperation.getFirstButton (ElementGO);
			EventDelegate.Set (btn.onClick, nextEvent);

		} else if ((this.Type == LineType.Table)&&(ParagraphRef.DragSourceTableList.Contains(this as TableLine))) {
			//Checking for DragSourceLine Table
			Debug.Log ("Element Display Anim:Checking for DragSourceLine Table");
			nextEvent.Execute ();
		} else {
//			Debug.Log ("Element Display Anim: No Interaction Element is present");
			//If no interaction element is present
			BasicGOOperation.displayElementGOAnim (ElementGO, nextEvent);
		}
	}
	virtual public void activateQuestionListAnim(EventDelegate nextEvent){
		//Going through all questionHolders in current Line ElementGO except the last one. Seting their next EventDelegate as the next targetItemChecker in list.
//		Debug.Log("QuestionList Count"+QuestionList.Count);
		for(int i = 0; i < QuestionList.Count; i++){
			EventDelegate nextLineEvent = nextEvent;
			if (i < QuestionList.Count - 1) {
				nextLineEvent = new EventDelegate (QuestionList [i + 1].activateAnim);
			}
			Debug.Log (nextEvent.ToString());
			QuestionList [i].nextEvent = nextLineEvent;
//			Debug.Log("nextLineEvent"+(nextLineEvent.ToString()==null));
		}
		QuestionList [0].activateAnim ();
	}
	public void displayDragSourceLine(){
		if (ElementGO.GetComponentInChildren<DropZoneOptionChecker> () != null) {
			foreach (TableLine table in ParagraphRef.DragSourceTableList) {
				BasicGOOperation.displayElementGOAnim (table.ElementGO,null);
			}
		}
	}
	 public void displayElementGO(EventDelegate nextAnim){
//		BasicGOOperation.displayElementGOAnim (ElementGO,ParagraphRef.LineList);
//		foreach (Row row in RowList) {
//			row.displayElementGO ();
//		}
	}
}
