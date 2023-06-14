using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stockItems 
{
    public int itemID;
    public float itemQuantity;

    public stockItems(int id,float quantity)
    {
        this.itemID = id;
        this.itemQuantity = quantity;
    }
}
