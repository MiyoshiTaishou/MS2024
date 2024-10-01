using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachColor : MonoBehaviour
{
    Dictionary<Collider, GameObject> dict = new Dictionary<Collider, GameObject>();
    public Color color = new Color(1, 0, 0, 0.2f);

    private GameObject CreatePrimitive(PrimitiveType type)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        Destroy(obj.GetComponent<Collider>());
        return obj;
    }

    void UpdatePrimitive(GameObject primitive, SphereCollider collider)
    {
        primitive.transform.localPosition = collider.center;
        primitive.transform.localScale = Vector3.one * collider.radius * 2f;
    }

    void UpdatePrimitive(GameObject primitive, BoxCollider collider)
    {
        primitive.transform.localPosition = collider.center;
        primitive.transform.localScale = collider.size;
    }

    void UpdatePrimitive(GameObject primitive, CapsuleCollider capsuleCollider)
    {
        primitive.transform.localPosition = capsuleCollider.center;

        if (capsuleCollider.direction == 0) primitive.transform.rotation = Quaternion.Euler(90, 0, 0);
        if (capsuleCollider.direction == 2) primitive.transform.rotation = Quaternion.Euler(0, 0, 90);

        Vector3 s = Vector3.one;// primitive.transform.localScale;
        float radius = capsuleCollider.radius;
        float sx = s.x * radius * 2f;
        float sy = s.y * capsuleCollider.height * 0.5f;
        float sz = s.z * radius * 2f;
        primitive.transform.localScale = new Vector3(sx, sy, sz);
    }

    void Awake()
    {
        
        var colliders = GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
            if (collider.enabled == false) continue;

            GameObject primitive;

            if (collider is SphereCollider)
            {
                primitive = CreatePrimitive(PrimitiveType.Sphere);
                primitive.transform.SetParent(collider.transform, false);
                UpdatePrimitive(primitive, collider as SphereCollider);
            }
            else if (collider is BoxCollider)
            {
                primitive = CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.SetParent(collider.transform, false);
                UpdatePrimitive(primitive, collider as BoxCollider);
            }
            else
            {
                primitive = CreatePrimitive(PrimitiveType.Capsule);
                primitive.transform.SetParent(collider.transform, false);
                UpdatePrimitive(primitive, collider as CapsuleCollider);
            }

            var material = primitive.GetComponent<Renderer>().material;
            material.shader = Shader.Find("Sprites/Default");
            primitive.GetComponent<MeshRenderer>().material.color = color;

            this.dict.Add(collider, primitive);
        }
    }

    void Update()
    {
        foreach (var kv in this.dict)
        {
            Collider collider = kv.Key;
            GameObject primitive = kv.Value;

            if (collider is SphereCollider)
            {
                UpdatePrimitive(primitive, collider as SphereCollider);
            }
            else if (collider is BoxCollider)
            {
                UpdatePrimitive(primitive, collider as BoxCollider);
            }
            else
            {
                UpdatePrimitive(primitive, collider as CapsuleCollider);
            }
        }
    }
}
