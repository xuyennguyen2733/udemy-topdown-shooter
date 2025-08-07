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
    [Range(1f, 5f)]
    [SerializeField] private float minCameraDistance;

    [Range(1f, 5f)]
    [SerializeField] private float maxCameraDistance;

    [Range(1f,5f)]
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private LayerMask aimLayerMask;


    public bool preciseAimEnabled { get; private set; }

    private Vector2 aimInput;
    private RaycastHit lastKnownMouseInput;

    private void Start()
    {
        player = GetComponent<Player>();
        preciseAimEnabled = true;
        controls = player.controls;
        AssignInputEvents();
    }
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.P))
        {
            preciseAimEnabled = !preciseAimEnabled;
        }
        UpdateCameraPosition();
        UpdateAimPosition();
    }

    private void UpdateAimPosition()
    {
        aim.position = GetMouseHitInfo().point;
        if (!preciseAimEnabled)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y, aim.position.z);
        }
    }

    private void UpdateCameraPosition()
    {
        cameraFollowTarget.position = Vector3.Lerp(cameraFollowTarget.position, GetDesiredCameraPostion(), cameraSensitivity * Time.deltaTime);
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private Vector3 GetDesiredCameraPostion()
    {
        float actualMaxCameraDistance = player.moveController.isMovingBackward() ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float desiredCameraDistance = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(desiredCameraDistance, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition= transform.position + aimDirection * clampedDistance;

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
