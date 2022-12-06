
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace TranslucentUI
{
	[CustomEditor(typeof(Translucency))]
	[ExecuteInEditMode]
	public class TranslucencyEditor : Editor 
	{
		public override void OnInspectorGUI ()
		{
			GUILayout.Space (10);
		}
	}

}