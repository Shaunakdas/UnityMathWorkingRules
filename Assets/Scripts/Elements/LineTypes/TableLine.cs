using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLine  : Line {
	public Type TableType;
	//For Table type
//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public TableLine(string type){
		if(type == "table") TableType = Type.Table;
	}
}
