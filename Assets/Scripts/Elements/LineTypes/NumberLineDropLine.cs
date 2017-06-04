using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineDropLine  : Line {
	public LineType NumberLineType{ get; set; }
	public int LabelCount{ get; set; }

	//For NumberLineDropJump
	public int DropStartIndex{get; set;}
	public int DropCount{ get; set; }
	public int JumpSize{ get; set; }

	///<summary>
	/// Constructor for NumberLine and NumberLineDrop
	/// </summary>
	public NumberLineDropLine(string labelCount, string type){	
		LabelCount = int.Parse(labelCount);
		getLineType (type);
	}

	///<summary>
	/// Constructor for NumberLineDropLine
	/// </summary>
	public NumberLineDropLine(string labelCount,string dropStartIndex,string dropCount,string jumpSize, string type){
		RowList = new List<Row>();	
		LabelCount = int.Parse(labelCount);
		getLineType (type);
		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (dropStartIndex);
			DropCount = int.Parse (dropCount);
			JumpSize = int.Parse (jumpSize);
		}
	}
	/// <summary>
	/// Initializes a new instance of the NumberLineDropLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineDropLine(HtmlNode line_node){
		RowList = new List<Row>();
		string type = line_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Found Line node of type text"+ type);
		LabelCount = int.Parse(line_node.Attributes [HTMLParser.ATTR_LABEL_COUNT].Value);
		getLineType (line_node.Attributes [HTMLParser.ATTR_TYPE].Value);

		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (line_node.Attributes [HTMLParser.ATTR_DROP_START_INDEX].Value);
			DropCount = int.Parse (line_node.Attributes [HTMLParser.ATTR_DROP_COUNT].Value);
			JumpSize = int.Parse (line_node.Attributes [HTMLParser.ATTR_JUMP_SIZE].Value);
		}
	}
	public void getLineType(string type_text){
		switch (type_text) {
		case "number_line_drop": 
			NumberLineType = LineType.NumberLineDrop;
			break;
		case "number_line_select": 
			NumberLineType = LineType.NumberLineSelect;
			break;
		case "number_line_drop_jump": 
			NumberLineType = LineType.NumberLineDropJump;
			break;
		}
	}
}
