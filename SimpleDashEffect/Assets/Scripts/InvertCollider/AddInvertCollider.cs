using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class AddInvertCollider : MonoBehaviour
{
   public bool removeExistingColliders = true;
 
  public void CreateInvertedMeshCollider()
  {
    if (removeExistingColliders)
      RemoveExistingColliders();
 
  InvertMesh();
 
  gameObject.AddComponent<MeshCollider>();
  }
 
  private void RemoveExistingColliders()
  {
    Collider[] colliders = GetComponents<Collider>();
    for (int i = 0; i < colliders.Length; i++)
      DestroyImmediate(colliders[i]);
  }
 
  private void InvertMesh()
  {
    Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
    mesh.triangles = mesh.triangles.Reverse().ToArray();
  }
}

[CustomEditor(typeof(AddInvertCollider))]
public class AddInvertedMeshColliderEditor :Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    AddInvertCollider script = (AddInvertCollider)target;
    if (GUILayout.Button("Create Inverted Mesh Collider"))
      script.CreateInvertedMeshCollider();
   }
}
