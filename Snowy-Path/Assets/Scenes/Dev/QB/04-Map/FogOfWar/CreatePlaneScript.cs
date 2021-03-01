﻿using UnityEngine;
using System.Collections;

public class CreatePlaneScript : MonoBehaviour
{

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public enum AnchorPoint
    {
        TopLeft,
        TopHalf,
        TopRight,
        RightHalf,
        BottomRight,
        BottomHalf,
        BottomLeft,
        LeftHalf,
        Center
    }

    public int widthSegments = 1;
    public int lengthSegments = 1;
    public float width = 1.0f;
    public float length = 1.0f;
    public Orientation orientation = Orientation.Horizontal;
    public AnchorPoint anchor = AnchorPoint.Center;
    public bool addCollider = false;
    public bool createAtOrigin = true;
    public string optionalName;

    void Start()
    {
        BuildPlane();
    }

    void Update()
    {
        widthSegments = Mathf.Clamp(widthSegments, 1, 254);
        lengthSegments = Mathf.Clamp(lengthSegments, 1, 254);
    }

    public void BuildPlane()
    {

        Vector2 anchorOffset;
        string anchorId;
        switch (anchor)
        {
        case AnchorPoint.TopLeft:
            anchorOffset = new Vector2(-width/2.0f,length/2.0f);
            anchorId = "TL";
            break;
        case AnchorPoint.TopHalf:
            anchorOffset = new Vector2(0.0f,length/2.0f);
            anchorId = "TH";
            break;
        case AnchorPoint.TopRight:
            anchorOffset = new Vector2(width/2.0f,length/2.0f);
            anchorId = "TR";
            break;
        case AnchorPoint.RightHalf:
            anchorOffset = new Vector2(width/2.0f,0.0f);
            anchorId = "RH";
            break;
        case AnchorPoint.BottomRight:
            anchorOffset = new Vector2(width/2.0f,-length/2.0f);
            anchorId = "BR";
            break;
        case AnchorPoint.BottomHalf:
            anchorOffset = new Vector2(0.0f,-length/2.0f);
            anchorId = "BH";
            break;
        case AnchorPoint.BottomLeft:
            anchorOffset = new Vector2(-width/2.0f,-length/2.0f);
            anchorId = "BL";
            break;            
        case AnchorPoint.LeftHalf:
            anchorOffset = new Vector2(-width/2.0f,0.0f);
            anchorId = "LH";
            break;            
        case AnchorPoint.Center:
        default:
            anchorOffset = Vector2.zero;
            anchorId = "C";
            break;
        }

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        Mesh m = new Mesh();
        m.name = this.name;

        int hCount2 = widthSegments+1;
        int vCount2 = lengthSegments+1;
        int numTriangles = widthSegments * lengthSegments * 6;
        int numVertices = hCount2 * vCount2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];

        int index = 0;
        float uvFactorX = 1.0f/widthSegments;
        float uvFactorY = 1.0f/lengthSegments;
        float scaleX = width/widthSegments;
        float scaleY = length/lengthSegments;
        for (float y = 0.0f; y < vCount2; y++)
        {
            for (float x = 0.0f; x < hCount2; x++)
            {
                if (orientation == Orientation.Horizontal)
                {
                    vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, 0.0f, y*scaleY - length/2f - anchorOffset.y);
                }
                else
                {
                    vertices[index] = new Vector3(x*scaleX - width/2f - anchorOffset.x, y*scaleY - length/2f - anchorOffset.y, 0.0f);
                }
                uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
            }
        }

        index = 0;
        for (int y = 0; y < lengthSegments; y++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[index]   = (y     * hCount2) + x;
                triangles[index+1] = ((y+1) * hCount2) + x;
                triangles[index+2] = (y     * hCount2) + x + 1;

                triangles[index+3] = ((y+1) * hCount2) + x;
                triangles[index+4] = ((y+1) * hCount2) + x + 1;
                triangles[index+5] = (y     * hCount2) + x + 1;
                index += 6;
            }
        }

        m.vertices = vertices;
        m.uv = uvs;
        m.triangles = triangles;
        m.RecalculateNormals();

        meshFilter.mesh = m;
        m.RecalculateBounds();

        if (addCollider) {
            this.gameObject.AddComponent(typeof(BoxCollider));
        }

        //Selection.activeObject = plane;
    }
}