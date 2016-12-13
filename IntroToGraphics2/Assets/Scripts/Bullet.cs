using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float Speed = 20;

	// Use this for initialization
	void Start ()
    {
	
	}
	
    public void SetDirection(Vector3 Direction)
    {
        transform.forward = Direction;
        Destroy(this.gameObject, 10);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
	}
}
