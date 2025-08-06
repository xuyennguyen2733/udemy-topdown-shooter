using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Control")]
    [SerializeField] private Transform aim;

    [Header("Camera Control")]
    [SerializeField] private float minCameraDistance;
    [SerializeField] private float maxCameraDistance;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private LayerMask aimLayerMask;

    [Space]

    private Vector2 aimInput;
    private RaycastHit lastKnownMouseInput;

    private void Start()
    {
        player = GetComponent<Player>();

        controls = player.controls;
        AssignInputEvents();
    }
    private void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y, aim.position.z);

        cameraFollowTarget.position = GetDesiredCameraPostion();
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private Vector3 GetDesiredCameraPostion()
    {
        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;
        float aimDistanceFromPlayer = Vector3.Distance(transform.position, desiredCameraPosition);

        if (aimDistanceFromPlayer > maxCameraDistance)
        {
            desiredCameraPosition = transform.position + aimDirection * maxCameraDistance;
        }
        else if (aimDistanceFromPlayer < minCameraDistance)
        {
            desiredCameraPosition = transform.position + aimDirection * minCameraDistance;
        }

        desiredCameraPosition.y = transform.position.y;

            return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseInput = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseInput;
    }
}
