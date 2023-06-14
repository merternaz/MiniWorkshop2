using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobScript 
{
    
    // Start is called before the first frame update

    public int jobID;
    public int orderID;
    public ItemType jobType;
    public float orderQty;
    public float productQty;
    public bool isDone;


    public JobScript(int id,int orderid,ItemType tip,float ordQty)
    {
        this.jobID = id;
        this.orderID = orderid;
        this.jobType = tip;
        this.orderQty = ordQty;
        this.productQty = 0;
        this.isDone = false;

    }
}
