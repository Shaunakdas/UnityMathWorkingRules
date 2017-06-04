using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class HTMLParserTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HTMLParser parser = new HTMLParser ();

		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
		parser.getParagraphList (html);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
