﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class DragSourceCell : Cell {

	public enum SeriesType{Integer,Prime};
	public SeriesType SourceType;
	//For Table type
	//	public List<Row> RowList {get; set;}
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public DragSourceCell(string type, string id, string displayText){
		getCellType (type);
		CellId =  (id);
		DisplayText = displayText;
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}
	//Constructor
	public DragSourceCell(string type, string displayText){
		getCellType (type);
		DisplayText = displayText;
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}
	/// <summary>
	/// Initializes a new instance of the DragSourceCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public DragSourceCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing DragSourceCell node of type "+type_text);
		getCellType (type_text);
		HtmlAttribute attr_tag = cell_node.Attributes [AttributeManager.ATTR_ID];
		if (attr_tag != null) {
			CellId = cell_node.Attributes [AttributeManager.ATTR_ID].Value;
		}
		DisplayText = cell_node.InnerText;
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}


	/// <summary>
	/// Set Cell  Type
	/// </summary>
	public void getCellType(string type_text){
		if(type_text == "drag_source") Type = CellType.DragSource;
	}
	public void getSourceType(string source_type){
		switch (source_type) {
		case "integer": 
			SourceType = SeriesType.Integer;
			break;
		case "prime": 
			SourceType = SeriesType.Prime;
			break;
		}
	}


	override public void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
		GameObject labelGO = BasicGOOperation.getChildGameObject (ElementGO, "Label");
		labelGO.GetComponent<UILabel> ().text = DisplayText;
		ElementGO.name = ElementGO.name + "_"+ generateStandardName(DisplayText);
		ElementGO.GetComponent<UIDragDropItem> ().restriction = (DragAlign == Paragraph.AlignType.Horizontal)? UIDragDropItem.Restriction.Horizontal:UIDragDropItem.Restriction.Vertical ;
		//Increasing size based on text
//		float width = BasicGOOperation.getTextSize(DisplayText);
//		float elementWidth = ElementGO.GetComponent<UISprite> ().localSize.y;
//		elementWidth = (width > 30f) ? (width + 30f) : 60f;
	}
	public string generateStandardName(string text){
		int number; string newText = text;
		if (int.TryParse (text, out number)) {
			number = number + 100;
			newText = number.ToString (); int rem = 3 - newText.Length; 
			for (int i = 0; i < rem; i++) {
				newText = "0" + newText;
			}
		}
		return newText;
	}
}
