using UnityEngine;
using System.Collections;
using Rezero;

namespace Rezero
{
	public class SkillTime : MonoBehaviour {

		private Character character;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void PauseTime()
		{
			Time.timeScale = 0f;
		}

		public void ResumeTime()
		{
			Time.timeScale = 1.0f;
			this.gameObject.SetActive (false);
			GameObject Player = GameObject.FindGameObjectWithTag("Player");
			if (Player != null)
				character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
			character.UltiShoot();
		}
	}
}