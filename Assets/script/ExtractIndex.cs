using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractIndex : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Start Index write");
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        using (System.IO.StreamWriter file =new System.IO.StreamWriter(@"D:\Code\3DMM-Unity\Unity_pt.txt"))
        {
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                string line = (vertices[i].x).ToString() + ", " + (vertices[i].y).ToString() + ", " + (vertices[i].z).ToString();
                file.WriteLine(line);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
