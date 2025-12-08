using UnityEngine;

public class UIStabilizer : MonoBehaviour
{
    void LateUpdate()
    {
        // 1. Force Rotation to be zero (World Up)
        transform.rotation = Quaternion.identity;
        // transform.position = shipTransform.position + offset;
    }
}