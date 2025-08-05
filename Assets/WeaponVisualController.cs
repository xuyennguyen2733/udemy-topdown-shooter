using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandIKHint;

    [Header("Rig")]
    private Rig rig;
    [SerializeField] float rigIncreaseStep;
    private bool rigShouldBeIncreased;
    private bool rigShouldBeDecreased;

    private Animator animator;

    private void Start()
    {
        SwitchGuns(pistol);

        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("reload");
            rigShouldBeDecreased = true;
            //rig.weight = 0;
        }

        EaseRigWeightOut();
        EaseRigWeightIn();
    }

    private void EaseRigWeightIn()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;
            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void EaseRigWeightOut()
    {
        if (rigShouldBeDecreased)
        {
            rig.weight -= rigIncreaseStep * Time.deltaTime;
            if (rig.weight <= 0)
            {
                rigShouldBeDecreased = false;
            }
        }
    }

    public void ReturnRigWeightToOne() => rigShouldBeIncreased = true;

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGuns(pistol);
            SwitchAnimationLayers(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGuns(revolver);
            SwitchAnimationLayers(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchGuns(autoRifle);
            SwitchAnimationLayers(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchGuns(shotgun);
            SwitchAnimationLayers(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchGuns(rifle);
            SwitchAnimationLayers(3);
        }
    }

    private void SwitchGuns(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        SetLeftHandPosition(gunTransform);

    }

    private void SetLeftHandPosition(Transform gunTransform)
    {
        Transform targetTransform = gunTransform.Find("LeftHand_TargetTransform").transform;
        Transform hintTransform = gunTransform.Find("LeftHand_HintTransform").transform;

        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;

        leftHandIKHint.localPosition = hintTransform.localPosition;
        leftHandIKHint.localRotation = hintTransform.localRotation;
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void SwitchAnimationLayers(int layerIndex)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }
}
