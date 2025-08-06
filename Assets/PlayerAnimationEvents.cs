using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ResumeRigWeightAfterAnimation()
    {
        visualController.ReturnRigWeightToOne();
    }

    public void OnActionLockEnd()
    {
        visualController.SetLockInAction(false);
        
    }
}
