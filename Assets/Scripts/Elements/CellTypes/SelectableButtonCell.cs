using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableButtonCell : Cell {
	public bool correctFlag;
	public CellType SelectableButtonType;

	//Constructor
	public SelectableButtonCell(string type, string answer){
		if(type == "selectable_button") SelectableButtonType = CellType.SelectableButton;
		correctFlag = answer=="1"? true:false;
	}

}
