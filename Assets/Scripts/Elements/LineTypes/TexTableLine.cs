using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TexTableLine : TableLine {

	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public TexTableLine(){
	}
	//Constructor
	public TexTableLine(string type):base(){
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TexTableLine(HtmlNode line_node):base(line_node){

	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void initGOProp(GameObject ElementGO){
//		base.initGOProp (ElementGO);
		TexTableLine newLine = convertTexLine (this); 
		RowList = newLine.RowList;
		ColumnCount = newLine.ColumnCount;
	}
	override protected void updateGOProp(GameObject ElementGO){
	}
	virtual protected TexTableLine convertTexLine(TexTableLine _line){
		TexTableLine texTableLine = _line;
		string updatedText = texString (_line);
		if (texTableLine.ColumnCount == null)
			texTableLine.ColumnCount = _line.RowList[0].CellList.Count;
		
		//Creating First row
		texTableLine.RowList = new List<Row> ();
		Row row = new Row ();
		row.Parent = texTableLine;

		//Creating First Cell
		Cell newCell = new TextCell (updatedText);
		string cell_type = "text";
		newCell.Type = (Cell.CellType)System.Enum.Parse (typeof(Cell.CellType),StringWrapper.ConvertToPascalCase(cell_type),true);
		//Add Cell node into CellList
		newCell.Parent = this;
		Debug.Log (newCell.DisplayText);
		row.CellList.Add (newCell);

		texTableLine.RowList.Add(row);

		return texTableLine;
	}
	virtual protected string texString(TexTableLine _line){
		string _texString = "";
		int rowCounter = _line.RowList.Count;
		foreach (Row row in _line.RowList){
			Debug.Log (row.CellList.Count);
			rowCounter--;
			int cellCounter = row.CellList.Count;
			foreach (Cell cell in row.CellList){
				cellCounter--;
				_texString += cell.DisplayText;
				if ((cellCounter == 0)&&(rowCounter == 0)) {
				} else if (cellCounter == 0) {
					_texString += "|";
				} else if (rowCounter+cellCounter > 0) {
					_texString += "&";
				}
			}
		}
		return texTableString(_texString);
	}
	virtual protected string texTableString(string _texString){
		return "\\ltable[111111]" + _texString;
	}

}
