using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

namespace TexDrawLib
{
	[AddComponentMenu("TEXDraw/Supplemets/TEXSup Predefined Symbols")]
	[TEXSupHelpTip("A Quick Find-Replace tool for Making Customized Symbols")]
	public class TEXSupPredefinedSymbols : TEXDrawSupplementBase
    {
        const string f = @"\\n[(?=\W)|\s]";
        const string t = "\n";

        public string[] m_Find;
        public string[] m_Replace;
        
        public override string ReplaceString(string original)
        {
            //This will recognize \n as new line
            if (m_Find.Length > m_Replace.Length)
                return original;

            m_builder.Length = 0;
            m_builder.Append(original);
            var length = m_Find.Length;
            for (int i = 0; i < length; i++)
            {
                
            }
            return Regex.Replace(original, f, t);
        }

        StringBuilder m_builder;
        protected override void OnEnable () {
            m_builder = new StringBuilder();
            base.OnEnable();
        }
    }
}