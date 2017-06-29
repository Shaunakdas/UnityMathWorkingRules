using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableLine  : Line {
	//For Table type
//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}
	public bool SelBtnFlag{ get; set; }
	//Constructor
	public TableLine(){
	}
	//Constructor
	public TableLine(string type){
		RowList = new List<Row>();
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableLine(HtmlNode line_node):base(line_node){
		prefabName = LocationManager.NAME_TABLE_LINE;
	}
	override public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.Table;
			break;
		}
	}
	override public void updateGOProp(GameObject ElementGO){
		ElementGO.GetComponent<UITable> ().columns = ColumnCount;
		SelBtnHolder selBtnHolder = ElementGO.GetComponent<SelBtnHolder> ();
		if (selBtnHolder != null) {
			selBtnHolder.setParentCorrectCount (Parent as Paragraph, this);
		}
		BasicGOOperation.CheckAndRepositionTable (ElementGO);
	}
	//For Selectable Button Cell
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		SelBtnFlag = true;
		ElementGO.AddComponent<SelBtnHolder> (); SelBtnHolder holderScript = ElementGO.GetComponent<SelBtnHolder> ();
		holderScript.addSelectBtn (_selBtnGO, _selBtnBool);
		//Changing Paragraph's correctType
		holderScript.setParentCorrectCount(Parent as Paragraph,this);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
