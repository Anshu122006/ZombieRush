using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour {
    public event EventHandler OnShootPerformed;
    public event EventHandler OnTestPerformed;
    public event EventHandler OnPrevWeaponPerformed;
    public event EventHandler OnNextWeaponPerformed;
    public event EventHandler OnPausePerformed;

    private InputActions inputActions;
    private readonly List<Vector2> activeDirs = new();

    private float weaponSwitchDelay = 0.1f;
    private Coroutine weaponSwitchCoroutine;
    public static GameInputManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        inputActions = new InputActions();
    }

    private void OnEnable() {
        inputActions.Player.Enable();
        inputActions.UI.Enable();

        inputActions.Player.Shoot.performed += Input_OnShootPerformed;
        inputActions.Player.TestButton.performed += Input_OnTestPerformed;
        inputActions.UI.Pause.performed += Input_OnPausePerformed;

        RegisterDir(inputActions.Player.Up, Vector2.up);
        RegisterDir(inputActions.Player.Down, Vector2.down);
        RegisterDir(inputActions.Player.Left, Vector2.left);
        RegisterDir(inputActions.Player.Right, Vector2.right);
    }

    private void OnDisable() {
        inputActions.Player.Disable();
        inputActions.UI.Disable();
    }

    private void Update() {
        HandleWeaponSwitch();
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

    public void DisablePlayerControls() => inputActions.Player.Disable();
    public void EnablePlayerControls() => inputActions.Player.Enable();

    public void HandleWeaponSwitch() {
        bool prev = inputActions.Player.PrevWeapon.ReadValue<float>() > 0.3f;
        bool next = inputActions.Player.NextWeapon.ReadValue<float>() > 0.3f;
        if (weaponSwitchCoroutine == null) {
            if (next) weaponSwitchCoroutine = StartCoroutine(SwitchWeapon(true));
            else if (prev) weaponSwitchCoroutine = StartCoroutine(SwitchWeapon(false));
        }
    }

    private void Input_OnShootPerformed(InputAction.CallbackContext context)
        => OnShootPerformed?.Invoke(this, EventArgs.Empty);
    private void Input_OnTestPerformed(InputAction.CallbackContext context)
        => OnTestPerformed?.Invoke(this, EventArgs.Empty);
    private void Input_OnPausePerformed(InputAction.CallbackContext context)
    => OnPausePerformed?.Invoke(this, EventArgs.Empty);

    private IEnumerator SwitchWeapon(bool next) {
        yield return new WaitForSeconds(weaponSwitchDelay);
        if (next) OnNextWeaponPerformed?.Invoke(this, EventArgs.Empty);
        else OnPrevWeaponPerformed?.Invoke(this, EventArgs.Empty);
        weaponSwitchCoroutine = null;
    }
}
