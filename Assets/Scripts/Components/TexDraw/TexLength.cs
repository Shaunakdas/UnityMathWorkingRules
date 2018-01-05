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
		int spaceCount = gameObject.GetComponent<TEXDrawNGUI> ().text.Count(f => f == ' ');
		gameObject.GetComponent<TEXDrawNGUI> ().width = (int)(scale_x * 20f * (m_Count + spaceCount));
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

