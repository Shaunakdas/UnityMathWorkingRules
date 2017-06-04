using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineDropLine  : Line {
	public Type NumberLineType{ get; set; }
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
		switch (type) {
		case "number_line_drop": 
			NumberLineType = Type.NumberLineDrop;
			break;
		case "number_line_select": 
			NumberLineType = Type.NumberLineSelect;
			break;
		case "number_line_drop_jump": 
			NumberLineType = Type.NumberLineDropJump;
			break;
		}
	}
	//Constructor
	public NumberLineDropLine(string labelCount,string dropStartIndex,string dropCount,string jumpSize, string type){	
		LabelCount = int.Parse(labelCount);
		switch (type) {
		case "number_line_drop": 
			NumberLineType = Type.NumberLineDrop;
			break;
		case "number_line_select": 
			NumberLineType = Type.NumberLineSelect;
			break;
		case "number_line_drop_jump": 
			NumberLineType = Type.NumberLineDropJump;
			break;
		}
		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (dropStartIndex);
			DropCount = int.Parse (dropCount);
			JumpSize = int.Parse (jumpSize);
		}
	}
}
