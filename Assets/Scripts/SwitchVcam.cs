using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVcam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int pariorityBoostAmount=10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas AimCanvas;
    private CinemachineVirtualCamera virtualCamera; 
    private InputAction aimAction;
    // Start is called before the first frame update

    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }
    private void OnEnable() {
        aimAction.performed += _ =>startAim();
        aimAction.canceled += _=>cancelAim();
    }
    private void OnDisable() {
        aimAction.performed -= _ =>startAim();
        aimAction.canceled -= _=>cancelAim();
    }
    private void startAim(){
        virtualCamera.Priority +=pariorityBoostAmount;
        AimCanvas.enabled=true;
        thirdPersonCanvas.enabled=false;

    }
     private void cancelAim(){
        virtualCamera.Priority -=pariorityBoostAmount;
        AimCanvas.enabled=false;
        thirdPersonCanvas.enabled=true;
        
    }
}
