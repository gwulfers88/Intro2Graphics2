using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour
{
    public float HitCount = 5;
    public bool Dest = false;

    public float GetHitCount()
    {
        return HitCount;
    }

    public void DestroyNow(bool Dest)
    {
        this.Dest = Dest;
    }

	public void TakeDamage(float Damage)
    {
        HitCount -= Damage;

        Debug.Log("Damaged: " + Damage);
        Debug.Log("HitCount: " + HitCount);

        if (HitCount <= 0.0f && Dest)
        {
            Destroy(this.gameObject);
        }
    }
}
