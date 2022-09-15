using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    [SerializeField] private GameObject trailMesh;
    [SerializeField] private int trailFrameLength;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 preTopPos;
    private Vector3 preBottomPos;

    private const int NUM_VERTICES = 12;

    private void Start()
    {
        mesh = new Mesh();
        trailMesh.GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[trailFrameLength * NUM_VERTICES];
        triangles = new int[vertices.Length];

        preTopPos = top.transform.position;
        preBottomPos = bottom.transform.position;
    }

    private void Update()
    {
        if(frameCount == (trailFrameLength * NUM_VERTICES))
            frameCount = 0;

        vertices[frameCount] = bottom.transform.position;
        vertices[frameCount + 1] = top.transform.position;
        vertices[frameCount + 2] = preTopPos;

        vertices[frameCount + 3] = bottom.transform.position;
        vertices[frameCount + 4] = preTopPos;
        vertices[frameCount + 5] = top.transform.position;

        vertices[frameCount + 6] = preTopPos;
        vertices[frameCount + 7] = bottom.transform.position;
        vertices[frameCount + 8] = preBottomPos;

        vertices[frameCount + 9] = preTopPos;
        vertices[frameCount + 10] = preBottomPos;
        vertices[frameCount + 11] = bottom.transform.position;

        for (int i = 0; i < NUM_VERTICES; ++i)
            triangles[frameCount + i] = frameCount + i;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        preTopPos = top.transform.position;
        preBottomPos = bottom.transform.position;

        frameCount += NUM_VERTICES;
    }
}
