using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacker : MonoBehaviour
{
    [SerializeField] private Transform leftStick, rightStick;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableStick"))
        {
            var addingScale = other.transform.localScale.x / 2;

            IncreaseStickScale(addingScale);

            Destroy(other.transform.parent.gameObject);
        }
    }

    private void IncreaseStickScale(float addingScale)
    {
        var currentLeftStickScale = leftStick.localScale;
        currentLeftStickScale.x += addingScale;
        leftStick.localScale = currentLeftStickScale;

        var currentRightStickScale = rightStick.localScale;
        currentRightStickScale.x += addingScale;
        rightStick.localScale = currentRightStickScale;
    }
}
