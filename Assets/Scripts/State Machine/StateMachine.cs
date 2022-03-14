using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InputSystem;
using UnityEditor.UIElements;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    [SerializeField] float velocity = 9;
    [SerializeField] float rotationSmoothTime = 0.12f;
    [SerializeField] float targetingSense = 2;
    [SerializeField] bool canMove;
    [SerializeField] float aimTimer = 0;
    [SerializeField] Transform FirePoint;


    [Header("Weapon")] [SerializeField] private WeaponConfig defaultWeapon = null;
    [SerializeField] private Transform rightHandTransform = null;
    [SerializeField] private Transform leftHandTransform = null;


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
    StateFactory states;
    Vector3 desiredMoveDirection;
    bool blockRotationPlayer;
    float desiredRotationSpeed = 0.1f;
    Animator anim;
    float Speed;
    Camera cam;
    CharacterController controller;
    bool isGrounded;
    float secondLayerWeight = 0;

    bool casting = false;
    GameObject TargetMarker;
    Transform parentObject;


    private AudioSource soundComponent; //Play audio from Prefabs
    private AudioClip clip;
    private AudioSource soundComponentCast; //Play audio from PrefabsCast

    float verticalVel;

    Vector3 moveVector;

    bool activeTarget = false;


    private Inputs input;
    public float InputX;
    public float InputY;

//Player move temp variables
    private Vector3 forward;
    private Vector3 right;

    //Weapon variables
    private const string weaponName = "Unarmed";
    private WeaponConfig currentWeaponConfig;
    private LazyValue<Weapon> currentWeapon;

    private bool rotateState = false;


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

    public BaseState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public StateFactory States
    {
        get => states;
        set => states = value;
    }


    public Inputs Input
    {
        get => input;
        set => input = value;
    }

    public bool LockCameraPosition
    {
        get => lockCameraPosition;
        set => lockCameraPosition = value;
    }

    public GameObject AimUI
    {
        get => aimUI;
        set => aimUI = value;
    }

    public AIConversant InteractableNPC
    {
        get => interactableNPC;
        set => interactableNPC = value;
    }

    public GameObject CinemachineCameraTarget
    {
        get => cinemachineCameraTarget;
        set => cinemachineCameraTarget = value;
    }

    public float TopClamp
    {
        get => topClamp;
        set => topClamp = value;
    }

    public float BottomClamp
    {
        get => bottomClamp;
        set => bottomClamp = value;
    }

    public float CameraAngleOverride
    {
        get => cameraAngleOverride;
        set => cameraAngleOverride = value;
    }


    public Animator Anim
    {
        get => anim;
        set => anim = value;
    }

    public CharacterController Controller
    {
        get => controller;
        set => controller = value;
    }


    public Vector3 Forward
    {
        get => forward;
        set => forward = value;
    }

    public Vector3 Right
    {
        get => right;
        set => right = value;
    }

    public float DesiredRotationSpeed
    {
        get => desiredRotationSpeed;
        set => desiredRotationSpeed = value;
    }

    public float Velocity
    {
        get => velocity;
        set => velocity = value;
    }

    public float RotationSmoothTime
    {
        get => rotationSmoothTime;
        set => rotationSmoothTime = value;
    }

    public float TargetRotation { get; set; } = 0.0f;

    public float AimTimer
    {
        get => aimTimer;
        set => aimTimer = value;
    }

    public float SecondLayerWeight
    {
        get => secondLayerWeight;
        set => secondLayerWeight = value;
    }

    public Vector3 DesiredMoveDirection
    {
        get => desiredMoveDirection;
        set => desiredMoveDirection = value;
    }

    public Camera Cam
    {
        get => cam;
        set => cam = value;
    }

    public Vector2 UiOffset
    {
        get => uiOffset;
        set => uiOffset = value;
    }

    public LayerMask CollidingLayer
    {
        get => collidingLayer;
        set => collidingLayer = value;
    }

    public Image Aim
    {
        get => aim;
        set => aim = value;
    }

    public bool ActiveTarget
    {
        get => activeTarget;
        set => activeTarget = value;
    }

    public List<Transform> ScreenTargets
    {
        get => screenTargets;
        set => screenTargets = value;
    }

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public bool RotateState
    {
        get => rotateState;
        set => rotateState = value;
    }

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    public float TargetingSense
    {
        get => targetingSense;
        set => targetingSense = value;
    }

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    public WeaponConfig CurrentWeaponConfig
    {
        get => currentWeaponConfig;
        set => currentWeaponConfig = value;
    }

    //User interface variables


    void Awake()
    {
        states = new StateFactory(this);
        currentState = states.AppState();
        currentState.EnterState();
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

        if (anim.layerCount > 1)
        {
            anim.SetLayerWeight(1, secondLayerWeight);
        }
    }

    public void InputMagnitude()
    {
        if (!canMove)
        {
            SetAnimZero();
            return;
        }

        InputX = input.move.x;
        InputY = input.move.y;

        //TODO anim input z ve inputx set edildi 651
        anim.SetFloat(animIDInputY, InputY, VerticalAnimTime, Time.deltaTime * 2f);
        anim.SetFloat(animIDInputX, InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        Speed = new Vector2(input.move.x, input.move.y).sqrMagnitude;
        //TODO 658 stateler ile halledildi.

        PlayerMoveAndRotationInput();
    }

    private void PlayerMoveAndRotationInput()
    {
        InputX = input.move.x;
        InputY = input.move.y;

        forward = cam.transform.forward;
        right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputY + right * InputX;

        desiredMoveDirection.Normalize();
    }

    private void GetRequiredComponents()
    {
        input = GetComponent<Inputs>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        if (target)
        {
            target = screenTargets[targetIndex()];
        }

        controller = GetComponent<CharacterController>();
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
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel = 0;
        }
        else
        {
            verticalVel -= 1f * Time.deltaTime;
        }

        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);
    }

    public void StartRotateCoroutine(float rotatingTime, Vector3 targetPoint)
    {
        StartCoroutine(RotateToTarget(rotatingTime, targetPoint));
    }

    private IEnumerator RotateToTarget(float rotatingTime, Vector3 targetPoint)
    {
        rotateState = true;
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
                rotateState = false;
                yield break;
            }

            yield return null;
        }
    }

    public void PerformBasicAttack()
    {
        anim.SetTrigger(animIDMaskAttack1);
        secondLayerWeight = Mathf.Lerp(secondLayerWeight, 0.5f, Time.deltaTime * 60);
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
        currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, 20,
            uiOffset);
        //StartCoroutine(cameraShaker.Shake(0.1f, 2, 0.2f, 0));
    }

    public int targetIndex()
    {
        float[] distances = new float[screenTargets.Count];
        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(cam.WorldToScreenPoint(screenTargets[i].position),
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
        Gizmos.DrawWireSphere(transform.position, currentWeaponConfig ? currentWeaponConfig.GetRange() : 0f);
    }

    public void InitializeWeapon()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        currentWeapon.ForceInit();
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon.value;
    }

    public float GetWeaponDamage()
    {
        return currentWeaponConfig.GetDamage();
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
        anim.SetFloat(animIDInputY, 0);
        anim.SetFloat(animIDInputX, 0);
    }
}