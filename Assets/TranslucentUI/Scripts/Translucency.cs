using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TranslucentUI
{
	[AddComponentMenu("TranslucentUI/Translucency")]
	[RequireComponent(typeof(Image))]
	public class Translucency : MonoBehaviour
	{
		private Image image = null;
		private Material translucencyMat = null;
		private TranslucentUICamera translucentUICamera = null;
		private TranslucentUICameraMobile translucentUICameraMobile = null;

		[Range(0f, 1f)]
		public float Transparency = 0.5f;

		[Range(0f, 1f)]
		public float GreyScale = 0.0f;

		[Range(-1f, 1f)]
		public float Brightness = 0.0f;

		private int _BlurTexID;
		private int _GreyScaleID;
		private int _BrightnessID;
		void Start()
		{
			translucentUICamera = FindObjectOfType<TranslucentUICamera> ();
			if (translucentUICamera != null) 
			{
				image = this.GetComponent<Image> ();
				if (translucentUICamera.blurOption == BlurOption.BlurBehindUI) 
				{
					if (translucencyMat != null) 
					{
						image.material = translucencyMat;		
					} 
					else 
					{
						Shader translucentImage = Shader.Find ("Custom/Translucency");
						Material translucentMat = new Material (translucentImage);
						image.material = translucentMat;
					}
					_BlurTexID = Shader.PropertyToID ("_BlurTex");
					_GreyScaleID = Shader.PropertyToID ("_GreyScale");
					_BrightnessID = Shader.PropertyToID ("_Brightness");
				}	
			} 
			else 
			{
				translucentUICameraMobile = FindObjectOfType<TranslucentUICameraMobile> ();
				if (translucentUICameraMobile != null) 
				{
					image = this.GetComponent<Image> ();
					if (translucentUICameraMobile.blurOption == BlurOption.BlurBehindUI) 
					{
						if (translucencyMat != null) 
						{
							image.material = translucencyMat;		
						} 
						else 
						{
							Shader translucentImage = Shader.Find ("Custom/Translucency");
							Material translucentMat = new Material (translucentImage);
							image.material = translucentMat;
						}
						_BlurTexID = Shader.PropertyToID ("_BlurTex");
						_GreyScaleID = Shader.PropertyToID ("_GreyScale");
						_BrightnessID = Shader.PropertyToID ("_Brightness");
					}	
				} 
			}
			if (image) 
			{
				Color color = image.color;
				Transparency = 1 - color.a;
			}	

		}

		public void SetTranslucencyMaterial(Material mat)
		{
			translucencyMat = mat;
		}

		/*public void SetTransparency(float transparency)
		{
			Transparency = transparency;
		}*/

		public void SetGreyScale(float greyScale)
		{
			GreyScale = greyScale;
		}

		public void SetBrightness(float brightness)
		{
			Brightness = brightness;
		}

		void LateUpdate()
		{
			if (translucentUICamera && translucentUICamera.blurOption == BlurOption.BlurBehindUI) 
			{
				if (translucentUICamera.BlurRT != null) 
				{
					image.materialForRendering.SetTexture (_BlurTexID, translucentUICamera.BlurRT);
					//image.material.SetTexture (_BlurTexID, translucentUICamera.BlurRT);
					image.material.SetFloat(_GreyScaleID, GreyScale);
					image.material.SetFloat(_BrightnessID, Brightness);
				}
			} 
			else if (translucentUICameraMobile && translucentUICameraMobile.blurOption == BlurOption.BlurBehindUI) 
			{
				if (translucentUICameraMobile.BlurRT != null) 
				{
					image.materialForRendering.SetTexture (_BlurTexID, translucentUICameraMobile.BlurRT);
					//image.material.SetTexture (_BlurTexID, translucentUICameraMobile.BlurRT);
					image.material.SetFloat(_GreyScaleID, GreyScale);
					image.material.SetFloat(_BrightnessID, Brightness);
				}
			}
			if (image) 
			{
				Color color = image.color;
				color.a = 1.0f - Transparency;
				image.color = color;
			}

		}

	}
}
