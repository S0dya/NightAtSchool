using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collision) => ToggleMesh(collision.gameObject, true);
    void OnTriggerExit(Collider collision) => ToggleMesh(collision.gameObject, false);

    void ToggleMesh(GameObject obj, bool val)
    {
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = val;
        foreach (Transform child in obj.transform) ToggleMesh(child.gameObject, val);
    }
}
