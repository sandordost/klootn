using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TranslucentUI
{
	public class Rotate : MonoBehaviour {

		private Vector3 eulerAngles;

		[Range(1f, 5f)]
		public float RotationSpeed = 3.0f;
		// Use this for initialization
		void Start () 
		{
			eulerAngles = transform.eulerAngles;
		}

		// Update is called once per frame
		void Update () 
		{
			eulerAngles.y += 10 * RotationSpeed * Time.deltaTime;
			transform.eulerAngles = eulerAngles;
		}
	}

}
