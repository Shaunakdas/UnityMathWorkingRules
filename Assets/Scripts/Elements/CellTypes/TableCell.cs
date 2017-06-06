using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableCell : Cell {
	//For Table type
	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public TableCell(string type){
		if(type == "fraction_table") Type = CellType.FractionTable;
		if(type == "exponent_table") Type = CellType.ExponentTable;
	}
	/// <summary>
	/// Set Cell Type
	/// </summary>
	public void getCellType(string type_text){
		switch (type_text) {
		case "fraction_table": 
			Type = CellType.FractionTable;
			break;
		case "exponent_table": 
			Type = CellType.ExponentTable;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableCell(HtmlNode cell_node){
		RowList = new List<Row> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing TableCell node of type "+type_text);
		getCellType (type_text);
		parseTableCell (cell_node);
	}
	/// <summary>
	/// Parses the parseTableCell Node to generate Row nodes
	/// </summary>
	public void parseTableCell(HtmlNode cell_node){
//		HtmlNodeCollection node_list = cell_node.SelectNodes ("//" + HTMLParser.ROW_TAG);
		IEnumerable<HtmlNode> node_list = cell_node.Elements(HTMLParser.ROW_TAG) ;
		if (node_list!=null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.ROW_TAG);

			foreach (HtmlNode row_node in node_list) {
				RowList.Add (new Row (row_node));
			}
		}
	}
}
