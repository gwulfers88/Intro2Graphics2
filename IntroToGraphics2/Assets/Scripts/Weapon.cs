using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public Transform Mussle;
    public float ShootRate = 3.0f;

    float ShootTimer = 0.0f;
    
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        ShootTimer += Time.deltaTime;

	    if(Input.GetButtonDown("Fire1"))
        {
            if (ShootTimer >= ShootRate)
            {
                GameObject Bullet = Instantiate(Resources.Load("Bullet"), Mussle.position, Mussle.rotation) as GameObject;
                Bullet.GetComponent<Bullet>().SetDirection(Mussle.forward);

                ShootTimer = 0.0f;
            }
        }
	}
}
