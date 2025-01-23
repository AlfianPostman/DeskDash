using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform camTarget;
    public Transform offsetTarget;

    public void Moving(bool isMoving)
    {
        if(isMoving)
            camTarget.transform.position = offsetTarget.transform.position;
        else
            camTarget.transform.position = transform.position;
    }
}
