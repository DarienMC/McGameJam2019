using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public Sprite standardReticle;
    public Sprite empoweredReticle;
    public float empowerScaling = 1.1f;

    private Animator animator;
    private Image image;
    private RectTransform rectTransform;
    private float standardScaling;

    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        standardScaling = rectTransform.sizeDelta.x;
    }

    public void Charge(float duration)
    {
        animator.CrossFade("Charging", 0.0f);
        animator.speed = 1 / duration;
    }

    public void ResetReticle()
    {
        image.sprite = standardReticle;
        rectTransform.sizeDelta = Vector2.one * standardScaling;
    }

    public void EmpowerReticle()
    {
        image.sprite = empoweredReticle;
        rectTransform.sizeDelta = Vector2.one * standardScaling * empowerScaling;
    }
}
