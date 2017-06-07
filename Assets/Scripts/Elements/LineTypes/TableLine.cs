using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableLine  : Line {
	//For Table type
//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public TableLine(string type){
		RowList = new List<Row>();
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableLine(HtmlNode line_node){
		RowList = new List<Row>();
		Debug.Log ("Initializing TableLine node of type text"+ line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		getLineType (line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		if (line_node.Attributes [HTMLParser.ATTR_LOCATION_TYPE] != null) {
			getLocationType (line_node.Attributes [HTMLParser.ATTR_LOCATION_TYPE].Value);
		} else {
			getLocationType ("");
		}
		if (line_node.Attributes [HTMLParser.ATTR_COL_COUNT] != null) {
			Debug.Log ("Column Attribute is present");
			ColumnCount = int.Parse (line_node.Attributes [HTMLParser.ATTR_COL_COUNT].Value);
		} else {
			ColumnCount = -1;
		}
		prefabName = LocationManager.NAME_TABLE_LINE;
	}
	public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.Table;
			break;
		}
	}
	override public void updateGOProp(GameObject ElementGO){
		ElementGO.GetComponent<UITable> ().columns = ColumnCount;
	}
}
