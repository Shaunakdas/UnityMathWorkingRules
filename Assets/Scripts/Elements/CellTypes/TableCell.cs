using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCell : Cell {
	public CellType TableType;
	//For Table type
	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public TableCell(string type){
		if(type == "fraction_table") TableType = CellType.FractionTable;
		if(type == "exponent_table") TableType = CellType.ExponentTable;
	}

}
