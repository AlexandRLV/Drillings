using System;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class LayoutWorldUI : MonoBehaviour
{
    [SerializeField] private float sizeTextOffset;
    
    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private ArrowsManager arrowsManager;
    [SerializeField] private GameObject arrowsParent;
    [SerializeField] private UIFadeManager infoImage;
    [SerializeField] private UIFadeManager infoCircle;
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
    private bool arrowsEnabled;
    private RectTransform imageRect;
    private RectTransform pointerRect;
    private RectTransform hLTextRect;
    private RectTransform hsTextRect;
    private RectTransform vTextRect;
    private Transform cameraTransform;
    private Transform pointerTargetTransform;
    
    

    private void OnEnable()
    {
        imageRect = infoImage.GetComponent<RectTransform>();
        canvas.worldCamera = Camera.main;
        cameraTransform = Camera.main.transform;
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


        if (pointerEnabled)
        {
            // Calculate reference points for pointer
            // reference point on info image
            float x = imageRect.anchoredPosition.x + imageRect.rect.width * imageRect.localScale.x / 2;
            //x *= transform.localScale.x;
            float y = imageRect.anchoredPosition.y - imageRect.rect.height * imageRect.localScale.y / 2;
            //y *= transform.localScale.y;
            Vector2 imageCorner = new Vector2(x, y);

            // reference point on target object, relative to camera view
            Vector2 pointerTarget = GetReferencePoint(pointerTargetTransform.position);

            // Set pointer length, position and rotation
            SetUpRect(pointerRect, imageCorner, pointerTarget);
        }

        if (arrowsEnabled)
        {
            arrowsManager.CalculateArrowPoints();
            
            // set up long arrow
            Vector2 arrowPoint1 = GetReferencePoint(arrowsManager.LongArrowPoint1);
            Vector2 arrowPoint2 = GetReferencePoint(arrowsManager.LongArrowPoint2);
            SetUpRect(horizontalLongArrow, arrowPoint1, arrowPoint2);
            SetUpTextRect(hLTextRect, horizontalLongArrow);
            
            // set up short arrow
            arrowPoint1 = GetReferencePoint(arrowsManager.ShortArrowPoint1);
            arrowPoint2 = GetReferencePoint(arrowsManager.ShortArrowPoint2);
            SetUpRect(horizontalShortArrow, arrowPoint1, arrowPoint2);
            SetUpTextRect(hsTextRect, horizontalShortArrow);
            
            // set up vertical arrow
            arrowPoint1 = GetReferencePoint(arrowsManager.VertArrowPoint1);
            arrowPoint2 = GetReferencePoint(arrowsManager.VertArrowPoint2);
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

    public void EnableArrows(string longSize, string shortSize, string vertSize)
    {
        arrowsEnabled = true;
        arrowsParent.SetActive(true);
        horizontalLongText.text = longSize;
        horizontalShortText.text = shortSize;
        verticalText.text = vertSize;
    }

    public void DisableArrows()
    {
        arrowsEnabled = false;
        arrowsParent.SetActive(false);
    }


    private void SetUpRect(RectTransform rect, Vector2 point1, Vector2 point2)
    {
        Vector2 l = point1 - point2;
        float x = Vector2.Angle(Vector2.left, l);
        x = l.y < 0 ? x : -x;
        float y = l.magnitude;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, y / pointerRect.localScale.x);

        rect.anchoredPosition = (point1 + point2) / 2;

        rect.localRotation = Quaternion.Euler(0, 0, x);
    }

    private void SetUpTextRect(RectTransform textRext, RectTransform arrowRect)
    {
        float a = arrowRect.localRotation.eulerAngles.z;
        float x = sizeTextOffset * Mathf.Sin(a * Mathf.Deg2Rad);
        x = arrowRect.anchoredPosition.x > 0 ? Mathf.Abs(x) : -Mathf.Abs(x);
        float y = sizeTextOffset * Mathf.Cos(a * Mathf.Deg2Rad);
        textRext.anchoredPosition = arrowRect.anchoredPosition + new Vector2(x, -y);
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