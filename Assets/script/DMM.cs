using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class DMM : MonoBehaviour {


    float[] mu_shape_, mu_exp_;
  
    float[,] b_shape_,b_exp_; //100x95823

    float[] test_shape_dmm_, test_exp_dmm_;
    int[] unity_index_map_;
    Vector3[] vertice_base;
    Mesh mesh;
    Vector3[] vertices; 
    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        int pt_num = vertices.Length;
        mu_shape_ = new float[95823];
        mu_exp_ = new float[95823];

      
        b_shape_ = new float[100, 95823];
        b_exp_ = new float[79, 95823];

        test_shape_dmm_ = new float[100];
        test_exp_dmm_ = new float[79];
        unity_index_map_ = new int[pt_num];
        vertice_base = new Vector3[pt_num];
        LoadDMM();
        Debug.Log("Load DMM");
        LoadMean();
        Debug.Log("Load Mean");
        LoadBasis();
        Debug.Log("Load Basis");
        LoadMap();
        Debug.Log("Load MAP");
        VertexBasis();
        Debug.Log("Load VertexBasis");
        //Test();
    }

    void LoadDMM()
    {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Data\3dmm_test.txt");
        string[] identity_line = lines[0].Split();
        string[] exp_line = lines[1].Split();
        int count = 0;
        for (int i=0;i<190;i++) 
        {
            if (System.String.IsNullOrEmpty(identity_line[i]))
                continue ;
            test_shape_dmm_[count] =float.Parse(identity_line[i]);
            count += 1;
        }
        count = 0;
        for (int i = 0; i < 135; i++)
        {
            if (System.String.IsNullOrEmpty(exp_line[i]))
                continue;
            test_exp_dmm_[count] =float.Parse(exp_line[i]);
            count += 1;
        }
    }


    void LoadMean()
    {
        string filename = @"Assets\Data\new_mu_shape.txt";
        string[] lines = System.IO.File.ReadAllLines(filename);
        for(int i=0;i<lines.Length;i++)
        {
            mu_shape_[i] = float.Parse(lines[i]);
        }

        filename = @"Assets\Data\new_mu_exp.txt";
        string[] lines_ = System.IO.File.ReadAllLines(filename);
        for (int i = 0; i < lines_.Length; i++)
        {
            mu_exp_[i] = float.Parse(lines[i]);
        }
    }

    void LoadBasis()
    {
        for(int i=0;i<100;i++)
        {
            string filename = @"Assets\Data\new_"+i.ToString()+"_identity.txt";
            string[] lines = System.IO.File.ReadAllLines(filename);
            for (int j = 0; j < lines.Length; j++)
            {
                b_shape_[i, j] = float.Parse(lines[j]);
            }
        }

        for (int i = 0; i < 79; i++)
        {
            string filename = @"Assets\Data\new_" + i.ToString() + "_expression.txt";
            string[] lines = System.IO.File.ReadAllLines(filename);
            
            for (int j = 0; j < lines.Length; j++)
            {
                b_exp_[i, j] = float.Parse(lines[j]);
            }

        }
    }


    void LoadMap()
    {
        string filename = @"Assets\script\unity_index.txt";
        string[] lines = System.IO.File.ReadAllLines(filename);
        for (int i = 0; i < lines.Length; i++)
        {
            unity_index_map_[i]=int.Parse(lines[i]);
        }
    }

    void VertexBasis()
    {

        for (int i = 0; i < vertice_base.Length; i++)
        {
            int index_ = unity_index_map_[i];
            float x = mu_exp_[3 * index_] + mu_exp_[3 * index_];
            float y = mu_exp_[3 * index_ + 1] + mu_exp_[3 * index_ + 1];
            float z = mu_exp_[3 * index_ + 2] + mu_exp_[3 * index_ + 2];
            for (int j = 0; j < 100; j++)
            {
                if (j < 100)
                {
                    x += b_shape_[j, 3 * index_] * test_shape_dmm_[j];
                    y += b_shape_[j, 3 * index_ + 1] * test_shape_dmm_[j];
                    z += b_shape_[j, 3 * index_ + 2] * test_shape_dmm_[j];
                }

            }
            vertice_base[i].x = x;
            vertice_base[i].y = y;
            vertice_base[i].z = z;
        }
    }

    public void ChangeExpression(float[] parameter)
    {
        

        for (int i = 0; i < vertices.Length; i++)
        {

            int index_ = unity_index_map_[i];
            float x = vertice_base[i].x;
            float y = vertice_base[i].y;
            float z = vertice_base[i].z;
            for (int j = 0; j < 79; j++)
            {
                x += b_exp_[j, 3 * index_] * parameter[j];
                y += b_exp_[j, 3 * index_ + 1] * parameter[j];
                z += b_exp_[j, 3 * index_ + 2] * parameter[j];
            }
            vertices[i].x = x;
            vertices[i].y = y;
            vertices[i].z = z;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

    }

    void Test()
    {

        for (int i=0;i< vertices.Length;i++)
        {

            int index_ = unity_index_map_[i];
            float x = vertice_base[i].x;
            float y = vertice_base[i].y;
            float z = vertice_base[i].z;
            //for (int j = 0; j < 79; j++)
            //{
            //    x += b_exp_[j ,3 * index_] * test_exp_dmm_[j];
            //    y += b_exp_[j ,3 * index_ + 1] * test_exp_dmm_[j ];
            //    z += b_exp_[j ,3 * index_ + 2] * test_exp_dmm_[j ];
            //}
            vertices[i].x = x;
            vertices[i].y = y;
            vertices[i].z = z;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        
    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetButtonDown("Jump"))
        //    Test();
        
    }
}
