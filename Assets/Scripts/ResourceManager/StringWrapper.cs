﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Contrib;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
public static class StringWrapper {

	static List<string> oldStringUpper,newStringUpper,oldStringLower ,newStringLower;
	static List<string> newString;
	public static void init(){
		//$$ Latex structure
		oldStringUpper = new List<string> { "\\div","$$","&#160;","<span>","</span>","\\displaystyle","\\frac","\\cfrac","<br/>","<div>","</div>","\\dfrac","\\times","\\circ","&nbsp","\\delta","\\cong","\\angle","\\sqrt","\\RightArrow","\\left ","\\right ","\\triangle","<br>","&amp;","\\," };
		newStringUpper = new List<string> { "\\div",""," ","","","","\\frac","\\frac","\n","","","\\frac","\\times","\\circ"," ","\\delta","\\approxeq","\\angle","\\root","\\Rightarrow","","","\\delta","","&",""};
		//{tex} Latex structure
		oldStringLower = new List<string> { "\\div","\\cong","{tex}","{/tex}","\\times","\\circ","</br>","\\frac","\\Delta","\\angle",'_'.ToString(),"\\text","\\left","\\right","^","\\%","\\pi","\\triangle","\\bot","\\ne","<br />","\\parallel","∠","\\cong","<br/>" };
		newStringLower = new List<string> { "\\div","\\approxeq","{","}","\\times","\\circ","\n","\\frac","\\delta","\\angle",'_'.ToString(),"\\text","","","^","\\%","\\pi","\\delta","\\bot","\\ne","\n","\\parallel","\\angle","\\approxeq","\n"};

	}
	public static string changeString(string textToChange){
		init ();
//		int std = UserProfile.userStandard.standard_number;
		int std = 6;
//		Debug.Log ("User standard"+std);

		if(textToChange.IndexOf ("\\_") == -1){
//			Debug.Log ("Replacing underscore");
			textToChange =  textToChange.Replace ('_'.ToString(), "\\_");
		}
		if(textToChange.IndexOf ('^') != -1){
			Debug.Log ("Replacing exponent");

			MatchCollection matches = Regex.Matches(textToChange, @"\^([\d]+)");
			for (int i = 0; i < matches.Count; i++)
			{
				string match = matches[i].ToString();
				int start = textToChange.IndexOf(match);
				int end = textToChange.IndexOf(match)+match.Length;
				textToChange = textToChange.Substring(0, start+1) + "{" + textToChange.Substring(start+1);
				textToChange = textToChange.Substring(0, end+1) + "}" + textToChange.Substring(end+1);
			}
		}
		if (textToChange.IndexOf ("&gt;") != -1) {
			Debug.Log ("Replacing greater than sign");
			textToChange =  textToChange.Replace ("&gt;", ">");
		}

		if (textToChange.IndexOf ("&lt;") != -1) {
			Debug.Log ("Replacing greater than sign");
			textToChange =  textToChange.Replace ("&lt;", "<");
		}
		if (textToChange.IndexOf ("\\\\") != -1) {
			Debug.Log ("Replacing double backslash");
			textToChange = textToChange.Replace ("\\\\", "\\");
		}
		if(std==6 || std ==7){
			for (var i = 0; i < oldStringLower.Count; i++) {
				textToChange = textToChange.Replace (oldStringLower [i], newStringLower [i]);
			}
		}else if(std==8 || std ==9 || std ==10){
			textToChange = replaceAlternateString(textToChange,"{","}","$$");
			for (var i = 0; i < oldStringUpper.Count; i++) {
				textToChange = textToChange.Replace (oldStringUpper [i], newStringUpper [i]);
			}
		}
		textToChange = textToChange.Replace("\n",System.Environment.NewLine);
		//Debug.Log ("StringWrapper text output" + textToChange);
		return textToChange;

//		if(textToChange.IndexOf ("</br>") != -1){
//			Debug.Log ("Replacing </br>"+textToChange.Replace ("</br>", "\n"));
//		}
	}	
	public static string replaceAlternateString(string CompleteString, string evenReplace,string oddReplace, string Find){
		int CheckOcuuerence = 0;
		string NewString = CompleteString;
		while (NewString.IndexOf(Find) != -1){
			int a = NewString.IndexOf(Find) + Find.Length;
			string first_half = NewString.Substring(0,a);
			string second_half = NewString.Substring(a);

			if (CheckOcuuerence%2==0){
				first_half = first_half.Replace(Find,evenReplace);
			}else{
				first_half = first_half.Replace(Find,oddReplace);
			}
			NewString = first_half + second_half;
			CheckOcuuerence++;
		}
		return NewString;
	}
	public static string HtmlToPlainText(string html)
	{
		const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
		const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
		const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
		var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
		var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
		var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

		var text = html;
		//Decode html specific characters
		text = HttpUtility.HtmlDecode(text); 
		//Remove tag whitespace/line breaks
//		text = tagWhiteSpaceRegex.Replace(text, "><");
		//Replace <br /> with line breaks
//		text = lineBreakRegex.Replace(text, System.Environment.NewLine);
		//Strip formatting
//		text = stripFormattingRegex.Replace(text, string.Empty);
		text = changeString(text);
		return text;
	}
	static public List<string> splitTargetText(string targetText){
		return targetText.Split (';').ToList ();
	}
	static public List<int> splitTargetTextToInt(string targetText){
		return targetText.Split (';').Select(int.Parse).ToList ();
	}
	public static string ConvertToCamelCase(string snakeCase){
		return snakeCase.Split(new [] {"_"}, System.StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
	}
	public static string ConvertToPascalCase(string snakeCase){
		return snakeCase.Split(new [] {"_"}, System.StringSplitOptions.RemoveEmptyEntries).Aggregate(string.Empty, (s1, s2) => s1 + s2);
	}
	public static string HtmlAttrValue(HtmlNode _node, string _attr){
		HtmlAttribute attr_col = _node.Attributes [_attr];
		if (attr_col != null) {
			return (attr_col.Value);
		} else {
			return null;
		}
	}
	public static int HtmlAttrValueInt(HtmlNode _node, string _attr){
		HtmlAttribute attr_col = _node.Attributes [_attr];
		if (attr_col != null) {
			return int.Parse(attr_col.Value);
		} else {
			return 0;
		}
	}
	public static bool HtmlAttrValueBool(HtmlNode _node, string _attr){
		HtmlAttribute attr_col = _node.Attributes [_attr];
		if (attr_col != null) {
			return (attr_col.Value == "0")? false:true;
		} else {
			return false;
		}
	}
	public static List<int> generateRandInRange(int start, int end, int count){
		System.Random rnd = new System.Random();
		int numbers;
		List<int> listNumbers = new List<int>();
		while (listNumbers.Count < count){
			rnd = new System.Random();
			numbers = rnd.Next(start+1,end);
			if(!listNumbers.Contains(numbers)) {
				listNumbers.Add(numbers);
			}
		}
		return listNumbers;
	}

}
