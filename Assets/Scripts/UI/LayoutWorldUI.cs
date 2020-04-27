using System;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class LayoutWorldUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private UIFadeManager infoImage;
    [SerializeField] private UIFadeManager infoCircle;
    [SerializeField] private Text infoText;
    [SerializeField] private UIFadeManager pointer;

    private bool pointerEnabled;
    private RectTransform imageRect;
    private RectTransform pointerRect;
    private Transform cameraTransform;
    private Transform pointerTargetTransform;
    public Vector2 imageCorner;
    public Vector2 pointerTarget;
    
    

    private void OnEnable()
    {
        imageRect = infoImage.GetComponent<RectTransform>();
        canvas.worldCamera = Camera.main;
        cameraTransform = Camera.main.transform;
        pointer.gameObject.SetActive(pointerEnabled);
        pointerRect = pointer.GetComponent<RectTransform>();
        
        infoImage.gameObject.SetActive(false);
        infoCircle.gameObject.SetActive(false);
        pointer.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Rotate to camera
        transform.forward = cameraTransform.forward;


        if (!pointerEnabled)
            return;
        
        // Calculate reference points for pointer
        // reference point on info image
        float x = imageRect.anchoredPosition.x + imageRect.rect.width * imageRect.localScale.x / 2;
        //x *= transform.localScale.x;
        float y = imageRect.anchoredPosition.y - imageRect.rect.height * imageRect.localScale.y / 2;
        //y *= transform.localScale.y;
        imageCorner = new Vector2(x, y);

        // reference point on target object, relative to camera view
        Vector3 inversedTargetPos = canvas.transform.InverseTransformPoint(pointerTargetTransform.position);// * canvas.transform.localScale.x;
        Vector3 inversedCamPos = canvas.transform.InverseTransformPoint(cameraTransform.position);// * canvas.transform.localScale.x;
        float t = Mathf.Abs(inversedCamPos.z / Mathf.Abs(inversedCamPos.z - inversedTargetPos.z));
        Vector2 flatTargetPos = new Vector2(inversedTargetPos.x, inversedTargetPos.y);
        Vector2 flatCamPos = new Vector2(inversedCamPos.x, inversedCamPos.y);
        Vector2 flatCamToTargetDir = (flatTargetPos - flatCamPos) * t;

        pointerTarget = flatCamPos + flatCamToTargetDir;

        // Set pointer length, position and rotation
        Vector2 l = imageCorner - pointerTarget;
        x = Vector2.Angle(Vector2.left, l);
        y = l.magnitude;
        pointerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, y / pointerRect.localScale.x);

        pointerRect.anchoredPosition = (imageCorner + pointerTarget) / 2;

        pointerRect.localRotation = Quaternion.Euler(0, 0, -x);
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
        
        infoCircle.gameObject.SetActive(true);
        infoCircle.FadeIn();
        infoCircle.GetComponent<Image>().sprite = image;
    }

    public void DisableInfo()
    {
        if (infoImage.gameObject.activeSelf)
            infoImage.FadeOut();
        
        if (infoCircle.gameObject.activeSelf)
            infoCircle.FadeOut();
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
}