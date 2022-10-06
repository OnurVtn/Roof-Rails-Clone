using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform kidTransform;
    [SerializeField] private float smooth;
    private float offsetY, offsetZ;

    void Start()
    {
        offsetY = transform.position.y - kidTransform.position.y;
        offsetZ = transform.position.z - kidTransform.position.z;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, kidTransform.position.y + offsetY, kidTransform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth);
    }
}
