using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineLabelCell : Cell {
	public int LabelIndex{ get; set; }
	public CellType NumberLineLabelType;

	//Constructor
	public NumberLineLabelCell(string type, string labelIndex){
		getCellType (type);
		LabelIndex = int.Parse(labelIndex);
	}
	/// <summary>
	/// Set NumberLineLabelCell  Type
	/// </summary>
	public void getCellType(string type_text){
		switch (type_text) {
		case "number_line_label": 
			NumberLineLabelType = CellType.NumberLineLabel;
			break;
		case "number_line_label_answer": 
			NumberLineLabelType = CellType.NumberLineLabelAnswer;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the NumberLineLabelCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineLabelCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing NumberLineLabelCell node of type "+type_text);
		getCellType (type_text);
		LabelIndex = int.Parse(cell_node.Attributes [HTMLParser.ATTR_LABEL_INDEX].Value);
	}
}
