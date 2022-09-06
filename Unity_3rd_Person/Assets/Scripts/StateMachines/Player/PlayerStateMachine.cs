using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float RunSpeedRatio { get; private set; }
    [field: SerializeField] public float WalkSpeedRatio { get; private set; }
    [field: SerializeField] public float TargetingSpeedRatio { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    private float currentSpeed = 0f;
    private float currentSpeedRatio = 0f;
    private bool isWalkingNext = true;

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        currentSpeed = MaxSpeed;
        currentSpeedRatio = RunSpeedRatio;

        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    public void SetToWalking()
    {
        isWalkingNext = false;
        currentSpeed = MaxSpeed * WalkSpeedRatio;
        currentSpeedRatio = WalkSpeedRatio;
    }

    public void SetToRunning()
    {
        isWalkingNext = true;
        currentSpeed = MaxSpeed * RunSpeedRatio;
        currentSpeedRatio = RunSpeedRatio;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetCurrentSpeedRatio()
    {
        return currentSpeedRatio;
    }

    public bool IsWalking()
    {
        return !isWalkingNext;
    }
}
