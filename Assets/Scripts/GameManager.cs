using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

    [SerializeField] private Transform kid, x1;
    [SerializeField] private Slider slider;
    private float totalDistance;

    [SerializeField] private GameObject levelCompletedText, nextButton;
    [SerializeField] private GameObject targetYPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalDistance = x1.position.z - kid.position.z;
    }

    void Update()
    {
        CheckProgressBar();
    }

    public void OnGameFinished()
    {
        StartCoroutine(FinishUI());
    }

    private void CheckProgressBar()
    {
        var kidModelCurrentPosition = kid.position.z;
        var currentDistance = totalDistance - kidModelCurrentPosition;
        var newSliderValue = (totalDistance - currentDistance) / totalDistance;
        slider.value = newSliderValue;
    }

    private IEnumerator FinishUI()
    {
        levelCompletedText.transform.DOMoveY(targetYPosition.transform.position.y, 1.5f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2f);

        nextButton.transform.DOScale(Vector3.one, 1.5f)
            .SetEase(Ease.OutBack);
    }
}
