using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class PrimeDivisionLine : Line {
	public int TargetInt{get; set;}
	//Constructor
	public PrimeDivisionLine(string integer, string type){
		RowList = new List<Row>();
		TargetInt = int.Parse(integer);
		getLineType (type);  
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public PrimeDivisionLine(HtmlNode line_node){
		RowList = new List<Row>();
		Debug.Log ("Initializing PrimeDivisionLine node of type text"+ line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		getLineType (line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		getLocationType (line_node.Attributes [HTMLParser.ATTR_LOCATION_TYPE].Value);
		TargetInt = int.Parse(line_node.Attributes [HTMLParser.ATTR_TARGET].Value);
	}
	public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.PrimeDivision;
			break;
		}
	}
}

