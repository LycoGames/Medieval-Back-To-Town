using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InputSystem;
using UnityEditor.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    [SerializeField] float velocity = 9;
    [SerializeField] float rotationSmoothTime = 0.12f;
    [SerializeField] float targetingSense = 2;
    [SerializeField] float aimTimer = 0;
    [SerializeField] Transform FirePoint;


    [Header("Weapon")] [SerializeField] private WeaponConfig defaultWeapon = null;
    [SerializeField] private Transform rightHandTransform = null;
    public Transform GetRightHandTransform() { return rightHandTransform; }
    [SerializeField] private Transform leftHandTransform = null;
    public Transform GetLeftHandTransform() { return leftHandTransform; }
    [SerializeField] private Transform head = null;
    public Transform GetHeadTransform() { return head; }
    [SerializeField] LayerMask collidingLayer = ~0; //Target marker can only collide with scene layer

    //[SerializeField] GameObject TargetMarker2;
    [SerializeField] GameObject[] Prefabs;

    [SerializeField] GameObject[] PrefabsCast;


    //[SerializeField] float[] castingTime; //If 0 - can loop, if > 0 - one shot time


    [Space] [Header("Canvas")] [SerializeField]
    Image aim;

    [SerializeField] private GameObject aimUI;

    [SerializeField] Vector2 uiOffset;
    [SerializeField] List<Transform> screenTargets = new List<Transform>();

    [SerializeField] float fireRate = 0.1f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField]
    GameObject cinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")] [SerializeField]
    float topClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")] [SerializeField]
    float bottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField]
    float cameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")] [SerializeField]
    bool lockCameraPosition = false;


    [Header("Animation Smoothing")] [Range(0, 1f)] [SerializeField]
    float HorizontalAnimSmoothTime = 0.2f;


    [Range(0, 1f)] [SerializeField] float VerticalAnimTime = 0.2f;
    /*[Range(0, 1f)] [SerializeField] float StartAnimTime = 0.3f;
    [Range(0, 1f)] [SerializeField] float StopAnimTime = 0.15f;*/

    [SerializeField] Transform target;

    [SerializeField] private AIConversant interactableNPC = null;

    //state variables
    BaseState currentState;

    BaseState currentSubState;
    Vector3 desiredMoveDirection;
    bool blockRotationPlayer;
    float Speed;
    bool isGrounded;

    bool casting = false;
    GameObject TargetMarker;
    Transform parentObject;


    private AudioSource soundComponent; //Play audio from Prefabs
    private AudioClip clip;
    private AudioSource soundComponentCast; //Play audio from PrefabsCast

    float verticalVel;

    Vector3 moveVector;


    //Player move temp variables
    private Vector3 forward;
    private Vector3 right;

    //Weapon variables
    private const string weaponName = "Unarmed";
    private LazyValue<Weapon> currentWeapon;


    //Animaiton IDs
    [HideInInspector] public int animIDInputX;

    [HideInInspector] public int animIDInputY;
    [HideInInspector] public int animIDAimMoving;

    [HideInInspector] public int animIDMaskAttack1;
    [HideInInspector] public int animIDAttack1;

    /*[HideInInspector] public int animIDInputMagnitude;

    

    [HideInInspector] public int animIDAoE;

    [HideInInspector] public int animIDAttack2;

    [HideInInspector] public int animIDUpAttack;

    [HideInInspector] public int animIDMaskAttack2;

    [HideInInspector] public int animIDUpAttack2;*/

    //CAMERA ROTATION

    private const float Threshold = 0.01f;
    private const int CursorSwitchSpeed = 1000;


    //User interface variables
    private Vector3 screenCenter;
    private Vector3 screenPos;
    private Vector3 cornerDistance;
    private Vector3 absCornerDistance;
    private Vector3 worldViewField;

    //cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    //Action store
    private ActionStore actionStore;

    //Equipment
    private Equipment equipment;

    public BaseState CurrentState
    {
        set => currentState = value;
    }

    public StateFactory States { get; private set; }


    public Inputs Input { get; private set; }

    public bool LockCameraPosition
    {
        set => lockCameraPosition = value;
    }

    public GameObject AimUI => aimUI;

    public AIConversant InteractableNpc
    {
        get => interactableNPC;
        set => interactableNPC = value;
    }


    public Animator Anim { get; private set; }

    public CharacterController Controller { get; private set; }


    public Vector3 Forward => forward;

    public float DesiredRotationSpeed { get; } = 0.1f;

    public float Velocity => velocity;

    public float RotationSmoothTime => rotationSmoothTime;

    public float TargetRotation { get; set; }

    public float AimTimer
    {
        get => aimTimer;
        set => aimTimer = value;
    }

    public float SecondLayerWeight { get; set; }

    public Vector3 DesiredMoveDirection => desiredMoveDirection;

    public Camera Cam { get; private set; }

    public Vector2 UiOffset => uiOffset;

    public LayerMask CollidingLayer => collidingLayer;

    public Image Aim => aim;

    public bool ActiveTarget { get; set; }

    public List<Transform> ScreenTargets => screenTargets;

    public bool RotateState { get; private set; } = false;

    public float FireRate => fireRate;

    public float TargetingSense => targetingSense;

    public float InputX { get; set; }
    public float InputY { get; set; }

    public bool CanMove { get; set; }

    public List<GameObject> MainUiArray { get; private set; }


    public Transform Target
    {
        get => target;
        set => target = value;
    }

    public WeaponConfig CurrentWeaponConfig { get; private set; }

    public GameObject InventoryUi { get; set; }

    //User interface variables


    void Awake()
    {
        States = new StateFactory(this);
        currentState = States.AppState();
        currentState.EnterState();

        equipment = GetComponent<Equipment>();
        if (equipment)
        {
            equipment.equipmentUpdated += UpdateWeapon;
        }

        MainUiArray = new List<GameObject>();
        MainUiArray.AddRange(GameObject.FindGameObjectsWithTag("MainUI"));
    }

    void Start()
    {
        GetRequiredComponents();
        AssignAnimationIDs();
    }

    void Update()
    {
        currentState.UpdateStates();
        //Need second layer in the Animator

        // if (screenTargets.Count == 0)
        // {
        //     GetEnemies();
        // }

        if (Anim.layerCount > 1)
        {
            Anim.SetLayerWeight(1, SecondLayerWeight);
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void InputMagnitude()
    {
        if (!CanMove)
        {
            SetAnimZero();
            return;
        }

        InputX = Input.move.x;
        InputY = Input.move.y;

        Anim.SetFloat(animIDInputY, InputY, VerticalAnimTime, Time.deltaTime * 2f);
        Anim.SetFloat(animIDInputX, InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        Speed = new Vector2(Input.move.x, Input.move.y).sqrMagnitude;

        PlayerMoveAndRotationInput();
    }


    private void PlayerMoveAndRotationInput()
    {
        InputX = Input.move.x;
        InputY = Input.move.y;

        forward = Cam.transform.forward;
        right = Cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputY + right * InputX;

        desiredMoveDirection.Normalize();
    }

    private void GetRequiredComponents()
    {
        Input = GetComponent<Inputs>();
        Anim = GetComponent<Animator>();
        actionStore = GetComponent<ActionStore>();
        Cam = Camera.main;
        if (target)
        {
            target = screenTargets[targetIndex()];
        }

        Controller = GetComponent<CharacterController>();
    }

    private void AssignAnimationIDs()
    {
        animIDInputX = Animator.StringToHash("InputX");
        animIDInputY = Animator.StringToHash("InputY");
        ;
        animIDAimMoving = Animator.StringToHash("AimMoving");
        ;
        animIDMaskAttack1 = Animator.StringToHash("MaskAttack1");
        animIDAttack1 = Animator.StringToHash("Attack1");
        /*animIDInputMagnitude = Animator.StringToHash("InputMagnitude");
        ;
        
        ;
        animIDAoE = Animator.StringToHash("DAoE");
        ;
        animIDAttack2 = Animator.StringToHash("Attack2");
        ;
        animIDUpAttack = Animator.StringToHash("UpAttack");
        ;
     
        ;
        animIDMaskAttack2 = Animator.StringToHash("MaskAttack2");
        ;
        animIDUpAttack2 = Animator.StringToHash("UpAttack2");
        ;*/
    }

    public void ApplyGravity()
    {
        //If you don't need the character grounded then get rid of this part.
        isGrounded = Controller.isGrounded;
        if (isGrounded)
        {
            verticalVel = 0;
        }
        else
        {
            verticalVel -= 1f * Time.deltaTime;
        }

        moveVector = new Vector3(0, verticalVel, 0);
        Controller.Move(moveVector);
    }

    public void StartRotateCoroutine(float rotatingTime, Vector3 targetPoint)
    {
        StartCoroutine(RotateToTarget(rotatingTime, targetPoint));
    }

    private IEnumerator RotateToTarget(float rotatingTime, Vector3 targetPoint)
    {
        RotateState = true;
        float delay = rotatingTime;
        var lookPos = targetPoint - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 20);
            delay -= Time.deltaTime;
            if (delay <= 0 || transform.rotation == rotation)
            {
                RotateState = false;
                yield break;
            }

            yield return null;
        }
    }

    public void PerformBasicAttack()
    {
        Anim.SetTrigger(animIDMaskAttack1);
        SecondLayerWeight = Mathf.Lerp(SecondLayerWeight, 0.5f, Time.deltaTime * 60);
        try
        {
            PrefabsCast[8].GetComponent<ParticleSystem>().Play();
        }
        catch (Exception e)
        {
            Console.WriteLine(e + " Basic attack efekti yok");
        }

        //float damage = currentWeaponConfig.GetDamage() + GetComponent<BaseStats>().GetStat(Stat.Damage);
        Debug.Log(uiOffset);
        var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        CurrentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage,
            uiOffset);
        //StartCoroutine(cameraShaker.Shake(0.1f, 2, 0.2f, 0));
    }

    public int targetIndex()
    {
        float[] distances = new float[screenTargets.Count];
        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(Cam.WorldToScreenPoint(screenTargets[i].position),
                new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;
        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CurrentWeaponConfig ? CurrentWeaponConfig.GetRange() : 0f);
    }

    public void InitializeWeapon()
    {
        CurrentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        currentWeapon.ForceInit();
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        CurrentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    private Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator weaponAnimator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, weaponAnimator);
    }


    public void SetAnimZero()
    {
        Anim.SetFloat(animIDInputY, 0);
        Anim.SetFloat(animIDInputX, 0);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (Input.look.sqrMagnitude >= Threshold && !lockCameraPosition)
        {
            cinemachineTargetYaw += Input.look.x * Time.deltaTime;
            cinemachineTargetPitch += Input.look.y * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        /* ctx.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
             cinemachineTargetPitch + ctx.CameraAngleOverride,
             cinemachineTargetYaw, 0.0f);*/

        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
            cinemachineTargetPitch,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void UpdateWeapon()
    {
        var weapon = equipment.GetItemInSlot(EquipLocation.PrimaryWeapon) as WeaponConfig;
        EquipWeapon(weapon == null ? defaultWeapon : weapon);
    }


    public void OnAbility1(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(0, gameObject);
        }
    }

    public void OnAbility2(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(1, gameObject);
        }
    }

    public void OnAbility3(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(2, gameObject);
        }
    }

    public void OnAbility4(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(3, gameObject);
        }
    }

    public void OnAbility5(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(4, gameObject);
        }
    }

    public void OnAbility6(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            actionStore.Use(5, gameObject);
        }
    }
}