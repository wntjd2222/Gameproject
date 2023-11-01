using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maincharc : MonoBehaviour
{
    public GameObject character;
    Animator animator;

    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;
    public Image nowHpbar;
    public float jumpPower = 60;
    public float moveSpeed = 2;

    bool inputJump = false;
    bool inputRight = false;
    bool inputLeft = false;
    Rigidbody2D rigid2D;

    BoxCollider2D col2D;

    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
    void SetAttackSpeed(float speed)
    {
        animator.SetFloat("attackSpeed", speed);
        atkSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 50;
        nowHp = 50;
        atkDmg = 10;

        character.transform.position = new Vector3(0,0,0);
        animator = GetComponent<Animator>();
        SetAttackSpeed(1.5f);

        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        /*float h = Input.GetAxis("Horizontal");
        if (h > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        */
        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputLeft = true;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
        }
        else animator.SetBool("moving", false);
        if (Input.GetKey(KeyCode.C) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AttackTrue();
            animator.SetTrigger("attack");
            AttackFalse();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jumping"))
        {
            inputJump = true;
        }
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
            animator.SetBool("jumping", false);
        else animator.SetBool("jumping", true);
    }

    private void FixedUpdate()
    {
        if(inputRight)
        {
            inputRight = false;
            //rigid2D.AddForce(Vector2.right * moveSpeed);
            rigid2D.velocity = new Vector2(moveSpeed, rigid2D.velocity.y);
        }
        if(inputLeft)
        {
            inputLeft = false;
            //rigid2D.AddForce(Vector2.left * moveSpeed);
            rigid2D.velocity = new Vector2(-moveSpeed, rigid2D.velocity.y);
        }
        //if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        //else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
        if(inputJump)
        {
            inputJump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
        }
    }

   /* private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            nowHp -= 10;
            Debug.Log(nowHp);
            if (nowHp <= 0)
            {
                Destroy(gameObject);
                Destroy(nowHpbar.gameObject);
            }
        }
    }*/
}
