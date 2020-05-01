using UnityEngine;

[ExecuteInEditMode]
public class WorldPoseToGridMaterial : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;

    
    private void Update()
    {
        if (targetMaterial == null)
            return;
        
        targetMaterial.SetVector("_CenterPoint", transform.position);
    }
}