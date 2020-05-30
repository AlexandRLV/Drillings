using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetVariables;
using UnityEngine;

namespace Wireframes
{
    public class Wireframe1 : MonoBehaviour
    {
        public bool render_mesh_normaly = true;
        public bool render_lines_1st = false;
        public bool render_lines_2nd = false;
        public bool render_lines_3rd = false;
        public ColorVariable lineColor;
        public ColorVariable backgroundColor;

        public Material lineMaterial;
        public Material meshMaterial;

        private Vector3[] lines;
        private ArrayList lines_List;
        private Renderer targetRenderer;
        private Camera mainCamera;
        
        
        
        private void Start()
        {
#if UNITY_ANDROID
            enabled = false;
            return;
#endif
#if UNITY_EDITOR
            enabled = false;
            //return;
#endif
        
            mainCamera = Camera.main;

            targetRenderer = GetComponent<Renderer>();
            targetRenderer.material = meshMaterial;
            lines_List = new ArrayList();

            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i + 2 < triangles.Length; i += 3)
            {
                lines_List.Add(vertices[triangles[i]]);
                lines_List.Add(vertices[triangles[i + 1]]);
                lines_List.Add(vertices[triangles[i + 2]]);
            }

            //arrays are faster than array lists
            lines = (Vector3[]) lines_List.ToArray(typeof(Vector3));
            lines = lines.Select(to_world).ToArray();
            lines_List.Clear(); //free memory from the arraylist
        }

        private void Update()
        {
            
        }


        private Vector3 to_world(Vector3 vec)
        {
            return gameObject.transform.TransformPoint(vec);
        }
    }
}