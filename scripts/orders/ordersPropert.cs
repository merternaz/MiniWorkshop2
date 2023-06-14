using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ordersPropert
{
    public string customer;
    //public Text Cust;
    public int custid;
    public int itemid;
    public string ItemName;
    //public Text Item;
    public Sprite ItemImg;
    //public Text orderMessage;
    public float quantity;
    public float production; // üretim yapılınca güncellesin. order id den...oyunda oldukça sipariş olur. çıkınca sıfırlanır.veritabanına gerek yok
    public int counter;
    public int orderid;
    public int demand;
    public float orderTime;
    public float lastDemandTime;
    public ItemType itype;
    public float dealPrice;
    public bool finished;
    
    public ordersPropert(int orderid,int custID,int itemID,float qty,int remainingTime,float createdTime,ItemType iType,float deal)
    {

        this.orderid = orderid;
        this.custid = custID;
        this.ItemName = ItemDatabase.GetItem(itemID).name;
        this.itemid = itemID;
        this.ItemImg=Resources.Load<Sprite>("items / " + ItemName);
        this.quantity = qty;
        this.demand = remainingTime;        
        this.orderTime = createdTime;
        this.lastDemandTime = remainingTime * 1440 + createdTime;//siparişin geldiği andan itibaren kendisine ulaşması gereken süre sonu(1.gün geldi (3 gün termin) - 4.gün varış olacak)
        this.itype = iType;
        this.dealPrice = deal;
        this.finished = false;
        //this.exportQuantity = expQty;
    }

   

}
