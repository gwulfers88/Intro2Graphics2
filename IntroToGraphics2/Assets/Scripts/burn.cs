using UnityEngine;
using System.Collections;

public class burn : MonoBehaviour
{
    [SerializeField]
    float rate = 0.05f;
    Material burnMaterial;
    float Threshold1;
    float Threshold2;
    Vector2 WorldToUV;
    bool Burning = false;

    // Use this for initialization
    void Start ()
    {
        burnMaterial = GetComponent<Renderer>().material;
        if(burnMaterial.shader.name != "Custom/ClickBurn")
        {
            Destroy(this);
        }

        Threshold1 = burnMaterial.GetFloat("_Thresh1");
        Threshold2 = burnMaterial.GetFloat("_Thresh2");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 MouseP = Vector2.zero;
        GameObject obj = null;
        RaycastHit hit;
        Ray ray;

        if (Input.GetMouseButtonDown(0) && !Burning)
        {
            Burning = true;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject)
                {
                    obj = hit.collider.gameObject;
                    MouseP = hit.point;
                }
            }
        }
        
        if (Burning)
        {
            if(obj)
            {
                Vector2 Dims = obj.GetComponent<Collider>().bounds.size;
                Vector2 RelativeCenter = new Vector2(0.5f, 0.5f);
                WorldToUV.x = Mathf.Abs((transform.position.x - RelativeCenter.x) - MouseP.x / Dims.x);
                WorldToUV.y = Mathf.Abs((transform.position.y - RelativeCenter.y) - MouseP.y / Dims.y);
            }
            
            Threshold2 += (Time.deltaTime * rate);
            Threshold1 += (Time.deltaTime * rate) * 0.85f;

            Threshold1 = Mathf.Clamp(Threshold1, 0.0f, 0.69f);
            Threshold2 = Mathf.Clamp(Threshold2, 0.0f, 0.69f);

            if (Threshold1 >= 0.68f && Threshold2 >= 0.68f)
            {
                Threshold2 = Threshold1 = 0;
                Burning = false;
            }
        }
        
        burnMaterial.SetFloat("_MouseX", WorldToUV.x);
        burnMaterial.SetFloat("_MouseY", WorldToUV.y);

        burnMaterial.SetFloat("_Thresh1", Threshold1);
        burnMaterial.SetFloat("_Thresh2", Threshold2);
    }
}
