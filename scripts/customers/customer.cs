using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customer 
{

    public int cust_id;
    public ItemType sector;
    public string cust_name;
    public Dictionary<string, int> OrderDemand = new Dictionary<string, int>();


    public customer(int id,ItemType sector,string name,Dictionary<string,int> demand)
    {
        this.cust_id = id;
        this.sector = sector;
        this.cust_name = name;
        this.OrderDemand = demand;

    }
}
