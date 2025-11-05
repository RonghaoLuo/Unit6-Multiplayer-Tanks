using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
