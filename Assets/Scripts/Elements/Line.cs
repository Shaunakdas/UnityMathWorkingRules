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
		Default,Top,Center,Bottom
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
}
