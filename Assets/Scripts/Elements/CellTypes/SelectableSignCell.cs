using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableSignCell : Cell {
	public enum Sign{Positive,Negative};
	public Sign TargetSign{ get; set; }

	//Constructor
	public SelectableSignCell(string type, string answer){
		getCellType (type);
		TargetSign = answer=="1"? Sign.Positive:Sign.Negative;
	}
	/// <summary>
	/// Set SelectableSignCell  Type
	/// </summary>
	public void getCellType(string type_text){
		if(type_text == "selectable_sign") Type = CellType.SelectableSign;
	}
	/// <summary>
	/// Initializes a new instance of the SelectableSignCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableSignCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing SelectableSignCell node of type "+type_text);
		getCellType (type_text);
		TargetSign = (cell_node.Attributes [HTMLParser.ATTR_ANSWER].Value)=="1"? Sign.Positive:Sign.Negative;
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
	}
}
