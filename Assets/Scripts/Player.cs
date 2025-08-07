using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    public PlayerAimController aimController { get; private set; }
    public PlayerMovementController moveController { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();
        aimController = GetComponent<PlayerAimController>();
        moveController = GetComponent<PlayerMovementController>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
