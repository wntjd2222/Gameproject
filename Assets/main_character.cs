using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_character : MonoBehaviour
{
    public GameObject character;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        character.transform.position = new Vector3(0,0,0);
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
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
