using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;

    private MeshRenderer meshRenderer;
    [SerializeField] private GameObject topPositon = null;
    [SerializeField] private GameObject bottomPositon = null;
    [SerializeField] private GameObject trailMesh = null;
    [SerializeField] private int trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color _colour = Color.red;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 prevTobPosition;
    private Vector3 prevBottomPosition;

    void Awake()
    {
        transform.SetParent(null);
        transform.position = bottomPositon.transform.position;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1f, 1f, 1f);
        mesh = new Mesh();
        trailMesh.GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[trailFrameLength * NUM_VERTICES];
        triangles = new int[vertices.Length];

        prevTobPosition = topPositon.transform.position - transform.position;
        prevBottomPosition = bottomPositon.transform.position - transform.position;
        
    }

    void LateUpdate()
    {
        transform.position = bottomPositon.transform.position;
        //Reset the frame count one we reach the frame length
        if (frameCount == (trailFrameLength * NUM_VERTICES))
        {
            frameCount = 0;
        }
        Vector3 bottomPos = bottomPositon.transform.position - transform.position;
        Vector3 topPos = topPositon.transform.position - transform.position;
        //Draw first triangle vertices for back and front
        vertices[frameCount] = bottomPos;
        vertices[frameCount + 1] = topPos;
        vertices[frameCount + 2] = prevTobPosition;
        vertices[frameCount + 3] = bottomPos;
        vertices[frameCount + 4] = prevTobPosition;
        vertices[frameCount + 5] = topPos;

        //Draw fill in triangle vertices
        vertices[frameCount + 6] = prevTobPosition;
        vertices[frameCount + 7] = bottomPos;
        vertices[frameCount + 8] = prevBottomPosition;
        vertices[frameCount + 9] = prevTobPosition;
        vertices[frameCount + 10] = prevBottomPosition;
        vertices[frameCount + 11] = bottomPos;

        //Set triangles
        triangles[frameCount] = frameCount;
        triangles[frameCount + 1] = frameCount + 1;
        triangles[frameCount + 2] = frameCount + 2;
        triangles[frameCount + 3] = frameCount + 3;
        triangles[frameCount + 4] = frameCount + 4;
        triangles[frameCount + 5] = frameCount + 5;
        triangles[frameCount + 6] = frameCount + 6;
        triangles[frameCount + 7] = frameCount + 7;
        triangles[frameCount + 8] = frameCount + 8;
        triangles[frameCount + 9] = frameCount + 9;
        triangles[frameCount + 10] = frameCount + 10;
        triangles[frameCount + 11] = frameCount + 11;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //Track the previous base and tip positions for the next frame
        prevTobPosition = topPos;
        prevBottomPosition = bottomPos;
        frameCount += NUM_VERTICES;
    }
}
