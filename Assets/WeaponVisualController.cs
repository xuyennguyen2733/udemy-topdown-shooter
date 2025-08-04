using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        SwitchGuns(pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGuns(pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGuns(revolver);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchGuns(autoRifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchGuns(shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchGuns(rifle);
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
}
