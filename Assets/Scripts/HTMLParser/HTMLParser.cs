using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using Tidy;

public class HTMLParser  {
	string inputHtml;
	public const string BODY_TAG = "body";
	public const string P_TAG = "p",LINE_TAG = "line",ROW_TAG="trow",CELL_TAG = "tcell";
	public const string ATTR_ANSWER = "answer",ATTR_TYPE="type";
	//QuestionStep Paragraph
	public const string ATTR_CORRECT_TYPE = "correctType";
	//NumberLineDrop Line
	public const string ATTR_LABEL_COUNT = "labelCount",ATTR_LABEL_INDEX = "labelIndex";
	//NumberLineDropJump Line
	public const string ATTR_DROP_START_INDEX="dropStartIndex",ATTR_DROP_COUNT="dropStartIndex",ATTR_JUMP_SIZE="attr_jump_size";
	//Combination Line 
	public const string ATTR_OUTPUT_VISIBLE="outputVisible";
	//DragSourceLine Row 
	public const string ATTR_START="start",ATTR_END="end",ATTR_SOURCE_TYPE="sourceType";
	//DropZone Row
	public const string ATTR_SIZE="size";
	//DragSource and DropZoneRow Cell 
	public const string ATTR_ID="id",ATTR_TAG="tag";
	//PrimeDisivion Line
	public const string ATTR_TARGET="target";
	//Line Location
	public const string ATTR_LOCATION_TYPE="locationType";


	public List<Paragraph> ParagraphList{ get; set; }
	public HTMLParser(){
		ParagraphList = new List<Paragraph> ();
	}
	public void getParagraphList(HtmlDocument html){
//		HtmlNodeCollection question_list = html.DocumentNode.SelectNodes ("//" + P_TAG);
		IEnumerable<HtmlNode> question_list = html.DocumentNode.Elements(HTMLParser.P_TAG) ;
//		Debug.Log ("There are " + question_list.Count + " nodes of type: p");
//		HtmlNode para = question_list [0];

		foreach (HtmlNode para_node in question_list) {
			Paragraph para = new Paragraph (para_node);
			para.parseParagraph (para_node);
			ParagraphList.Add (para);
		}

	}
}
