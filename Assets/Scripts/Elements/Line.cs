using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Line : BaseElement{
	//Line Prefabs
//	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF,TableLinePF;

	public enum LineType 
	{
		Text,PostSubmitText,IncorrectSubmitText,Table,NumberLineDrop,
		NumberLineDropJump,NumberLineSelect,PrimeDivision,CombinationProduct,CombinationSum,CombinationProductSum
	};
	public enum LocationType
	{
		Default,Top,Center,Bottom,Left
	}

	//Handling Attributes
	public LineType Type{ get; set; }
	public LocationType LineLocation { get; set; }

	//List of child rows
	public List<Row> RowList {get; set;}


	//Constructor
	public Line(){
	}
	/// <summary>
	/// Set Line Type
	/// </summary>
	public void getLineType(string type_text){
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
		string type_text = line_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		getLineType (type_text);
	}
	/// <summary>
	/// Parses the Line Node to generate Row nodes
	/// </summary>
	public void parseLine(HtmlNode line_node){
		IEnumerable<HtmlNode> node_list = line_node.Elements(HTMLParser.ROW_TAG) ;
		if (node_list!=null) {
			foreach (HtmlNode row_node in node_list) {
				Debug.Log ("Content of row_node : " + row_node.InnerHtml);
				Row row = new Row (row_node);
				row.Parent = this;
				RowList.Add(row);
			}
		}
	}

	virtual public void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Line");
	}
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	virtual public GameObject generateElementGO(GameObject parentGO){
		GameObject returnParentGO = parentGO;
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
			GameObject paraContentTableGOCopy = Instantiate (parentGO);
			parentGO.GetComponent<UITable> ().columns = 2;
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			Debug.Log ("name of parentGO" + parentGO.name);
			returnParentGO = BasicGOOperation.InstantiateNGUIGO (paraContentTableGOCopy, parentGO.transform);
			break;
		default:
			CenterContentScrollGO = BasicGOOperation.getChildGameObject (parentGO, LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW);
			CenterContentGO = BasicGOOperation.getChildGameObject (CenterContentScrollGO, "LineTablePF");
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, CenterContentGO.transform);
			break;
		}

		initGOProp (lineGO);
		foreach (Row row in RowList) {
			row.generateElementGO (lineGO);
		}
		updateGOProp (lineGO);
		BasicGOOperation.CheckAndRepositionTable (CenterContentGO);
		BasicGOOperation.CheckAndRepositionTable (lineGO);
		return returnParentGO;
	}
}
