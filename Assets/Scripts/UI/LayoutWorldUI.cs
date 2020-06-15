﻿using AssetVariables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
	public class LayoutWorldUI : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private FloatVariable sizeTextOffset;
		[SerializeField] private FloatVariable maxCanvasDistance;
		[SerializeField] private FloatVariable minCanvasDistance;
		[SerializeField] private FloatVariable canvasToCameraDistanceMultiplier;
    
		[Header("References")]
		[SerializeField] private Canvas canvas;
		[SerializeField] private ArrowsManager arrowsManager;
		[SerializeField] private GameObject arrowsParent;
		[SerializeField] private UIFadeManager infoImage;
		[SerializeField] private UIFadeManager infoCircle;
		[SerializeField] private UIFadeManager photoImageFadeManager;
		[SerializeField] private Image photoImage;
		[SerializeField] private UIFadeManager videoPlayer;
		[SerializeField] private Text infoText;
		[SerializeField] private UIFadeManager pointer;

		[Header("SizeArrows")] 
		[SerializeField] private RectTransform horizontalLongArrow;
		[SerializeField] private RectTransform horizontalShortArrow;
		[SerializeField] private RectTransform verticalArrow;
		[SerializeField] private Text horizontalLongText;
		[SerializeField] private Text horizontalShortText;
		[SerializeField] private Text verticalText;

		private bool pointerEnabled;
		private bool longArrowEnabled;
		private bool shortArrowEnabled;
		private bool vertArrowEnabled;
		private float sizeArrowsVertOffset;
		private RectTransform imageRect;
		private RectTransform circleRect;
		private RectTransform pointerRect;
		private RectTransform hLTextRect;
		private RectTransform hsTextRect;
		private RectTransform vTextRect;
		private Transform cameraTransform;
		private Transform canvasTransform;
		private Transform pointerTargetTransform;
		private VideoPlayer currentVideoPlayer;
    
    

		private void OnEnable()
		{
			imageRect = infoImage.GetComponent<RectTransform>();
			circleRect = infoCircle.GetComponent<RectTransform>();
			canvas.worldCamera = Camera.main;
			cameraTransform = Camera.main.transform;
			canvasTransform = canvas.transform;
			pointer.gameObject.SetActive(pointerEnabled);
			pointerRect = pointer.GetComponent<RectTransform>();
			hLTextRect = horizontalLongText.GetComponent<RectTransform>();
			hsTextRect = horizontalShortText.GetComponent<RectTransform>();
			vTextRect = verticalText.GetComponent<RectTransform>();
        
			infoImage.gameObject.SetActive(false);
			infoCircle.gameObject.SetActive(false);
			pointer.gameObject.SetActive(false);
		}

		private void Update()
		{
			// Rotate to camera
			transform.forward = cameraTransform.forward;
			
			// Calculate canvas distance
			float canvasDistance = (cameraTransform.position - transform.position).magnitude;
			canvasDistance *= canvasToCameraDistanceMultiplier;
			if (canvasDistance > maxCanvasDistance)
				canvasDistance = maxCanvasDistance;
			else if (canvasDistance < minCanvasDistance)
				canvasDistance = minCanvasDistance;
			
			// Apply canvas distance
			canvasTransform.position = transform.position - (transform.forward * canvasDistance);
			
			

			if (pointerEnabled)
			{
				// Calculate reference points for pointer
				// reference point on target object, relative to camera view
				Vector2 pointerTarget = GetReferencePoint(pointerTargetTransform.position);
				
				// reference point on info image
				float x, y;
				if (imageRect.gameObject.activeSelf)
				{
					x = imageRect.anchoredPosition.x + imageRect.rect.width * imageRect.localScale.x / 2;
					y = imageRect.anchoredPosition.y - imageRect.rect.height * imageRect.localScale.y / 2;
				}
				else
				{
					x = circleRect.rect.width * circleRect.localScale.x / 2;
					y = 0;
					
					Vector2 diff = pointerTarget - circleRect.anchoredPosition;

					float angle = Vector2.Angle(Vector2.right, diff);
					if (pointerTarget.y < circleRect.anchoredPosition.y)
						angle = -angle;
					
					float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
					float sin = Mathf.Sin(angle * Mathf.Deg2Rad);

					float rx = x * cos * 1.05f;
					float ry = x * sin * 1.05f;

					x = rx + circleRect.anchoredPosition.x;
					y = ry + circleRect.anchoredPosition.y;
				}
				Vector2 imageCorner = new Vector2(x, y);
				
				// Set pointer length, position and rotation
				SetUpRect(pointerRect, imageCorner, pointerTarget);
			}

			arrowsManager.CalculateArrowPoints();
		
			if (longArrowEnabled)
			{            
				// set up long arrow
				Vector2 arrowPoint1 = GetReferencePoint(arrowsManager.LongArrowPoint1) + Vector2.up * sizeArrowsVertOffset;
				Vector2 arrowPoint2 = GetReferencePoint(arrowsManager.LongArrowPoint2) + Vector2.up * sizeArrowsVertOffset;
				SetUpRect(horizontalLongArrow, arrowPoint1, arrowPoint2);
				SetUpTextRect(hLTextRect, horizontalLongArrow);
			}
		
			if (shortArrowEnabled)
			{
				// set up short arrow
				Vector2 arrowPoint1 = GetReferencePoint(arrowsManager.ShortArrowPoint1) + Vector2.up * sizeArrowsVertOffset;
				Vector2 arrowPoint2 = GetReferencePoint(arrowsManager.ShortArrowPoint2) + Vector2.up * sizeArrowsVertOffset;
				SetUpRect(horizontalShortArrow, arrowPoint1, arrowPoint2);
				SetUpTextRect(hsTextRect, horizontalShortArrow);
			}
		
			if (vertArrowEnabled)
			{
				// set up vertical arrow
				Vector2 arrowPoint1 = GetReferencePoint(arrowsManager.VertArrowPoint1) + Vector2.up * sizeArrowsVertOffset;
				Vector2 arrowPoint2 = GetReferencePoint(arrowsManager.VertArrowPoint2) + Vector2.up * sizeArrowsVertOffset;
				SetUpRect(verticalArrow, arrowPoint1, arrowPoint2);
				SetUpTextRect(vTextRect, verticalArrow);
			}
		}


    
		public void SetUpText(string text)
		{
			if (infoCircle.gameObject.activeSelf)
				infoCircle.FadeOut();
        
			if (string.IsNullOrWhiteSpace(text))
			{
				if (infoImage.gameObject.activeSelf)
					infoImage.FadeOut();
			}
			else
			{
				infoImage.gameObject.SetActive(true);
				infoImage.FadeIn();
				infoText.text = text;
			}
		}

		public void SetUpImage(Sprite image)
		{
			if (infoImage.gameObject.activeSelf)
				infoImage.FadeOut();
			
			if (photoImageFadeManager.gameObject.activeSelf)
				photoImageFadeManager.FadeOut();
        
			infoCircle.gameObject.SetActive(true);
			infoCircle.FadeIn();
			infoCircle.GetComponent<Image>().sprite = image;
		}

		public void SetUpPhoto(Sprite photo, float aspectRatio)
		{
			photoImageFadeManager.gameObject.SetActive(true);
			photoImageFadeManager.FadeIn();
			photoImage.sprite = photo;

			WorldImageSizeController imageSize = photoImageFadeManager.GetComponent<WorldImageSizeController>();
			imageSize.HeightMultiplier = aspectRatio;
			imageSize.FlipX = infoCircle.gameObject.activeSelf;
		}

		public void SetUpVideo(int clipId, float aspectRatio, RenderTexture renderTexture)
		{
			videoPlayer.gameObject.SetActive(true);
			videoPlayer.FadeIn();

			currentVideoPlayer = VideoHolder.Instance.players[clipId];
			videoPlayer.GetComponentInChildren<RawImage>().texture = renderTexture;
			currentVideoPlayer.targetTexture = renderTexture;
			currentVideoPlayer.Play();
			
			// videoPlayer.GetComponentInChildren<RawImage>().texture = renderTexture;
			// videoPlayer.GetComponent<VideoPlayer>().targetTexture = renderTexture;
			// videoPlayer.GetComponent<VideoPlayer>().clip = VideoHolder.Instance.clips[clipId];
			// videoPlayer.GetComponent<VideoPlayer>().Play();
			
			WorldImageSizeController imageSize = videoPlayer.GetComponent<WorldImageSizeController>();
			imageSize.HeightMultiplier = aspectRatio;
			imageSize.FlipX = infoCircle.gameObject.activeSelf;
		}

		public void DisableInfo()
		{
			if (infoImage.gameObject.activeSelf)
				infoImage.FadeOut();
        
			if (infoCircle.gameObject.activeSelf)
				infoCircle.FadeOut();
			
			if (photoImageFadeManager.gameObject.activeSelf)
				photoImageFadeManager.FadeOut();
			
			if (videoPlayer.gameObject.activeSelf)
				videoPlayer.FadeOut();
			
			if (currentVideoPlayer != null)
				currentVideoPlayer.Stop();
		}

		public void EnablePointer(Transform target)
		{
			pointerEnabled = true;
			pointerTargetTransform = target;
			pointer.gameObject.SetActive(true);
			pointer.FadeIn();
		}

		public void DisablePointer()
		{
			pointerEnabled = false;
			if (pointer.gameObject.activeSelf)
				pointer.FadeOut();
		}

		public void EnableArrows(string longSize, string shortSize, string vertSize, float vertOffset)
		{
			if (!string.IsNullOrWhiteSpace(longSize))
			{
				longArrowEnabled = true;
				horizontalLongText.text = longSize;
				horizontalLongText.gameObject.SetActive(true);
				horizontalLongArrow.gameObject.SetActive(true);
			}
			else
			{
				longArrowEnabled = false;
				horizontalLongText.gameObject.SetActive(false);
				horizontalLongArrow.gameObject.SetActive(false);
			}
			if (!string.IsNullOrWhiteSpace(shortSize))
			{
				shortArrowEnabled = true;
				horizontalShortText.text = shortSize;
				horizontalShortText.gameObject.SetActive(true);
				horizontalShortArrow.gameObject.SetActive(true);
			}
			else
			{
				shortArrowEnabled = false;
				horizontalShortText.gameObject.SetActive(false);
				horizontalShortArrow.gameObject.SetActive(false);
			}
			if (!string.IsNullOrWhiteSpace(vertSize))
			{
				vertArrowEnabled = true;
				verticalText.text = vertSize;
				verticalText.gameObject.SetActive(true);
				verticalArrow.gameObject.SetActive(true);
			}
			else
			{
				vertArrowEnabled = false;
				verticalText.gameObject.SetActive(false);
				verticalArrow.gameObject.SetActive(false);
			}
		
			sizeArrowsVertOffset = vertOffset;
		
			arrowsParent.SetActive(true);
		}

		public void DisableArrows()
		{
			vertArrowEnabled = false;
			shortArrowEnabled = false;
			longArrowEnabled = false;
			arrowsParent.SetActive(false);
		}


		
		private void SetUpRect(RectTransform rect, Vector2 point1, Vector2 point2)
		{
			Vector2 l = point1 - point2;
			
			float x = Vector2.Angle(Vector2.left, l);
			
			x = l.y < 0 ? x : -x;
			// if (x > 90)
			// 	x -= 180;
			// else if (x < -90)
			// 	x += 180;
			
			float y = l.magnitude;

			rect.localRotation = Quaternion.identity;
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, y / rect.localScale.x);
			rect.anchoredPosition = (point1 + point2) / 2;
			rect.localRotation = Quaternion.Euler(0, 0, x);
		}

		private void SetUpTextRect(RectTransform textRext, RectTransform arrowRect)
		{
			float a = arrowRect.localRotation.eulerAngles.z;
			a = a > 180 ? a - 360 : a;
			float x = sizeTextOffset * Mathf.Sin(a * Mathf.Deg2Rad);
			x = arrowRect.anchoredPosition.x > 0 ? Mathf.Abs(x) : -Mathf.Abs(x);
			float y = sizeTextOffset * Mathf.Cos(a * Mathf.Deg2Rad);
			textRext.anchoredPosition = arrowRect.anchoredPosition + new Vector2(x, -y);
        
			if (a < -60)
			{
				a += 90;
			}
			else if (a > 60)
			{
				a -= 90;
			}
			textRext.localRotation = Quaternion.Euler(0, 0, a);
		}

		private Vector2 GetReferencePoint(Vector3 worldPoint)
		{
			Vector3 inversedTargetPos = canvas.transform.InverseTransformPoint(worldPoint);// * canvas.transform.localScale.x;
			Vector3 inversedCamPos = canvas.transform.InverseTransformPoint(cameraTransform.position);// * canvas.transform.localScale.x;
			float t = Mathf.Abs(inversedCamPos.z / Mathf.Abs(inversedCamPos.z - inversedTargetPos.z));
			Vector2 flatTargetPos = new Vector2(inversedTargetPos.x, inversedTargetPos.y);
			Vector2 flatCamPos = new Vector2(inversedCamPos.x, inversedCamPos.y);
			Vector2 flatCamToTargetDir = (flatTargetPos - flatCamPos) * t;

			return flatCamPos + flatCamToTargetDir;
		}
	}
}