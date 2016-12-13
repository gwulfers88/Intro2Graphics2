using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject Target;
    public float WalkSpeed;
    public float TurnSpeed;
    public bool IsDestructable = true;
    public Material DissolveMat;

    float T1 = 0;
    float T2 = 0;

    Color DebugRayColor = Color.blue;
    Destructable Destructable;

    void Start()
    {
        DissolveMat = GetComponentInChildren<MeshRenderer>().material;

        DissolveMat.SetFloat("_Thresh1", T1);
        DissolveMat.SetFloat("_Thresh2", T2);

        if (IsDestructable)
        {
            Destructable = gameObject.AddComponent<Destructable>();
        }
    }
    
	// Update is called once per frame
	void Update ()
    {
        RaycastHit Hit;

        Debug.DrawRay(transform.position, transform.forward * 20.0f, DebugRayColor);

        if (Destructable.GetHitCount() > 0.0f)
        {
            if (Target)
            {
                float DistanceToTarget = Vector3.Distance(Target.transform.position, transform.position);
                if (DistanceToTarget <= 20.0f)
                {
                    Vector3 DirectionToTarget = (Target.transform.position - transform.position).normalized;
                    Vector3 NewDirection = Vector3.RotateTowards(transform.forward, DirectionToTarget, TurnSpeed * Time.deltaTime, 0).normalized;

                    if (Vector3.Dot(DirectionToTarget, NewDirection) < 0.99f)
                    {
                        transform.rotation = Quaternion.LookRotation(NewDirection);
                    }
                    else
                    {
                        if (DistanceToTarget > 2.0f)
                        {
                            transform.position += transform.forward * WalkSpeed * Time.deltaTime;
                        }
                        else
                        {
                            // Attack Player
                            DebugRayColor = Color.black;
                        }
                    }

                    Debug.DrawLine(transform.position, DirectionToTarget * 20.0f, Color.green);
                }
                else
                {
                    Target = null;
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out Hit, 20.0f))
                {
                    if (Hit.collider.gameObject.CompareTag("Player"))
                    {
                        DebugRayColor = Color.red;
                        Target = Hit.collider.gameObject;
                    }
                }
                else
                {
                    DebugRayColor = Color.blue;
                }
            }
        }
        else
        {
            T2 += Time.deltaTime * 0.5f;
            T1 += (Time.deltaTime * 0.5f) * 0.5f;

            DissolveMat.SetFloat("_Thresh1", T1);
            DissolveMat.SetFloat("_Thresh2", T2);

            if(T1 >= 1.0f && T2 >= 1.0f)
            {
                Destructable.DestroyNow(true);
                Destructable.TakeDamage(1);
            }
        }
	}

    void OnCollisionEnter(Collision Col)
    {
        if(Col.collider.gameObject.CompareTag("Bullet"))
        {
            if (IsDestructable)
            {
                Destructable.TakeDamage(1);
                Destroy(Col.collider.gameObject);
            }
        }
    }
}
