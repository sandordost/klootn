using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace TranslucentUI
{
	[CustomEditor(typeof(TranslucentUI))]
	[ExecuteInEditMode]
	public class TranslucentUIEditor : Editor 
	{
		void Awake()
		{
			TranslucentUI myTarget = (TranslucentUI)target;
			myTarget.AddTranslucencyComponent ();
		}

		void OnEnable()
		{
			TranslucentUI myTarget = (TranslucentUI)target;
			myTarget.AddTranslucencyComponent ();
		}

		public override void OnInspectorGUI ()
		{
			TranslucentUI myTarget = (TranslucentUI)target;
			if (target == null) return;

			GUI.changed = false;

			GUILayout.Space (10);
			myTarget.ApplyOnChildren = EditorGUILayout.Toggle("ApplyOnChildren", myTarget.ApplyOnChildren);
			if (myTarget.ApplyOnChildren == true) 
			{
				myTarget.AddTranslucencyComponentOnChildren ();	
			} 
			else 
			{
				myTarget.RemoveTranslucencyComponentFromChildren ();
			}
			GUILayout.Space (10);
			myTarget.mainCamera = EditorGUILayout.ObjectField("MainCamera", myTarget.mainCamera, typeof(Camera), true) as Camera;
			if (myTarget.mainCamera != null) 
			{
				myTarget.AddTranslucentUICamera ();
			}
			GUILayout.Space (10);
			myTarget.blurOption = (BlurOption)EditorGUILayout.EnumPopup ("BlurOption", myTarget.blurOption);
			GUILayout.Space (10);
			myTarget.MobileDevice = EditorGUILayout.Toggle("MobileDevice", myTarget.MobileDevice);
			if (myTarget.MobileDevice == true) 
			{
				myTarget.RemoveTranslucentUICamera();
				myTarget.AddTranslucentUICameraMobile();
			} 
			else 
			{
				GUILayout.Space (10);
				myTarget.kernalSize = (BlurKernelSize)EditorGUILayout.EnumPopup ("KernalSize", myTarget.kernalSize);
				myTarget.RemoveTranslucentUICameraMobile();
				myTarget.AddTranslucentUICamera();
			}
			GUILayout.Space (10);
			myTarget.DownSample = EditorGUILayout.IntSlider ("DownSample", myTarget.DownSample, 0, 4);
			GUILayout.Space (10);
			myTarget.Iterations = EditorGUILayout.IntSlider ("Iterations", myTarget.Iterations, 0, 4);
			GUILayout.Space (10);
			myTarget.UpdateFrameRate = EditorGUILayout.IntSlider ("UpdateFrameRate", myTarget.UpdateFrameRate, 0, 60);

			GUILayout.Space (10);

			myTarget.GreyScale = EditorGUILayout.Slider("GreyScale", myTarget.GreyScale, 0.0f, 1.0f);
			GUILayout.Space (10);
			myTarget.Brightness = EditorGUILayout.Slider("Brightness", myTarget.Brightness, 0.0f, 1.0f);
			GUILayout.Space (10);



			if (GUI.changed && myTarget != null) 
			{
				EditorUtility.SetDirty (myTarget);
				myTarget.ApplyCameraProperties();
			}

			if(GUILayout.Button("RemoveTranslucentUI"))
			{
				myTarget.RemoveTranslucentUI ();
				GUIUtility.ExitGUI ();
			}
		}
	}
		
}
