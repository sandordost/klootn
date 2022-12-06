using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TranslucentUI
{
	[AddComponentMenu("TranslucentUI/TranslucentUICameraMobile")]
	[RequireComponent(typeof(Camera))]
	public class TranslucentUICameraMobile : MonoBehaviour 
	{
		//private Camera mainCamera = null;
		private RenderTexture blurRT = null;
		private Material cameraBlurMat;
		private float lastCameraUpdateTime;
		private float minCameraUpdateGap;
		private int screenHeight;
		private int screenWidth;

		public BlurOption     blurOption = BlurOption.BlurBackground;

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

		void Awake ()
		{
			cameraBlurMat = new Material (Shader.Find ("Custom/CameraBlurMobile")) as Material;
			__GreyScaleID = Shader.PropertyToID ("__GreyScale");
			__BrightnessID = Shader.PropertyToID ("__Brightness");
			blurRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

			screenHeight = Screen.height;
			screenWidth = Screen.width;

			lastCameraUpdateTime = 0.0f;
		}

		void Update () 
		{
			minCameraUpdateGap = 1.0f / UpdateFrameRate;
			if (blurOption == BlurOption.BlurBackground) {
				cameraBlurMat.SetFloat(__GreyScaleID, GreyScale);
				cameraBlurMat.SetFloat(__BrightnessID, Brightness);
			}
		}

		private void BlurBackground(RenderTexture source, RenderTexture destination)
		{
			RenderTexture tempRT2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			int iteration = 2 * Iterations;
			for (int i = 0; i < iteration; i++)
			{
				float _blurRange = 0.6f;
				float radius = (float)i * _blurRange + _blurRange;
				cameraBlurMat.SetFloat("_Radius", radius);
				Graphics.Blit(source, tempRT2, cameraBlurMat);
				source.DiscardContents();

				if (i == iteration - 1)
				{
					if (blurOption == BlurOption.BlurBehindUI) 
					{
						destination.DiscardContents ();
					}
					Graphics.Blit(tempRT2, destination, cameraBlurMat);
				}
				else
				{
					Graphics.Blit(tempRT2, source, cameraBlurMat);
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