using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row  {
	public enum RowType {Default,DragSourceLine};
	public RowType RowElementType{ get; set; }

	public List<Cell> CellList{get; set;}

	//Constructor
	public Row(string type){
		RowElementType = RowType.Default;
		if(type == "drag_source_line") RowElementType = RowType.DragSourceLine;
	}
}
