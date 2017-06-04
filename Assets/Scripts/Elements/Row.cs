using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Row  {
	public enum RowType {Default,DragSourceLine};
	public RowType RowElementType{ get; set; }

	public List<Cell> CellList{get; set;}

	//Constructor
	public Row(){

	}
	/// <summary>
	/// Set Row  Type
	/// </summary>
	public void getRowType(string type_text){
		RowElementType = RowType.Default;
		if(type_text == "drag_source_line") RowElementType = RowType.DragSourceLine;
	}
	/// <summary>
	/// Initializes a new instance of the Row class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Row(HtmlNode row_node){
		RowElementType = RowType.Default;
		CellList = new List<Cell> ();
		HtmlAttribute attr_tag = row_node.Attributes [HTMLParser.ATTR_TYPE];
		if (attr_tag != null) {
			string type_text = row_node.Attributes [HTMLParser.ATTR_TYPE].Value;
			Debug.Log ("Found Row node of type "+type_text);
			getRowType (type_text);
		}

	}
	/// <summary>
	/// Parses the Row Node to generate Cell nodes
	/// </summary>
	public void parseRow(HtmlNode row_node){
//		HtmlNodeCollection node_list = row_node.SelectNodes ("//" + HTMLParser.CELL_TAG);
		IEnumerable<HtmlNode> node_list = row_node.Elements(HTMLParser.CELL_TAG) ;
//		if (node_list!=null) {
		if (node_list!=null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.CELL_TAG);

			foreach (HtmlNode cell_node in node_list) {
				CellList.Add (new Cell (cell_node));
			}
		}
	}
}
