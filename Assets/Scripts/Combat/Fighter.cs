using System;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] WeaponConfig defaultWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] GameObject arissaHead = null;

    public GameObject HandArrow;
    const string weaponName = "Unarmed";

    Animator anim;
    Health target;
    //Weapon
    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;

    private void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        currentWeapon.ForceInit();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GetComponent<Health>();
        GetActive();
    }

    private void GetActive()
    {
        HandArrow.gameObject.SetActive(false);
    }

    void Update()
    {

        Debug.DrawRay(arissaHead.transform.position, GetAimPosition(), Color.green);

    }

    public Vector3 GetAimPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.transform.tag == "Enemy")
        {
            //Debug.Log("Mouse is on the Enemy");
            return hitData.point;
        }

        return hitData.point;

    }

    private Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    public void AttackBehaviour()
    {
        anim.SetTrigger("attack");
        //LookMousePosition();
    }

    void Hit()
    {
        HandArrow.gameObject.SetActive(false);
        /*
                  if (currentWeaponConfig.HasProjectile())
                  {
                      currentWeaponConfig.LaunchArrow(rightHandTransform, leftHandTransform, target, gameObject, GetWeaponDamage(), GetMouseWorldPosition());
                  }
        */

        currentWeaponConfig.LaunchArrow(rightHandTransform, leftHandTransform, gameObject, GetWeaponDamage(), GetAimPosition());


        //Debug.Log("Yumruk Atıldı.");

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            return mouseWorldPosition;
        }
        return new Vector3();

    }

    public void Shoot()
    {

        Hit();
    }

    public void DrawArrow()
    {
        //Instantiate(arrow, GameObject.Find("HandArrow").transform.position, Quaternion.LookRotation());
        HandArrow.gameObject.SetActive(true);
        print("ok cekildi.");

    }

    private bool LookMousePosition()
    {
        float cameraRotation = Camera.main.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraRotation, 0);
        if (transform.rotation == Quaternion.Euler(0, cameraRotation, 0))
        {
            return true;
        }
        return false;
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

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}
