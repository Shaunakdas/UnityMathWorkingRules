using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class ExponentCell : Cell {
	//For Table type
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}

	//Constructor
	public ExponentCell(string type):base(type){
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public ExponentCell(HtmlNode cell_node) :base(cell_node){
		prefabName = LocationManager.NAME_EXPONENT_TABLE_CELL;
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
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		GameObject ContentGO = BasicGOOperation.getChildGameObject (cellGO, LocationManager.NAME_CONTENT_TABLE);
		ElementGO = cellGO;
		initGOProp (cellGO);
		foreach (Row row in RowList) {
			row.generateElementGO (ContentGO);
		}
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	override protected void updateGOProp(GameObject cellGO){
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		//Getting maximum height of sibling gameobjects.
		float maxSiblingHeight=0f;
		foreach (Transform siblingTf in cellGO.transform.parent) {
			if (siblingTf != cellGO.transform) {
				maxSiblingHeight = Mathf.Max (maxSiblingHeight, (NGUIMath.CalculateAbsoluteWidgetBounds (siblingTf).size.y/BasicGOOperation.scale.y));
				Debug.Log ("maxSiblingHeight"+maxSiblingHeight);
			}
		}

		GameObject ContentGO = BasicGOOperation.getChildGameObject (cellGO, LocationManager.NAME_CONTENT_TABLE);

		float contentHeight = NGUIMath.CalculateAbsoluteWidgetBounds (ContentGO.transform).size.y / BasicGOOperation.scale.y;
		//Adding empty widget of height 3/4 of earlier calculated widgets to cellGO
		GameObject emptyPf = Resources.Load (LocationManager.COMPLETE_LOC_OTHER_TYPE + LocationManager.NAME_EMPTY_CONTAINER)as GameObject;
		GameObject emptyGO = BasicGOOperation.InstantiateNGUIGO (emptyPf, cellGO.transform);
		emptyGO.GetComponent<UIWidget> ().height =  (int)((0.5 * maxSiblingHeight) + (contentHeight));
	}
}	
