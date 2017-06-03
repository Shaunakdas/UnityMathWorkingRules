using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineDropLine  : Line {
	public Type NumberLineType{ get; set; }
	public int LabelCount{ get; set; }
	//Constructor
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
}
