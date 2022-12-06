using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace TranslucentUI
{
	[AddComponentMenu("TranslucentUI/TranslucentUI")]
	//[RequireComponent(typeof(Canvas))]
	public class TranslucentUI : MonoBehaviour 
	{
		//[Range(0f, 1f)]
		//public float Transparency = 0.5f;

		[SerializeField]
		public Camera mainCamera;

		public BlurOption     blurOption = BlurOption.BlurBehindUI;

		public BlurKernelSize kernalSize = BlurKernelSize.Medium;

		[Range(0, 4)]
		public int DownSample = 2;

		[Range(0, 4)]
		public int Iterations = 2;

		[Range(0, 60)]
		public int UpdateFrameRate = 60;


		public bool ApplyOnChildren = false;

		public bool MobileDevice = false;

		[Range(0f, 1f)]
		public float GreyScale = 0.0f;

		[Range(-1f, 1f)]
		public float Brightness = 0.0f;

		private Image[] uiImages;
		private Image uiImage;

		private Shader shader = null;
		private Material translucencyMat = null;
		private TranslucentUICamera translucentUICamera = null;
		private TranslucentUICameraMobile translucentUICameraMobile = null;

		// Use this for initialization
		void Start () 
		{
			
		}

		public void AddTranslucencyComponent()
		{
			if(shader == null)
				shader = Shader.Find ("Custom/Translucency");

			if(translucencyMat)
				translucencyMat = new Material (shader);


			if(ApplyOnChildren)
			{
				AddTranslucencyComponentOnChildren ();
			}
			else
			{
				uiImage = this.GetComponent<Image> ();
				if(uiImage != null)
				{
					Translucency translucency = uiImage.gameObject.GetComponent<Translucency> ();
					if (translucency == null) 
					{
						translucency = uiImage.gameObject.AddComponent<Translucency>();	
						translucency.SetTranslucencyMaterial (translucencyMat);
						translucency.SetGreyScale (GreyScale);
						translucency.SetBrightness (Brightness);
					}
				}
			}


		}

		public void AddTranslucentUICamera()
		{
			if (mainCamera) 
			{
				translucentUICamera = mainCamera.gameObject.GetComponent<TranslucentUICamera> ();
				if (translucentUICamera == null) 
				{
					mainCamera.gameObject.AddComponent<TranslucentUICamera> ();
				}
			}
		}

		public void RemoveTranslucentUICamera()
		{
			if (mainCamera) 
			{
				TranslucentUICamera tuc = mainCamera.gameObject.GetComponent<TranslucentUICamera> ();
				if (tuc != null) 
				{
					DestroyImmediate (tuc);
					translucentUICamera = null;
				}
			}
		}

		public void AddTranslucentUICameraMobile()
		{
			if (mainCamera) 
			{
				translucentUICameraMobile = mainCamera.gameObject.GetComponent<TranslucentUICameraMobile> ();
				if (translucentUICameraMobile == null) 
				{
					mainCamera.gameObject.AddComponent<TranslucentUICameraMobile> ();
				}
			}
		}

		public void RemoveTranslucentUICameraMobile()
		{
			if (mainCamera) 
			{
				TranslucentUICameraMobile tuc = mainCamera.gameObject.GetComponent<TranslucentUICameraMobile> ();
				if (tuc != null) 
				{
					DestroyImmediate (tuc);
					translucentUICameraMobile = null;
				}
			}
		}

		public void AddTranslucencyComponentOnChildren()
		{
			uiImages = this.GetComponentsInChildren<Image> ();
			for (int i = 0; i < uiImages.Length; i++) 
			{
				Translucency translucency = uiImages [i].gameObject.GetComponent<Translucency> ();
				if (translucency == null) 
				{
					translucency = uiImages [i].gameObject.AddComponent<Translucency>();	
					translucency.SetTranslucencyMaterial (translucencyMat);
					translucency.SetGreyScale (GreyScale);
					translucency.SetBrightness (Brightness);
				}
			}
		}

		public void RemoveTranslucentUI()
		{
			foreach (Image image in GetComponentsInChildren<Image>()) 
			{
				Translucency translucency = image.GetComponent<Translucency> ();
				if (translucency != null) 
				{
					DestroyImmediate (translucency);
				}
			}
			RemoveTranslucentUICamera ();
			RemoveTranslucentUICameraMobile();

			TranslucentUI tui = GetComponent<TranslucentUI> ();
			if(tui != null)
			{
				DestroyImmediate (tui);
			}
		}

	
		public void RemoveTranslucencyComponentFromChildren()
		{
			foreach (Image image in GetComponentsInChildren<Image>()) 
			{
				Translucency translucency = image.GetComponent<Translucency> ();
				TranslucentUI translucentUI = image.GetComponent<TranslucentUI>();
				if (translucency != null && translucentUI == null) 
				{
					DestroyImmediate (translucency);
				}
			}
			uiImages = this.GetComponentsInChildren<Image> ();
		}

		public void ApplyCameraProperties()
		{
			if(MobileDevice)
			{
				if(translucentUICameraMobile != null)
				{
					translucentUICameraMobile.blurOption = blurOption;
					translucentUICameraMobile.DownSample = DownSample;
					translucentUICameraMobile.Iterations = Iterations;
					translucentUICameraMobile.UpdateFrameRate = UpdateFrameRate;
				}
			}
			else
			{
				if(translucentUICamera != null)
				{
					translucentUICamera.blurOption = blurOption;
					translucentUICamera.DownSample = DownSample;
					translucentUICamera.Iterations = Iterations;
					translucentUICamera.kernalSize = kernalSize;
					translucentUICamera.UpdateFrameRate = UpdateFrameRate;
				}
			}

		}

		// Update is called once per frame
		void Update () 
		{
			if (ApplyOnChildren == true) 
			{
				for (int i = 0; i < uiImages.Length; i++) 
				{
					Translucency translucency = uiImages [i].gameObject.GetComponent<Translucency>();
					translucency.SetGreyScale (GreyScale);
					translucency.SetBrightness (Brightness);
				}	
			}
			else 
			{
				if(uiImage != null)
				{
					Translucency translucency = uiImage.gameObject.GetComponent<Translucency>();
					translucency.SetGreyScale (GreyScale);
					translucency.SetBrightness (Brightness);	
				}
			}


			if (blurOption == BlurOption.BlurBackground) 
			{
				if(MobileDevice)
				{
					if (translucentUICameraMobile != null) {
						translucentUICameraMobile.Brightness = Brightness;
						translucentUICameraMobile.GreyScale = GreyScale;
					}
				}
				else
				{
					if (translucentUICamera != null) {
						translucentUICamera.Brightness = Brightness;
						translucentUICamera.GreyScale = GreyScale;
					}
				}
			}

		}
	}
}