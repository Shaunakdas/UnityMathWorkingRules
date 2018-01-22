﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableLine  : Line {

	//-------------Common Attributes -------------------
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}
	/// <summary>
	/// Gets or sets Flag indicating whether this <see cref="TableLine"/> has selectable cell and hence contain SelBtnHolder Component.
	/// </summary>
	/// <value><c>true</c> if table contains any Selectable Cell; otherwise, <c>false</c>.</value>
	public bool SelBtnFlag{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public TableLine(){
	}
	//Constructor
	public TableLine(string type):base(){
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableLine(HtmlNode line_node):base(line_node){
		prefabName = LocationManager.NAME_TABLE_LINE;
		HtmlAttribute attr_col = line_node.Attributes [AttributeManager.ATTR_COL_COUNT];
		if (attr_col != null) {
			ColumnCount = int.Parse (attr_col.Value);
		}

	}


	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void initGOProp(GameObject ElementGO){
		foreach (Row row in RowList){
			Debug.Log (row.CellList.Count);
			if ((ColumnCount!=null)&&(row.CellList.Count < ColumnCount)){
				int extra = ColumnCount - row.CellList.Count;
				for (int i = 0; i < extra; i++) {
					Cell newCell = new TextCell ("");
					newCell.Type = Cell.CellType.Text;
					newCell.Parent = row;
					row.CellList.Add (newCell);
				}
			}
		}
	}
	override protected void updateGOProp(GameObject ElementGO){
//		Debug.Log ("COLUMN_COUNT"+ColumnCount);
		ElementGO.GetComponent<UITable> ().columns = ColumnCount;
		SelBtnQuestionChecker selBtnHolder = ElementGO.GetComponent<SelBtnQuestionChecker> ();

		if (selBtnHolder != null) {
			selBtnHolder.ContainerElem = this;
			selBtnHolder.setParentCorrectCount (Parent as Paragraph, this);
		}
		BasicGOOperation.CheckAndRepositionTable (ElementGO);

		if (Paragraph.ParagraphAlign == Paragraph.AlignType.Vertical) {
			Debug.Log ("Table width"+BasicGOOperation.ScaledBounds (ElementGO).x.ToString());
			float paddingNeeded = Screen.width -BasicGOOperation.ScaledBounds(ElementGO).x;
			if (ColumnCount > 1) {
//				ElementGO.GetComponent<UITable> ().padding.x = (paddingNeeded - 30)/(ColumnCount);
			}
		}
	}
	public static void resizeLargestTexCell(TableLine _line){
		Debug.Log ("Resizing resizeLargestTexCell"+_line.ElementGO);
		if (_line.ColumnCount>0){
			int SCREEN_WIDTH = Screen.width;
			int extra = (int)(BasicGOOperation.ElementSize (_line.ElementGO).x - (0.9)*SCREEN_WIDTH);
			TextCell largestCell = new TextCell("");
			int maxTextCellLength = 0;
			Debug.Log (extra);
			if (extra<0){
				foreach (Row row in _line.RowList) {
					//Iterating through each row
					foreach (Cell cell in row.CellList) {
						//Iterating through each cell
						if (cell.GetType () == typeof(TextCell)) {
							//If the cell is a TextCell
							if ((cell.ElementGO.GetComponents<TEXDrawNGUI> ().Length > 0)&&(cell.ElementGO.GetComponent<TEXDrawNGUI> ().width > maxTextCellLength)) {
								//LatexTextCell with width more than last biggest latexTexCell
								largestCell = cell as TextCell;
								maxTextCellLength =  Mathf.Max(maxTextCellLength, cell.ElementGO.GetComponent<TEXDrawNGUI> ().width);
							}
						}
					}
				}
				//If found the largestTextCell
				if (maxTextCellLength > 0) {
					largestCell.ElementGO.GetComponent<TEXDrawNGUI> ().width = maxTextCellLength - extra;
					_line.ElementGO.GetComponent<UITable> ().Reposition ();
				}
			}
		}
	}
	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//For Selectable Button Holder
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		SelBtnFlag = true;

		ElementGO.AddComponent<SelBtnQuestionChecker> (); 
		SelBtnQuestionChecker question = ElementGO.GetComponent<SelBtnQuestionChecker> ();
		//Ref Variables
		question.ContainerElem = this;
		question.addToMasterLine (this);
		//Seltn Specific Variables
		question.addSelectBtn (_selBtnGO, _selBtnBool);
		//Changing Paragraph's correctType
		question.setParentCorrectCount(Parent as Paragraph,this);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
