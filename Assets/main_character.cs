using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main_character : MonoBehaviour
{
    public GameObject character;
    Animator animator;

    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed = 1;
    public bool attacked = false;
    public Image nowHpbar;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        float h = Input.GetAxis("Horizontal");
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

        if (Input.GetKeyDown(KeyCode.C) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("attack");
        }
    }
}
