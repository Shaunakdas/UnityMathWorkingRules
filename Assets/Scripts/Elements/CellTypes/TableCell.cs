using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableCell : Cell {
	//For Table type
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public TableCell(string type):base(type){
	}
	/// <summary>
	/// Set Cell Type
	/// </summary>
	override public void getCellType(string type_text){
		switch (type_text) {
		case "fraction_table": 
			Type = CellType.FractionTable;
			break;
		case "exponent_table": 
			Type = CellType.ExponentTable;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableCell(HtmlNode cell_node) :base(cell_node){
		prefabName = LocationManager.NAME_TABLE_CELL;
	}
	/// <summary>
	/// Parses the parseTableCell Node to generate Row nodes
	/// </summary>
	override public void parseChildNode(HtmlNode cell_node){
//		HtmlNodeCollection node_list = cell_node.SelectNodes ("//" + HTMLParser.ROW_TAG);
		IEnumerable<HtmlNode> node_list = cell_node.Elements(AttributeManager.TAG_ROW) ;
		if (node_list!=null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.ROW_TAG);

			foreach (HtmlNode row_node in node_list) {
				Row row = new Row (row_node);
				row.Parent = this;
				RowList.Add(row);
			}

		}
	}
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		Debug.Log ("TableCell generateElementGO");
		getAlignType ();
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_LINE_TYPE + prefabName)as GameObject;
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
}	
