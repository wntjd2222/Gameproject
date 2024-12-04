using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class UnitCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/

public enum UnitCode
{
    swordman,
    enemy1
}

public class Status
{
    public equiplist equip;

    public UnitCode unitCode { get; }
    public string name { get; set; }
    public float maxHp { get; set; }
    public float nowHp { get; set; }
    public float atkDmg { get; set; }
    public float atkSpeed { get; set; }
    public float moveSpeed { get; set; }
    public float atkRange { get; set; }
    public float fieldOfVision { get; set; }

    public Status()
    {
    }

    public Status(UnitCode unitCode, string name, float maxHp, float atkDmg, float atkSpeed, 
                  float moveSpeed, float atkRange, float fieldOfVision)
    {
        this.unitCode = unitCode;
        this.name = name;
        this.maxHp = maxHp;
        nowHp = maxHp;
        this.atkDmg = atkDmg;
        this.atkSpeed = atkSpeed;
        this.moveSpeed = moveSpeed;
        this.atkRange = atkRange;
        this.fieldOfVision = fieldOfVision;
    }

    public Status SetUnitStatus(UnitCode unitCode)
    {
        Status status = null;

        switch (unitCode)
        {
            case UnitCode.swordman:
                status = new Status(unitCode, "¼Òµå¸Ç", equip.maxHp, equip.atkDmg, 1f, 8f, 0.0f, 0.0f);
                break;
            case UnitCode.enemy1:
                status = new Status(unitCode, "Enemy1", 100, 10, 1.5f, 2f, 1.5f, 7f);
                break;
        }
        return status;
    }
}