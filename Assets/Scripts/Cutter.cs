using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private float minimumStickXScale;

    [SerializeField] private Transform leftStick, rightStick;
    [SerializeField] private float delay, centerSpeed, throwSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saw"))
        {
            var currentStickXScale = transform.localScale.x;
            
            var sawXPosition = other.transform.position.x;
            var sawXSize = other.bounds.size.x;

            if (this.CompareTag("LeftStick"))
            {
                var newSawXPosition = sawXPosition + sawXSize / 2;
                var newStickXScale = (sideMovementRoot.position.x - newSawXPosition) / 2;

                if(newStickXScale >= minimumStickXScale)
                {
                    SetNewStickScale(newStickXScale);

                    var detachedStickXScale = currentStickXScale - newStickXScale;
                    var detachedStickScale = new Vector3(detachedStickXScale, transform.localScale.y, transform.localScale.z);

                    var detachedStickPosition = new Vector3(newSawXPosition, transform.position.y, transform.position.z);

                    DetachPieceFromStick(detachedStickScale, detachedStickPosition, throwSpeed);

                    StartCoroutine(CenterTheStick());
                }
                else
                {
                    Debug.Log("Oyun Bitti");
                }
            }

            if (this.CompareTag("RightStick"))
            {
                var newSawXPosition = sawXPosition - sawXSize / 2;
                var newStickXScale = (sideMovementRoot.position.x - newSawXPosition) / 2;

                if(-newStickXScale >= minimumStickXScale)
                {
                    SetNewStickScale(-newStickXScale);

                    var detachedStickXScale = currentStickXScale + newStickXScale;
                    var detachedStickScale = new Vector3(detachedStickXScale, transform.localScale.y, transform.localScale.z);

                    var detachedStickPosition = new Vector3(newSawXPosition, transform.position.y, transform.position.z);

                    DetachPieceFromStick(detachedStickScale, detachedStickPosition, throwSpeed);

                    StartCoroutine(CenterTheStick());
                }
                else
                {
                    Debug.Log("Oyun Bitti");
                }
            }

            other.enabled = false;
        }
    }

    private void SetNewStickScale(float newStickXScale)
    {
        var stickLocalScale = transform.localScale;
        stickLocalScale.x = newStickXScale;
        transform.localScale = stickLocalScale;
    }

    private void DetachPieceFromStick(Vector3 detachedStickScale, Vector3 detachedStickPosition, float throwSpeed)
    {
        GameObject detachedStick = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        detachedStick.transform.localScale = detachedStickScale;
        detachedStick.transform.position = detachedStickPosition;

        Rigidbody detachedStickRigidbody = detachedStick.AddComponent<Rigidbody>();
        detachedStickRigidbody.AddForce(0f, 0f, throwSpeed);
    }

    private IEnumerator CenterTheStick()
    {
        if (leftStick.localScale.x != rightStick.localScale.x)
        {
            yield return new WaitForSeconds(delay);

            float newXScale = (leftStick.localScale.x + rightStick.localScale.x) / 2;

            var leftStickScale = leftStick.localScale;
            leftStickScale.x = Mathf.Lerp(leftStickScale.x, newXScale, centerSpeed);
            leftStick.localScale = leftStickScale;

            var rightStickScale = rightStick.localScale;
            rightStickScale.x = Mathf.Lerp(rightStickScale.x, newXScale, centerSpeed);
            rightStick.localScale = rightStickScale;
        }
    }
}
