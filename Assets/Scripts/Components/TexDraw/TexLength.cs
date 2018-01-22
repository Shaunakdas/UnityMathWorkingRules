using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TexDrawLib;

public class TexLength : TEXPerCharacterBase {

	public float m_Count;
	public float m_ApproxCount;

	// This is applied for each character (and commands)
	protected override string Subtitute(string match, float m_Factor)
	{

		if (match.Length == 1) {
			m_Count++;
			resizeTexCell ();
		}
		// In case if you wondering:
		// Debug.Log(match);
		return match;
	}
	public void resizeTexCell(){
		float scale_x = ((float)Screen.width / 480);
		float stdFontSize = scale_x * 35f;
		TEXDrawNGUI texComponent = gameObject.GetComponent<TEXDrawNGUI> ();
		float sizeMultiplier = texComponent.size * (17f/stdFontSize);
		if (texComponent.text.Contains ("\\n")) {
			//Add new Line component
			if(gameObject.GetComponents<TEXSupNewLine>().Length ==0)
				gameObject.AddComponent <TEXSupNewLine>();
		}
		int newLineIndex = texComponent.text.IndexOf ("\\n");
		if ((texComponent.text.Contains ("\\n"))&&(newLineIndex<m_Count)) {
			//Stop extending
		} else {
			int spaceCount = texComponent.text.Count (f => f == ' ');
			texComponent.width = Mathf.Min ((int)(scale_x * sizeMultiplier * (m_Count + spaceCount + 1)), Screen.width - 30);
		}
		gameObject.GetComponentInParent<UITable> ().Reposition ();
	}

	protected override void OnBeforeSubtitution (float count)
	{
//		 This is just an appoximate. For example a command will counted as 1. if you need it.
		m_ApproxCount = count;
		//count is total
		m_Count = 0;
	}
}

