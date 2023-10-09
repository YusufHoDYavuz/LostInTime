using UnityEngine;

public class CloseCombat : MonoBehaviour
{
    public float clickCooldown = 0.2f;
    public static int numberOfPunchs = 0;
    private float clickTimer = 0;

    public Animator animator;
    public static CloseCombat closeCombat;
    public static float range = 1f;

    private void Start()
    {
        if (closeCombat == null)
            closeCombat = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetBool("isWalk") && !animator.GetBool("isRun") &&
            animator.GetBool("isGrounded"))
            PunchRequest();
    }

    void PunchRequest()
    {
        if (Time.time - clickTimer > clickCooldown && numberOfPunchs < 2)
        {
            clickTimer = Time.time;
            animator.SetInteger("PunchCount", ++numberOfPunchs);
            PlayerController.canMove = false;
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Punch2") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("Punch1"))
        {
            PlayerController.canMove = true;
            animator.SetInteger("PunchCount", numberOfPunchs = 0);
        }
    }

    public static void PunchCollision()
    {
        var ray = new Ray(closeCombat.transform.position, closeCombat.transform.forward);
        Debug.DrawRay(closeCombat.transform.position, closeCombat.transform.forward * range, Color.red, 1f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                hit.transform.gameObject.GetComponent<Enemy_AI>().Die(100);
                Destroy(hit.transform.gameObject, 10f);
                Debug.Log("Enemy Hit");
            }
        }
    }
}