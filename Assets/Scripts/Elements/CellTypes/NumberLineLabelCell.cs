using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineLabelCell : Cell {
	public int LabelIndex{ get; set; }
	public CellType NumberLineLabelType;

	//Constructor
	public NumberLineLabelCell(string type, string labelIndex){
		if(type == "number_line_label") NumberLineLabelType = CellType.NumberLineLabel;
		if(type == "number_line_label_answer") NumberLineLabelType = CellType.NumberLineLabelAnswer;
		LabelIndex = labelIndex;
	}
}
