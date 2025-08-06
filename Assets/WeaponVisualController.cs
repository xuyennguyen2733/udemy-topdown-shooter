using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Left Hand IK")]
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandIKHint;
    [SerializeField] private TwoBoneIKConstraint leftHandIkConstraint;

    [Header("Rig")]
    private Rig rig;
    [SerializeField] private float reloadRigUpSteps;
    [SerializeField] private float reloadRigDownSteps;
    [SerializeField] private float switchWeaponsRigUpSteps;
    [SerializeField] private float switchWeaponsRigDownSteps;
    private float rigUpSteps;
    private float rigDownSteps;
    private bool rigShouldBeIncreased;
    private bool rigShouldBeDecreased;

    private bool isLockedInAction;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        isLockedInAction = false;
        //animator.SetBool("isLockedInAction", isLockedInAction);
        SwitchGuns(pistol);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("reload");
            rigUpSteps = reloadRigUpSteps;
            rigDownSteps = reloadRigDownSteps;
            PauseRig();
            //rig.weight = 0;
        }

        EaseRigWeightDown();
        EaseRigWeightUp();
    }

    public void EndLockInAction()
    {
        isLockedInAction = false;
        //animator.SetBool("isLockedInAction", isLockedInAction);
    }

    private void PauseRig()
    {
        rigShouldBeDecreased = true;
    }

    private void EaseRigWeightUp()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigUpSteps * Time.deltaTime;
            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void EaseRigWeightDown()
    {
        if (rigShouldBeDecreased)
        {
            rig.weight -= rigDownSteps * Time.deltaTime;
            if (rig.weight <= 0)
            {
                rigShouldBeDecreased = false;
            }
        }
    }

    private void PlayWeaponsSwitchingAnimation(WeaponTypes weaponType)
    {
        rigUpSteps = switchWeaponsRigUpSteps;
        rigDownSteps = switchWeaponsRigDownSteps;
        PauseRig();
        animator.SetFloat("weaponType", ((float)weaponType));
        animator.SetTrigger("switchWeapons");
    }

    public void ReturnRigWeightToOne()
    {
        rigShouldBeIncreased = true;
    }

    private void CheckWeaponSwitch()
    {

       if (!isLockedInAction)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isLockedInAction = true;
                //animator.SetBool("isLockedInAction", isLockedInAction);
                //animator.SetBool("isLockedInAction", true);
                SwitchGuns(pistol);
                SwitchAnimationLayers(1);
                PlayWeaponsSwitchingAnimation(WeaponTypes.Light);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                isLockedInAction = true;
                //animator.SetBool("isLockedInAction", isLockedInAction);
                //animator.SetBool("isLockedInAction", true);
                SwitchGuns(revolver);
                SwitchAnimationLayers(1);
                PlayWeaponsSwitchingAnimation(WeaponTypes.Light);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isLockedInAction = true;
                //animator.SetBool("isLockedInAction", isLockedInAction);
                //animator.SetBool("isLockedInAction", true);
                SwitchGuns(autoRifle);
                SwitchAnimationLayers(1);
                PlayWeaponsSwitchingAnimation(WeaponTypes.Heavy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                isLockedInAction = true;
                //animator.SetBool("isLockedInAction", isLockedInAction);
                //animator.SetBool("isLockedInAction", true);
                SwitchGuns(shotgun);
                SwitchAnimationLayers(2);
                PlayWeaponsSwitchingAnimation(WeaponTypes.Heavy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                isLockedInAction = true;
                //animator.SetBool("isLockedInAction", isLockedInAction);
                //animator.SetBool("isLockedInAction", true);
                SwitchGuns(rifle);
                SwitchAnimationLayers(3);
                PlayWeaponsSwitchingAnimation(WeaponTypes.Heavy);
            }
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


public enum WeaponTypes { Light, Heavy}