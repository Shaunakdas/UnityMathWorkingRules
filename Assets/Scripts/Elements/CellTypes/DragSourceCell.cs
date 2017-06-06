using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class DragSourceCell : Cell {
	public enum SeriesType{Integer,Prime};
	public SeriesType SourceType;
	public string CellId { get; set; }
	public string CellTag { get; set; }
	public string DisplayText {get; set;}
	//For Table type
	//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public DragSourceCell(string type, string id, string displayText){
		
		CellId =  (id);
		DisplayText = displayText;
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	public void getCellType(string type_text){
		if(type_text == "drag_source") Type = CellType.DragSource;
	}
	/// <summary>
	/// Initializes a new instance of the DragSourceCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public DragSourceCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing DragSourceCell node of type "+type_text);
		getCellType (type_text);
		CellId = cell_node.Attributes [HTMLParser.ATTR_ID].Value;
		DisplayText = cell_node.InnerText;
	}
	public void getSourceType(string source_type){
		switch (source_type) {
		case "integer": 
			SourceType = SeriesType.Integer;
			break;
		case "prime": 
			SourceType = SeriesType.Prime;
			break;
		}
	}
}
