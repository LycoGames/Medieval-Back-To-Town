using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Fighter : MonoBehaviour
{
    [SerializeField] private WeaponConfig defaultWeapon = null;
    [SerializeField] private Transform rightHandTransform = null;
    [SerializeField] private Transform leftHandTransform = null;

    private const string weaponName = "Unarmed";

    //Animation
    protected Animator animator;
    protected WStateMachine stateMachine;

    //Weapon
    private WeaponConfig currentWeaponConfig;
    private LazyValue<Weapon> currentWeapon;

    public abstract void BasicAttack(InputAction.CallbackContext ctx);
    protected abstract void AssignAnimationIDs();


    private void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        currentWeapon.ForceInit();
    }

    protected void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<WStateMachine>();
        AssignAnimationIDs();
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        GetComponent<Equipment>().updateEquipmentUiStatsEvent.Invoke();
        currentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    public void EquipUnarmed()
    {
        GetComponent<Equipment>().updateEquipmentUiStatsEvent.Invoke();
        SetupDefaultWeapon();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon.value;
    }

    public WeaponConfig GetCurrentWeaponConfig()
    {
        return currentWeaponConfig;
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
}