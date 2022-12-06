using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace TranslucentUI
{
	[CustomEditor(typeof(TranslucentUICamera))]
	[ExecuteInEditMode]
	public class TranslucentUICameraEditor : Editor 
	{
		public override void OnInspectorGUI ()
		{
			GUILayout.Space (10);
		}
	}

}
