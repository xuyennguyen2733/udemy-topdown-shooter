using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Info")]
    [SerializeField] private float minCameraDistance;
    [SerializeField] private float maxCameraDistance;
    [SerializeField] private float aimSensitivity;
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
        aim.position = GetAimPostion();
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private Vector3 GetAimPostion()
    {
        Vector3 desiredAimPosition = GetMousePosition();
        Vector3 aimDirection = (desiredAimPosition - transform.position).normalized;
        float aimDistanceFromPlayer = Vector3.Distance(transform.position, desiredAimPosition);

        if (aimDistanceFromPlayer > maxCameraDistance)
        {
            desiredAimPosition = transform.position + aimDirection * maxCameraDistance;
        }
        else if (aimDistanceFromPlayer < minCameraDistance)
        {
            desiredAimPosition = transform.position + aimDirection * minCameraDistance;
        }

        desiredAimPosition.y = transform.position.y;

            return desiredAimPosition;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
