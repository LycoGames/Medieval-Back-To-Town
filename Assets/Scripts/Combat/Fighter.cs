using System;
using Photon.Pun;
using UnityEngine;

public class Fighter : MonoBehaviour, IPunObservable
{
    public static Fighter localPlayer;
    private Animator anim;

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
        if (!myPV.IsMine) { return; }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(anim.GetBool("isAttacking"));
        }
        else
        {
            anim.SetBool("isAttacking", (bool)stream.ReceiveNext());
        }
    }
}
