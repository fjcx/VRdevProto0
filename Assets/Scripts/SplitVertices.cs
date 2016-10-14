using UnityEngine;
using System.Collections;

// Vertex Splitting script from https://forum.unity3d.com/threads/achieving-the-darwinia-look-in-unity.119870/
public class SplitVertices : MonoBehaviour {

    public void Start() {
        // Get mesh info from attached mesh
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uv = mesh.uv;
        int[] triangles = mesh.triangles;

        // Set up new arrays to use with rebuilt mesh
        Vector3[] newVertices = new Vector3[triangles.Length];
        Vector2[] newUV = new Vector2[triangles.Length];

        // Rebuild mesh so that every triangle has unique vertices
        for (int i = 0; i < triangles.Length; i++) {
            newVertices[i] = vertices[triangles[i]];
            newUV[i] = uv[triangles[i]];
            triangles[i] = i;
        }

        // Assign new mesh and rebuild normals
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
