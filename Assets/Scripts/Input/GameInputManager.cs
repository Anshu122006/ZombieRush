using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour {
    public event EventHandler OnShootPerformed;
    public event EventHandler OnTestPerformed;
    public event EventHandler OnPrevWeaponPerformed;
    public event EventHandler OnNextWeaponPerformed;

    private InputActions inputActions;
    private readonly List<Vector2> activeDirs = new(); // maintains press order
    public static GameInputManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        inputActions = new InputActions();
    }

    private void OnEnable() {
        inputActions.Player.Enable();

        inputActions.Player.Shoot.performed += Input_OnShootPerformed;
        inputActions.Player.TestButton.performed += Input_OnTestPerformed;
        inputActions.Player.PrevWeapon.performed += Input_OnPrevWeaponPerformed;
        inputActions.Player.NextWeapon.performed += Input_OnNextWeaponPerformed;

        RegisterDir(inputActions.Player.Up, Vector2.up);
        RegisterDir(inputActions.Player.Down, Vector2.down);
        RegisterDir(inputActions.Player.Left, Vector2.left);
        RegisterDir(inputActions.Player.Right, Vector2.right);
    }

    private void OnDisable() {
        inputActions.Player.Disable();
    }

    private void RegisterDir(InputAction action, Vector2 dir) {
        action.performed += ctx => OnDirection(dir, true);
        action.canceled += ctx => OnDirection(dir, false);
    }

    private void OnDirection(Vector2 dir, bool pressed) {
        if (pressed) {
            activeDirs.Remove(dir);
            activeDirs.Add(dir);
        }
        else {
            activeDirs.Remove(dir);
        }
    }

    public Vector2 GetInputDir() {
        if (activeDirs.Count == 0) return Vector2.zero;
        return activeDirs[^1];
    }

    public Vector2 GetMousePosition() {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    public bool IsShooting() {
        return inputActions.Player.Shoot.ReadValue<float>() > 0.3f;
    }

    private void Input_OnShootPerformed(InputAction.CallbackContext context)
        => OnShootPerformed?.Invoke(this, EventArgs.Empty);
    private void Input_OnTestPerformed(InputAction.CallbackContext context)
        => OnTestPerformed?.Invoke(this, EventArgs.Empty);
    private void Input_OnPrevWeaponPerformed(InputAction.CallbackContext context)
        => OnPrevWeaponPerformed?.Invoke(this, EventArgs.Empty);
    private void Input_OnNextWeaponPerformed(InputAction.CallbackContext context)
        => OnNextWeaponPerformed?.Invoke(this, EventArgs.Empty);
}
