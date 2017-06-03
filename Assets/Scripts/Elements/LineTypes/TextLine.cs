using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLine : Line {
	public Type TextType{get; set;}

	public string DisplayText {get; set;}

	//Contructor
	public TextLine(string displayText, string type){	
		DisplayText = displayText;
		switch (type) {
		case "text": 
			TextType = Type.Text;
			break;
		case "post_submit_text": 
			TextType = Type.PostSubmitText;
			break;
		case "incorrect_submit_text": 
			TextType = Type.IncorrectSubmitText;
			break;
		}
	}
}
