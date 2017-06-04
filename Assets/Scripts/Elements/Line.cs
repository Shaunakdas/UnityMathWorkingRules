using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Line {
	public enum LineType {Text,PostSubmitText,IncorrectSubmitText,Table,NumberLineDrop,NumberLineDropJump,NumberLineSelect,PrimeDivision,CombinationProduct,CombinationSum,CombinationProductSum};


	public List<Row> RowList {get; set;}

	//Constructor
	public Line(){
	}
	/// <summary>
	/// Set Line Type
	/// </summary>
	public void getLineType(string type_text){
	}
	/// <summary>
	/// Initializes a new instance of the Line class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Line(HtmlNode line_node){
		RowList = new List<Row>();
		string type_text = line_node.Attributes [HTMLParser.ATTR_TYPE].Value;
//		Debug.Log ("Found Line node of type "+type_text);
		getLineType (type_text);
	}
	/// <summary>
	/// Parses the Line Node to generate Row nodes
	/// </summary>
	public void parseLine(HtmlNode line_node){
//		HtmlNodeCollection node_list = line_node.SelectNodes ("//" + HTMLParser.ROW_TAG);
		IEnumerable<HtmlNode> node_list = line_node.Elements(HTMLParser.ROW_TAG) ;
		if (node_list!=null) {
//			Debug.Log ("There are " + node_list.Count);

			foreach (HtmlNode row_node in node_list) {
				Debug.Log ("Content of row_node : " + row_node.InnerHtml);
				Row row = new Row (row_node);
				RowList.Add(row);
			}
		}
	}

}
