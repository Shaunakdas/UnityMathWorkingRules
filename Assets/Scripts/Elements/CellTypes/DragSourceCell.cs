using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSourceCell : Cell {
	public CellType DragSourceType;
	public string CellId { get; set; }
	public string CellTag { get; set; }
	public string DisplayText {get; set;}
	//For Table type
	//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public DragSourceCell(string type, string id, string displayText){
		if(type == "drag_source") DragSourceType = CellType.DragSource;
		CellId =  (id);
		DisplayText = displayText;
	}

}
