using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HtmlAgilityPack;


/// <summary>
/// File to get question text from files or from UserPreferences if android
/// </summary>
public static class QuestionImporter  {
	//To check where questions should be imported from
	public enum ImportSettings {File,Android,Trial};
	public static ImportSettings importMode{get; set;}
	//User inputted standard for Testing
	public static int inputStandard{ get; set; }
	//User inputted chapter for Testing
	public static int inputChapter{ get; set; }
	//While Complete Testing, index of question currently being tested
	public static int questionArrIndex{ get; set; }
	//While Complete Testing, Arrays of Game screenplays based on user input and QuestionSource folder.
	static ArrayList questionArr = new ArrayList();

	static void setUserInput(){
		//Default Values
		importMode = ImportSettings.Trial;
		inputStandard = -1;
		inputChapter = -1;
		questionArrIndex = -1;
		//User Input Values
		importMode = ImportSettings.File;
		inputStandard = 6;
		inputChapter = -1;
		questionArrIndex = -1;
	}
	public static void getQuestionArray(){
		setUserInput();
		if ((inputStandard != -1)&&(inputChapter != -1)){
			string file = "QuestionSource/screenplays/standard_"+inputStandard+"/chapter_"+inputChapter+".html";
			loadFromFile (file);
		} else if (inputStandard != -1){
			string folder = "QuestionSource/screenplays/standard_"+inputStandard+"/";
			loadFromFolder(folder);
			//Go through the folder, make an array of chapter files
			//Go through each chapter files, add all html to questionArr
		} else {
//			GetFileStructure();
			string parent = "QuestionSource/screenplays/";
			//For each folder: folder+standard_i
			foreach (string folder in Directory.GetDirectories(parent)) {
				Debug.Log ("Folder"+folder);
				loadFromFolder(folder);
			}
		}
	}
	/// <summary>
	/// Loads from html.
	/// </summary>
	/// <param name="parentNode">Parent node.</param>
	static void loadFromHtml(string parentNode){
		var html = new HtmlDocument ();
		html.LoadHtml (parentNode);
		Debug.Log (html.DocumentNode.SelectNodes("//body")[0].OuterHtml);
		// For each questionHtml
		foreach (HtmlNode body in html.DocumentNode.SelectNodes("//body")) {
			questionArr.Add (body.OuterHtml);
		}
	}
	/// <summary>
	/// Loads from file.
	/// </summary>
	/// <param name="file">File.</param>
	static void loadFromFile(string file){
		if ((file.Contains ("chapter_"))&&(!file.Contains ("meta"))) {
			Debug.Log ("File2"+file);
			loadFromHtml(System.IO.File.ReadAllText (@file));
		}
	}
	/// <summary>
	/// Loads from folder.
	/// </summary>
	/// <param name="folder">Folder.</param>
	static void loadFromFolder(string folder){
		if (folder.Contains ("standard_")) {
			//For each file chapter_j
			foreach (string file in Directory.GetFiles(folder)) {
				loadFromFile (file);
			}
		}
	}
	public static string getNextQuestion(){
		if (importMode == ImportSettings.File){
			if (questionArr.Count == 0){
				getQuestionArray();
				return questionArr[0].ToString();
			} else {
				questionArrIndex ++;
				return questionArr[questionArrIndex].ToString();
			}
		} else {
			//Check for Android or editor and hence set default QuestionText
		}
		return "";
	}
}
