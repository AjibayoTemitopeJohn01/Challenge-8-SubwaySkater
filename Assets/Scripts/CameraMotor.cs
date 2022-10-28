using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // our player // object we are looking at
    public Vector3 offset = new Vector3(0, 5.0f, -10.0f);
    public Vector3 rotation = new Vector3(35, 0, 0);

    public bool IsMoving
    {
        set;
        get;
    }
    
    private void LateUpdate()
    {
        if(!IsMoving)
            return;
        
        Vector3 desiredPos = lookAt.position + offset;
        desiredPos.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPos, 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);
    }
}
