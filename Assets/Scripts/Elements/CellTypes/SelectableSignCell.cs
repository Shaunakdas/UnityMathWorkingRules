using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableSignCell : Cell {
	public enum Sign{Positive,Negative};
	public Sign TargetSign{ get; set; }
	public CellType SelectableSignType;

	//Constructor
	public SelectableSignCell(string type, string answer){
		if(type == "selectable_sign") SelectableSignType = CellType.SelectableSign;
		TargetSign = answer=="1"? Sign.Positive:Sign.Negative;
	}
}
