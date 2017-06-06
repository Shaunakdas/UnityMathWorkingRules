using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableButtonCell : Cell {
	public bool correctFlag;

	//Constructor
	public SelectableButtonCell(string type, string answer){
		getCellType (type);
		correctFlag = answer=="1"? true:false;
	}
	/// <summary>
	/// Set SelectableButtonCell  Type
	/// </summary>
	public void getCellType(string type_text){
		if(type_text == "selectable_button") Type = CellType.SelectableButton;
	}
	/// <summary>
	/// Initializes a new instance of the SelectableButtonCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableButtonCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing SelectableButtonCell node of type "+type_text);
		getCellType (type_text);
		correctFlag = (cell_node.Attributes [HTMLParser.ATTR_ANSWER].Value)=="1"? true:false;
	}

}
