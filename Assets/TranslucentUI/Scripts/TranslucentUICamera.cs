using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TranslucentUI
{
	[AddComponentMenu("TranslucentUI/TranslucentUICamera")]
	//[RequireComponent(typeof(Camera))]
	public class TranslucentUICamera : MonoBehaviour 
	{
		//private Camera mainCamera = null;
		private RenderTexture blurRT = null;
		private Material blurBackgroundMat;
		private float lastCameraUpdateTime;
		private float minCameraUpdateGap;
		private int screenHeight;
		private int screenWidth;

		public BlurOption     blurOption = BlurOption.BlurBehindUI;

		public BlurKernelSize kernalSize = BlurKernelSize.Medium;

		[Range(0, 4)]
		public int DownSample = 2;

		[Range(0, 4)]
		public int Iterations = 2;

		[Range(0, 60)]
		public int UpdateFrameRate = 60;

		[Range(0f, 1f)]
		public float GreyScale = 0.0f;

		[Range(-1f, 1f)]
		public float Brightness = 0.0f;

		private int __GreyScaleID;
		private int __BrightnessID;

		public RenderTexture BlurRT
		{
			get 
			{
				return blurRT;
			}
		}

		void Start () 
		{
			
		}

		void Awake ()
		{
			blurBackgroundMat = new Material (Shader.Find ("Custom/CameraBlur")) as Material;

			__GreyScaleID = Shader.PropertyToID ("__GreyScale");
			__BrightnessID = Shader.PropertyToID ("__Brightness");

			blurRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

			screenHeight = Screen.height;
			screenWidth = Screen.width;

			blurRT.Create ();
			lastCameraUpdateTime = 0.0f;
		}

		void Update () 
		{
			minCameraUpdateGap = 1.0f / UpdateFrameRate;
			if (blurOption == BlurOption.BlurBackground) {
				blurBackgroundMat.SetFloat(__GreyScaleID, GreyScale);
				blurBackgroundMat.SetFloat(__BrightnessID, Brightness);
			}

		}

		private void BlurBackground(RenderTexture source, RenderTexture destination)
		{
			RenderTexture tempRT2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			int kernal = (int)kernalSize;
			for (int i = 0; i < Iterations; i++)
			{
				float _blurRange = 0.6f;
				float radius = (float)i * _blurRange + _blurRange;
				blurBackgroundMat.SetFloat("_Radius", radius);
				Graphics.Blit(source, tempRT2, blurBackgroundMat, kernal + 1);
				source.DiscardContents();

				if (i == Iterations - 1)
				{
					if (blurOption == BlurOption.BlurBehindUI) 
					{
						destination.DiscardContents ();
					}
					Graphics.Blit(tempRT2, destination, blurBackgroundMat, kernal + 2);
				}
				else
				{
					Graphics.Blit(tempRT2, source, blurBackgroundMat, kernal + 2);
					tempRT2.DiscardContents();
				}
			}

			RenderTexture.ReleaseTemporary(tempRT2);
		}

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (screenWidth != Screen.width && screenHeight != Screen.height) 
			{
				screenWidth = Screen.width;
				screenHeight = Screen.height;

				// orientation is changed
				blurRT.height = Screen.height;
				blurRT.width = Screen.width;
			}

			if (blurOption == BlurOption.BlurBackground) 
			{
				if (Iterations == 0) 
				{
					Graphics.Blit (source, destination);
					return;
				}
				int w = Screen.width >> DownSample;
				int h = Screen.height >> DownSample;

				RenderTexture tempRT = RenderTexture.GetTemporary (w, h, 0, source.format);
				Graphics.Blit (source, tempRT);
				BlurBackground (tempRT, destination);
				RenderTexture.ReleaseTemporary (tempRT);
			} 
			else if (blurOption == BlurOption.BlurBehindUI) 
			{
				if (Iterations == 0) 
				{
					Graphics.Blit (source, blurRT);
					Graphics.Blit (source, destination);
					return;
				}
				float now = Time.unscaledTime;
				if (now - lastCameraUpdateTime >= minCameraUpdateGap)
				{
					int w = Screen.width >> DownSample;
					int h = Screen.height >> DownSample;
					RenderTexture tempRT = RenderTexture.GetTemporary (w, h, 0, source.format);
					Graphics.Blit (source, tempRT);
					BlurBackground (tempRT, blurRT);
					RenderTexture.ReleaseTemporary(tempRT);
					lastCameraUpdateTime = Time.unscaledTime;
				}

				Graphics.Blit (source, destination);
			}
		}
			
	}
}