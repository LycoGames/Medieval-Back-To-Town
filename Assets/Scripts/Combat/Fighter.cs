using Photon.Pun;
using UnityEngine;

public class Fighter : MonoBehaviour, IPunObservable
{
    [SerializeField] WeaponConfig defaultWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    const string weaponName = "Unarmed";

    public static Fighter localPlayer;
    private Animator anim;

    //Weapon
    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;

    //Network
    PhotonView myPV;

    private void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
            localPlayer = this;

        WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
        EquipWeapon(weapon);

        if (!myPV.IsMine)
            return;


        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine) { return; }
    }

    private Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    public bool AttackBehaviour()
    {
        TriggerAttack();
        return anim.GetBool("isAttacking");
    }

    void Hit()
    {
        //Debug.Log("Yumruk Atıldı.");
    }

    private void AttackAnimEnded()
    {
        if (!myPV.IsMine) return;
        anim.SetBool("isAttacking", false);
    }

    private void TriggerAttack()
    {
        if (!anim.GetBool("isAttacking") && Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("attack");
            anim.SetBool("isAttacking", true);
        }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.IsWriting)
        {
            stream.SendNext(anim.GetBool("isAttacking"));
        }
        else
        {
            anim.SetBool("isAttacking", (bool)stream.ReceiveNext());
        }*/
    }
}
