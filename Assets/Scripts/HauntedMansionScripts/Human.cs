using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour
{
    private Transform playerGhost;

    public Animator humanAnim;
    private Rigidbody humanRigid;
    private AudioSource humanAudio;
    public AudioClip humanScream;    

    public float humanHealth;
    public float humanSanity;
    public float humanSpeed;
    public float humanRange;

    private Ray humanRay;
    private RaycastHit humanHit;
    private bool humanRunning;
    private int PlayerMask = 1 << 8;

    void Start()
    {
        //playerGhost = PlayerGhost.player.transform;
        humanRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (humanRunning)
        {            
            humanRigid.velocity = transform.forward * humanSpeed;
            humanAnim.SetFloat("Speed", humanRigid.velocity.magnitude);
        }
        else
        {
            humanRigid.velocity = Vector3.zero;
            humanAnim.SetFloat("Speed", humanRigid.velocity.magnitude);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerGhost")
        {
            if (!humanRunning)
            {
                //humanAudio.PlayOneShot(humanScream);
               // new Task(RunFromPlayer());
            }          
        }         
    }

    IEnumerator RunFromPlayer()
    {
        humanRigid.transform.LookAt(playerGhost, Vector3.up);
        humanAnim.SetBool("Startle", true);
        yield return new WaitForSeconds(0.5f);
        humanRigid.transform.rotation = Quaternion.LookRotation(humanRigid.transform.position - playerGhost.transform.position);
        humanAnim.SetBool("Startle", false);
        humanRunning = true;
        yield return new WaitForSeconds(2f);
        humanRunning = false;
    }

    void LookForPlayer()
    {
        //humanRay = new Ray(transform.position, transform.forward);
        //humanAct = Physics.SphereCast(humanRay, transform.localScale.y / 2, humanRange, PlayerMask);
    }
}
