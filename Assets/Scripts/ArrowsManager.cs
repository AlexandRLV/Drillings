using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowsManager : MonoBehaviour
{
	public Vector3 LongArrowPoint1 { get; private set; }
	public Vector3 LongArrowPoint2 { get; private set; }
	public Vector3 ShortArrowPoint1 { get; private set; }
	public Vector3 ShortArrowPoint2 { get; private set; }
	public Vector3 VertArrowPoint1 { get; private set; }
	public Vector3 VertArrowPoint2 { get; private set; }

	[SerializeField] private float vertArrowLength;
	[SerializeField] private Transform[] referencePoints;
	
	private Transform camTransform;
	
	
	
	private void OnEnable()
	{
		camTransform = Camera.main.transform;
	}



	public void CalculateArrowPoints()
	{
		VertArrowPoint1 = GetVerticalPoint();
		VertArrowPoint2 = VertArrowPoint1 + Vector3.up * vertArrowLength;

		int longArrowId = GetHorizontalLongPointId();
		int arrowId = longArrowId - 1;
		if (arrowId < 0)
			arrowId = referencePoints.Length - 1;
		LongArrowPoint1 = referencePoints[arrowId].position;
		arrowId = longArrowId + 1;
		if (arrowId >= referencePoints.Length)
			arrowId = 0;
		LongArrowPoint2 = referencePoints[arrowId].position;

		int shortArrowId = GetHorizontalShortPointId();
		arrowId = shortArrowId - 1;
		if (arrowId < 0)
			arrowId = referencePoints.Length - 1;
		ShortArrowPoint1 = referencePoints[arrowId].position;
		arrowId = shortArrowId + 1;
		if (arrowId >= referencePoints.Length)
			arrowId = 0;
		ShortArrowPoint2 = referencePoints[arrowId].position;
	}



	private Vector3 GetVerticalPoint()
	{
		List<Vector3> vertPoints = new List<Vector3>();
		
		for (int i = 0; i < referencePoints.Length; i+= 2)
		{
			vertPoints.Add(referencePoints[i].position);
		}
		
		vertPoints = vertPoints.OrderBy(x => (x - camTransform.position).sqrMagnitude).ToList();
		
		return vertPoints[1];
	}

	private int GetHorizontalLongPointId()
	{
		float len1 = (referencePoints[1].position - camTransform.position).sqrMagnitude;
		float len2 = (referencePoints[5].position - camTransform.position).sqrMagnitude;

		return len1 < len2 ? 1 : 5;
	}

	private int GetHorizontalShortPointId()
	{
		float len1 = (referencePoints[3].position - camTransform.position).sqrMagnitude;
		float len2 = (referencePoints[7].position - camTransform.position).sqrMagnitude;

		return len1 < len2 ? 3 : 7;
	}
}