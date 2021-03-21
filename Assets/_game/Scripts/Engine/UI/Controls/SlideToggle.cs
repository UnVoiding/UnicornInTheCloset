using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
	// original code from https://github.com/Kalxoznik/Unity-Toggle-controller
	public class SlideToggle : MonoBehaviour 
	{
		public  bool isOn;
		public Action<bool> onValueChanged;

		public Color onColorBg;
		public Color offColorBg;

		public Image toggleBgImage;
		public RectTransform toggle;

		public GameObject handle;
		private RectTransform handleTransform;

		private float handleSize;
		private float onPosX;
		private float offPosX;

		public float handleOffset;

		public CanvasGroup onIcon;
		public CanvasGroup offIcon;

		public float speed;
		static float t = 0.0f;

		private bool switching = false;

		void Awake()
		{
			handleTransform = handle.GetComponent<RectTransform>();
			RectTransform handleRect = handle.GetComponent<RectTransform>();
			handleSize = handleRect.sizeDelta.x;
			float toggleSizeX = toggle.sizeDelta.x;
			onPosX = (toggleSizeX / 2) - (handleSize/2) - handleOffset;
			offPosX = onPosX * -1;
		}

		void Start()
		{
			if (isOn)
			{
				toggleBgImage.color = onColorBg;
				handleTransform.localPosition = new Vector3(onPosX, 0f, 0f);
				onIcon.gameObject.SetActive(true);
				offIcon.gameObject.SetActive(false);
			}
			else
			{
				toggleBgImage.color = offColorBg;
				handleTransform.localPosition = new Vector3(offPosX, 0f, 0f);
				onIcon.gameObject.SetActive(false);
				offIcon.gameObject.SetActive(true);
			}
		}
			
		void Update()
		{
			if (switching)
			{
				Toggle(isOn);
			}
		}

		public void DoYourStaff()
		{
			
		}

		public void Switching()
		{
			switching = true;
		}

		public void Toggle(bool toggleStatus)
		{
			if (!onIcon.gameObject.activeInHierarchy || !offIcon.gameObject.activeInHierarchy)
			{
				onIcon.gameObject.SetActive(true);
				offIcon.gameObject.SetActive(true);
			}
			
			if (toggleStatus)
			{
				toggleBgImage.color = SmoothColor(onColorBg, offColorBg);
				Transparency (onIcon.gameObject, 1f, 0f);
				Transparency (offIcon.gameObject, 0f, 1f);
				handleTransform.localPosition = SmoothMove(handle, onPosX, offPosX);
			}
			else 
			{
				toggleBgImage.color = SmoothColor(offColorBg, onColorBg);
				Transparency (onIcon.gameObject, 0f, 1f);
				Transparency (offIcon.gameObject, 1f, 0f);
				handleTransform.localPosition = SmoothMove(handle, offPosX, onPosX);
			}
		}

		Vector3 SmoothMove(GameObject toggleHandle, float startPosX, float endPosX)
		{
			Vector3 position = new Vector3 (Mathf.Lerp(startPosX, endPosX, t += speed * Time.deltaTime), 0f, 0f);
			StopSwitching();
			return position;
		}

		Color SmoothColor(Color startCol, Color endCol)
		{
			Color resultCol;
			resultCol = Color.Lerp(startCol, endCol, t += speed * Time.deltaTime);
			return resultCol;
		}

		CanvasGroup Transparency (GameObject alphaObj, float startAlpha, float endAlpha)
		{
			CanvasGroup alphaVal;
			alphaVal = alphaObj.gameObject.GetComponent<CanvasGroup>();
			alphaVal.alpha = Mathf.Lerp(startAlpha, endAlpha, t += speed * Time.deltaTime);
			return alphaVal;
		}

		void StopSwitching()
		{
			if (t > 1.0f)
			{
				switching = false;

				t = 0.0f;
				switch(isOn)
				{
				case true:
					isOn = false;
					onValueChanged?.Invoke(isOn);
					break;

				case false:
					isOn = true;
					onValueChanged?.Invoke(isOn);
					break;
				}
			}
		}
	}
}
