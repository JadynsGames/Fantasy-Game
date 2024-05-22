using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FireRay();
    }

    void FireRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;

        Physics.Raycast(ray, out hitData);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
    }
}
