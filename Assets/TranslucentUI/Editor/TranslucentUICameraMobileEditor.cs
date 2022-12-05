using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace TranslucentUI
{
	[CustomEditor(typeof(TranslucentUICameraMobile))]
	[ExecuteInEditMode]
	public class TranslucentUICameraMobileEditor : Editor 
	{
		public override void OnInspectorGUI ()
		{
			GUILayout.Space (10);
		}
	}

}
