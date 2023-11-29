using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player1"))
        {
            maincharc swordman = col.GetComponent<maincharc>();
            swordman.nowHp -= 10;
            if (swordman.nowHp < 0) swordman.nowHp = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
