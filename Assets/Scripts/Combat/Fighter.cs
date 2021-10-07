using Photon.Pun;
using UnityEngine;

public class Fighter : MonoBehaviour, IPunObservable
{
    public static Fighter localPlayer;
    private Animator anim;
    private bool syncAttackTrigger = false;

    //Network
    PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
            localPlayer = this;


        if (!myPV.IsMine)
            return;


        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (syncAttackTrigger)
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("attack");
            syncAttackTrigger = false;
        }

        if (!myPV.IsMine) { return; }

        AttackBehaviour();
    }

    private void AttackBehaviour()
    {
        TriggerAttack();
    }

    private void TriggerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("attack");
            syncAttackTrigger =true;
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(syncAttackTrigger);
        }
        else
        {
            syncAttackTrigger = (bool)stream.ReceiveNext();
        }
    }
}
