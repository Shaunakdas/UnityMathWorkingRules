using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Cell : BaseElement {
	//-------------Common Attributes -------------------
	public Paragraph.AlignType DragAlign;

	public enum CellType {Text,DropZone,DropZoneRow,SelectableButton,SelectableSign,DragSource,FractionTable,ExponentTable,NumberLineLabel,NumberLineLabelAnswer};
	public CellType Type{get; set;}

	public string CellId{ get; set; } 
	public string CellTag{ get; set; }
	public string DisplayText{ get; set; }

	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	public Cell(){
	}

	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Cell(HtmlNode cell_node){
//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
//		Debug.Log ("Initializing Cell node of type "+type_text);
		getCellType (type_text);
	}
	virtual public void  getAlignType(){
		Paragraph paraObj = (Paragraph)this.Parent.Parent.Parent;
		DragAlign = paraObj.ParagraphAlign;
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	virtual public void getCellType(string type_text){
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	virtual public GameObject generateElementGO(GameObject parentGO){
		getAlignType ();
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	virtual public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell");
	}
}
