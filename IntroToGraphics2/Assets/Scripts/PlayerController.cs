using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float WalkSpeed;
    public float TurnSpeed;
    public float Sensitivity;
    float Yaw = 0;
    float Pitch = 0;

    // Use this for initialization
    void Start ()
    {
        LockCursor(true);
	}
	
    void LockCursor(bool Locked)
    {
        if (Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Cursor.visible = !Locked;
    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
        }

        float Forward = Input.GetAxis("Vertical");
        float Side = Input.GetAxis("Horizontal");

        float VerticalTurn = Input.GetAxis("Mouse Y");
        float HorizontalTurn = Input.GetAxis("Mouse X");

        Yaw += HorizontalTurn;
        Pitch += -VerticalTurn;

        Camera.main.transform.localEulerAngles = new Vector3(Pitch, 0, 0) * Sensitivity * TurnSpeed;
        transform.localEulerAngles = new Vector3(0.0f, Yaw, 0.0f) * Sensitivity * TurnSpeed;
        transform.position += ((transform.forward * Forward) + (transform.right * Side)) * WalkSpeed * Time.deltaTime;
	}
}
