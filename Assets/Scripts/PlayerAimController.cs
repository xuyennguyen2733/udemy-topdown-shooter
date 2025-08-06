using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();

        controls = player.controls;
        AssignInputEvents();
    }

    private void Update()
    {
        GetMousePosition();
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
