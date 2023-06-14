using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warehouse
{
    //public string userid;
    public int item_id;
    public string item_name;
    public float quantity;

    public warehouse(int itemid,string name,float qty)
    {
        //this.userid = usrid;
        this.item_name = name;
        this.quantity = qty;
        this.item_id = itemid;
    }
}
