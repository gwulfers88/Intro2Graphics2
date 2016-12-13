using UnityEngine;
using System.Collections;

public class BendyControl : MonoBehaviour
{
    public Material bendyMaterial;

    public Vector4 start = -Vector4.one;
    public Vector4 end = Vector4.one;

    float t = 0;
    int sign = 1;

    // Use this for initialization
    void Start ()
    {
        bendyMaterial = GetComponent<MeshRenderer>().material;

        if(bendyMaterial.shader.name != "Custom/bendy")
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        t += sign * 0.5f *  Time.deltaTime;

        Vector3 CameraDirection = (Camera.main.transform.position - transform.position).normalized;
        Vector3 NewDirection = Vector3.RotateTowards(transform.forward, CameraDirection, 2 * Time.deltaTime, 0).normalized;
        transform.rotation = Quaternion.LookRotation(NewDirection);

        if (t > 1)
        {
            sign = -1;
            t = 1;
        }
        else if (t < 0)
        {
            sign = 1;
            t = 0;
        }
        
        Vector4 lerp = Vector4.Lerp(start, end, t);
        bendyMaterial.SetVector("_ControlPoint", lerp);
	}
}
