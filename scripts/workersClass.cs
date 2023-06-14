using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workersClass
{
    public int skills;
    public float wage;
    public float productivityRatio;

    public  workersClass(int skillPoint,float wage,float sumSkills)
    {
        this.skills= skillPoint;
        this.wage= wage;
        this.productivityRatio = sumSkills;
    }

}
