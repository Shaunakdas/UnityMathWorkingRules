using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Line : BaseElement{

	//-------------Common Attributes -------------------
	//What is the broad classification of line types?
	public enum LineType {
		Text,PostSubmitText,IncorrectSubmitText,Table,NumberLineDrop,
		NumberLineDropJump,NumberLineSelect,PrimeDivision,CombinationProduct,CombinationSum,CombinationProductSum
	}
	public LineType Type{ get; set; }

	//What is the location of line wrt paragraph content?
	public enum LocationType{
		Default,Top,Center,Bottom,Left,Right
	}
	public LocationType LineLocation { get; set; }

	//List of child rows
	public List<Row> RowList {get; set;}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Empty Constructor
	public Line(){
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
	/// Set Line Type
	/// </summary>
	virtual public void getLineType(string type_text){
		RowList = new List<Row>();
	}
	/// <summary>
	/// Set Location Type
	/// </summary>
	public void getLocationType(string type_text){
		switch (type_text) {
		case "top": 
			LineLocation = LocationType.Top;
			break;
		case "bottom": 
			LineLocation = LocationType.Bottom;
			break;
		case "center": 
			LineLocation = LocationType.Center;
			break;
		case "left": 
			LineLocation = LocationType.Left;
			break;
		case "right": 
			LineLocation = LocationType.Right;
			break;
		default:
			LineLocation = LocationType.Default;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the Line class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Line(HtmlNode line_node){
		RowList = new List<Row>();
		string type_text = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		getLineType (type_text);
		parseChildNode (line_node);
		if (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE] != null) {
			getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);
		} else {
			getLocationType ("");
		}
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
		for (int i = 0; i < (Parent as Paragraph).LineList.Count - 1; i++) {
			if ((Parent as Paragraph).LineList [i] == this)
				return i;
		}
		return 0;
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
		if (siblingIndex () < ParagraphRef.LineList.Count - 1) {
			BasicGOOperation.displayElementGOAnim (ElementGO, new EventDelegate (ParagraphRef.LineList [siblingIndex () + 1].displayElementGO));
		} else {
			BasicGOOperation.displayElementGOAnim (ElementGO);
		}
//		BasicGOOperation.displayElementGOAnim (ElementGO);
//		foreach (Row row in RowList) {
//			row.displayElementGO ();
//		}
	}
	 public void displayElementGO(EventDelegate nextAnim){
//		BasicGOOperation.displayElementGOAnim (ElementGO,ParagraphRef.LineList);
//		foreach (Row row in RowList) {
//			row.displayElementGO ();
//		}
	}
}
