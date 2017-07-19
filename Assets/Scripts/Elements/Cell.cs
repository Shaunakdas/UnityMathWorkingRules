using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Cell : TableLine {
	//-------------Common Attributes -------------------
	public Paragraph.AlignType DragAlign;

	public enum CellType {Text,DropZone,DropZoneRow,SelectableButton,SelectableSign,DragSource,FractionTable,ExponentTable,NumberLineLabel,NumberLineLabelAnswer};
	public CellType Type{get; set;}

	public string CellId{ get; set; } 
	public string CellTag{ get; set; }
	public string DisplayText{ get; set; }

	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	public Cell(){
		RowList = new List<Row> ();
	}
	public Cell(string type_text){
		RowList = new List<Row> ();
		getCellType (type_text);
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	virtual public void getCellType(string type_text){
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Cell(HtmlNode cell_node){
		RowList = new List<Row> ();
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing Cell node of type "+type_text);
		getCellType (type_text);
		parseChildNode (cell_node);
	}
	override public void parseChildNode(HtmlNode cell_node){
		Debug.Log ("inside parseChildNode of Cell");
	}
	virtual public void  getAlignType(){
		DragAlign = Paragraph.ParagraphAlign;
	}
	override public int siblingIndex(){
		for (int i = 0; i < (Parent as Row).CellList.Count - 1; i++) {
			if ((Parent as Row).CellList [i] == this)
				return i;
		}
		return 0;
	}
	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		getAlignType ();
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		ElementGO = cellGO;
		initGOProp (cellGO);
		foreach (Row row in RowList) {
			row.generateElementGO (cellGO);
		}
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	override public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell");
	}
}
