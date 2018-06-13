using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TexDrawLib;

public class TexLength : TEXPerCharacterBase {

	public float m_Count;
	public float m_ApproxCount;
	public int newLineIndex = -1;

	// This is applied for each character (and commands)
	protected override string Subtitute(string match, float m_Factor)
	{

		if (match == ("\\n")){
			newLineIndex = (int)m_Count;
		}
		if (match.Length == 1) {
			m_Count++;
			resizeTexCell ();
		}
		// In case if you wondering:
		// Debug.Log(match);
		return match;
	}
	public void resizeTexCell(){
		float scale_x = ((float)ScreenManager.ScreenWidth() / 480);
		float stdFontSize = scale_x * 35f;
		TEXDrawNGUI texComponent = gameObject.GetComponent<TEXDrawNGUI> ();
		float sizeMultiplier = texComponent.size * (19f/stdFontSize);
		if (texComponent.text.Contains ("\\n")) {
			//Add new Line component
			if(gameObject.GetComponents<TEXSupNewLine>().Length ==0)
				gameObject.AddComponent <TEXSupNewLine>();
		}

		float maxCount = Mathf.Max (m_ApproxCount,m_Count);
		if (texComponent.text.Contains("\\n")){
			maxCount = m_Count;
		}
		if ((newLineIndex>0)&&(newLineIndex<m_Count)) {
			//Stop extending
		} else {
			int spaceCount = texComponent.text.Count (f => f == ' ');
			texComponent.width = Mathf.Min ((int)(scale_x * sizeMultiplier * (maxCount + spaceCount + 1)), ScreenManager.ScreenWidth() - 30);
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

