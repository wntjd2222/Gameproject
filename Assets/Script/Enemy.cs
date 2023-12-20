using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject prfHpBar;
    public GameObject canvas;

    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;

    public Status status;
    public UnitCode unitCode;

    RectTransform hpBar;
    public float height = 1.7f;

    public maincharc sword_man;
    Image nowHpbar;
    public Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        status = new Status();
        status = status.SetUnitStatus(unitCode);

        SetAttackSpeed(atkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.position = _hpBarPos;
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
        /*
        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else if (transform.position.x <= 0.5 && transform.position.x >= -0.5 && transform.position.y <= 0)
        {
            nowHp = 0;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            if (sword_man.attacked)
            {
                nowHp -= sword_man.status.atkDmg;
                sword_man.attacked = false;
                if (nowHp <= 0)
                {
                    /*
                    Destroy(gameObject);
                    Destroy(hpBar.gameObject);
                    */
                    Die();
                }
            }
        }
    }

    void Die()
    {
        enemyAnimator.SetTrigger("die");
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 3);
        Destroy(hpBar.gameObject, 3);
    }

    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
