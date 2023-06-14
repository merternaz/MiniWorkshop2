using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orderDatabase : MonoBehaviour
{
    public static List<ordersPropert> ordersData = new List<ordersPropert>();
    public static List<ordersPropertColl> ordersDataCollection = new List<ordersPropertColl>();
    public static int counter=0;
    //public static ordersPropert GetOrderId(int)

    public void InsertOrder(int custid,int itemid,float qty,int day,float createTime,ItemType typp,float dealPrice)
    {
        
        ordersData = new List<ordersPropert>()
        {
            new ordersPropert(counter,custid,itemid,qty,day,createTime,typp,dealPrice),
        };
        

        ordersDataCollection.Add(
            new ordersPropertColl(counter, custid, itemid, qty, day, createTime,typp,dealPrice)
        );

        counter++;

    }

    public static ordersPropertColl GetOrderID(int id)
    {
        return ordersDataCollection.Find(x => x.orderid == id);
    }

    public static void DeleteOrderID(int id)
    {
        for(int i = 0; i < ordersDataCollection.Count; i++)
        {
            if (ordersDataCollection[i].orderid == id)
            {
                ordersDataCollection.RemoveAt(i);
            }
        }
        
    }

    public static ordersPropertColl GetCollectionOrderId(int ord_id)
    {
        return ordersDataCollection.Find(x => x.orderid == ord_id);
    }
}
