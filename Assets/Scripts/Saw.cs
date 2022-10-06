using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        transform.Rotate(Time.deltaTime * rotateSpeed, transform.rotation.y, transform.rotation.z);
    }
}
