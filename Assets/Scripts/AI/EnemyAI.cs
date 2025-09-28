using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour {
    [SerializeField] private List<SteeringBehaviour> steerings;
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;

    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;
    [SerializeField] private float attackRange = 1f;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private ContextSolver contextSolver;

    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    private bool following = false;

    private void Start() {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
        InvokeRepeating("PerformDirection", 0, aiUpdateDelay);
    }

    private void Update() {
        if (aiData.currentTarget != null) {
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false) {
                following = true;
            }
        }
        else if (aiData.GetTargetCount() > 0) {
            aiData.currentTarget = aiData.targets[0];
        }
        OnMovementInput?.Invoke(movementInput);
    }

    private void PerformDetection() {
        foreach (Detector detector in detectors) {
            detector.Detect(aiData);
        }
    }

    private void PerformDirection() {
        Vector2 bestDir = contextSolver.GetDirToMove(steerings, aiData);
        float randomAngle = Random.Range(-60f, 60f);
        Quaternion rot = Quaternion.Euler(0, 0, randomAngle);
        movementInput = (rot * bestDir).normalized;
    }


}
