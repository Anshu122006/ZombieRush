using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class GameInputManager : MonoBehaviour {
    public event EventHandler OnShootPerformed;
    public event EventHandler OnTestPerformed;
    public event EventHandler OnPrevWeaponPerformed;
    public event EventHandler OnNextWeaponPerformed;


    private InputActions inputActions;
    public static GameInputManager Instance { get; private set; }

    private void Awake() {
        Instance = GetComponent<GameInputManager>();
        inputActions = new InputActions();
    }

    private void OnEnable() {
        inputActions.Player.Enable();
        inputActions.Player.TestButton.performed += Input_OnTestPerformed;
        inputActions.Player.PrevWeapon.performed += Input_OnPrevWeaponPerformed;
        inputActions.Player.NextWeapon.performed += Input_OnNextWeaponPerformed;
    }

    private void OnDisable() {
        inputActions.Player.TestButton.performed -= Input_OnTestPerformed;
        inputActions.Player.PrevWeapon.performed -= Input_OnPrevWeaponPerformed;
        inputActions.Player.NextWeapon.performed -= Input_OnNextWeaponPerformed;
        inputActions.Player.Disable();
    }


    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool IsShooting() {
        float isShooting = inputActions.Player.Shoot.ReadValue<float>();

        return isShooting > 0.3f;
    }

    private void Input_OnShootPerformed(InputAction.CallbackContext context) {
        OnShootPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Input_OnTestPerformed(InputAction.CallbackContext context) {
        OnTestPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Input_OnPrevWeaponPerformed(InputAction.CallbackContext context) {
        OnPrevWeaponPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Input_OnNextWeaponPerformed(InputAction.CallbackContext context) {
        OnNextWeaponPerformed?.Invoke(this, EventArgs.Empty);
    }
}