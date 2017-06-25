using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using Tidy;

public class HTMLParser  {
	string inputHtml;


	public List<Paragraph> ParagraphList{ get; set; }
	public HTMLParser(){
		ParagraphList = new List<Paragraph> ();
	}
	public void getParagraphList(HtmlDocument html){
//		HtmlNodeCollection question_list = html.DocumentNode.SelectNodes ("//" + P_TAG);
		IEnumerable<HtmlNode> question_list = html.DocumentNode.Elements(AttributeManager.TAG_P) ;
//		Debug.Log ("There are " + question_list.Count + " nodes of type: p");
//		HtmlNode para = question_list [0];

		foreach (HtmlNode para_node in question_list) {
			Paragraph para = new Paragraph (para_node);
//			para.parseChildNode (para_node);
			ParagraphList.Add (para);
		}

	}
}
