using System.Collections;
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
//		switch (type_text) {
//		case "top": 
//			LineLocation = LocationType.Top;
//			break;
//		case "bottom": 
//			LineLocation = LocationType.Bottom;
//			break;
//		case "center": 
//			LineLocation = LocationType.Center;
//			break;
//		case "left": 
//			LineLocation = LocationType.Left;
//			break;
//		case "right": 
//			LineLocation = LocationType.Right;
//			break;
//		default:
//			LineLocation = LocationType.Default;
//			break;
//		}
		if(type_text != null){
			LineLocation = (LocationType)System.Enum.Parse (typeof(LocationType),StringWrapper.ConvertToPascalCase(type_text),true);
		}
		Debug.Log ("LineLocation TYPE" + LineLocation.ToString());
	}
	/// <summary>
	/// Initializes a new instance of the Line class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Line(HtmlNode line_node){
		RowList = new List<Row>();
		string type_text = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		parseChildNode (line_node);
		if (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE] != null) {
			getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);
		} else {
			getLocationType (null);
		}
		Interaction interactionType = (Interaction)System.Enum.Parse (typeof(Interaction),"drag",true);
		Debug.Log ("INTERACTION TYPE" + interactionType.ToString());
	}
	/// <summary>
	/// Parses the Line Node to generate Row nodes
	/// </summary>
	override public void parseChildNode(HtmlNode line_node){
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
	override public void  setChildParagraphRef(){
		foreach (Row row in RowList) {
			row.ParagraphRef = this.ParagraphRef;
			row.setChildParagraphRef ();
		}
	}

	override public int siblingIndex(){
		int index = 0;
		Paragraph para = (Parent as Paragraph);
		for (int i = 0; i < para.LineList.Count; i++) {
			if (para.LineList [i].ElementGO == this.ElementGO) {
				index = i;
			}
		}
		return index;
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
		BasicGOOperation.CheckAndRepositionTable (lineGO);
		BasicGOOperation.CheckAndRepositionTable (CenterContentGO);
		return lineGO;
	}
	override public void updateGOProp(GameObject _elementGO){
		Debug.Log ("Updating table of"+_elementGO.name);
		BasicGOOperation.CheckAndRepositionTable (_elementGO);
	}
	//----------------------Animations ----------------------------
	override public void hideElementGO(){
		BasicGOOperation.hideElementGO (ElementGO);
	}
	override public void displayElementGO(){
		bool lastFlag = (siblingIndex () == ParagraphRef.LineList.Count-1);
		EventDelegate nextEvent = new EventDelegate (ParagraphRef.finishQuestionStep);
		if (!lastFlag) {
			nextEvent = new EventDelegate (ParagraphRef.LineList [siblingIndex () + 1].displayElementGO);
		}
		displayDragSourceLine ();
		//checking for TargetItemChecker
		if (ElementGO.GetComponentsInChildren<TargetItemChecker>().Length > 0) {
			BasicGOOperation.displayElementGOAnim (ElementGO);
			activateItemCheckerListAnim (nextEvent);
		} else if (BasicGOOperation.getFirstButton(ElementGO)!=null) {
			UIButton btn = BasicGOOperation.getFirstButton (ElementGO);
			EventDelegate.Set (btn.onClick, nextEvent);
		} else {
			//
			BasicGOOperation.displayElementGOAnim (ElementGO, nextEvent);
		}
	}
	public void activateItemCheckerListAnim(EventDelegate nextEvent){
		//Going through all targetItemChecker in current Line ElementGO except the last one. Seting their next EventDelegate as the next targetItemChecker in list.
		for(int i = 0; i < Paragraph.targetItemCheckerList.Count - 1; i++){
			activateItemCheckerAnim (i,new EventDelegate (Paragraph.targetItemCheckerList [i + 1].activateAnim));
		}
		activateItemCheckerAnim (Paragraph.targetItemCheckerList.Count - 1,nextEvent);
	}
	public void activateItemCheckerAnim(int _lineIndex, EventDelegate _nextEvent){
		TargetItemChecker itemChecker = Paragraph.targetItemCheckerList [_lineIndex];
		if (ElementGO.GetComponentsInChildren<TargetItemChecker> ().Contains (itemChecker)) {
			itemChecker.nextEvent = _nextEvent;
			itemChecker.activateAnim ();
		}
	}
	public void displayDragSourceLine(){
		if (ElementGO.GetComponentInChildren<DropZoneItemChecker> () != null) {
			
		}
	}
	 public void displayElementGO(EventDelegate nextAnim){
//		BasicGOOperation.displayElementGOAnim (ElementGO,ParagraphRef.LineList);
//		foreach (Row row in RowList) {
//			row.displayElementGO ();
//		}
	}
}
