using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;
using System;

public class customerOrder : MonoBehaviour
{
    public static float t;
    public Transform orderPanel;
    public GameObject orderObj;
    public static List<GameObject> ordersList = new List<GameObject>();
    public  List<GameObject> ordersList2 = new List<GameObject>();
    public static Dictionary<int, GameObject> ordListDict = new Dictionary<int, GameObject>();
    orderDatabase orderDB = new orderDatabase();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > 10f)
        {
            StartCoroutine(SiparisAta(10f));
            t = 0;
        }
    }

    IEnumerator SiparisAta(float time)
    {

        RandomOrder();
        yield return new WaitForSeconds(time);
        /*  int custid = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).custid;
          int itemid = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).itemid;
          float qty = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).quantity;
          int remDay = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).demand;
          string customername = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).customer;
          int orderID = orderDatabase.GetCollectionOrderId(orderDatabase.counter - 1).orderid;
          AddToPanel(custid, itemid, qty, remDay, customername, orderID);*/

       Debug.Log("GELEN SİPARİS SAY:"+orderDatabase.ordersDataCollection.Count);
    }

    public void OrderAccept()
    {
        t = 0;
    }

    public void OrderReject()
    {
        t = 0;
    }

    void AddToPanel(int custid, int itemid, float qty, int remDay, string customername, int orderID,float dealPrice)
    {
        GameObject orderobj = Instantiate(orderObj, orderPanel);//orderObj, orderPanel, false
        ordersObjectProperties oop = orderobj.GetComponent<ordersObjectProperties>();
        string itemname = ItemDatabase.GetItem(itemid).name;
        oop.custname = customername;

        oop.orderid = orderID;
        oop.qty = qty;
        oop.quantity = qty.ToString();
        oop.itemname = itemname;
        oop.icon.sprite = Resources.Load<Sprite>("items/" + itemname);
        oop.message.text = string.Format("{0} ordered {3} pcs {1} in {2} days for {4} cash", customername, itemname.ToUpper(), remDay, qty, (dealPrice * qty).ToString("#.#"));
        oop.remTime = remDay * 1440f; // gerekli zaman dakika bazında (demand(gün)*1440 =dk türünden)
        oop.itemid = ItemDatabase.GetItem(itemname).id;
        oop.sellingPrice = dealPrice*qty;
        oop.IDTEXT.text = orderID.ToString();
        ordersList.Add(orderobj);
        ordersList2.Add(orderobj);
        //ordListDict.Add(orderID, orderobj);

    }

    float RandomTimer(float min, float max)// siparişin oluşacağı rastgele zamanlayıcı olacak.
    {
        return UnityEngine.Random.Range(min, max);
    }

    public void RandomOrder()
    {
        int idx, demand_min, demand_max, demand, itemid = 0, orderid;
        string itemname, custname;
        float orderQty,sellingCostRatio,sellingCost;
        ItemType ItemTip = 0, CustSector;

        idx = UnityEngine.Random.Range(1, customerDatabase.cust.Count + 1);
        sellingCostRatio = UnityEngine.Random.Range(1, 150);
        CustSector = customerDatabase.GetCustomer(idx).sector;
        demand_min = customerDatabase.GetCustomer(idx).OrderDemand["min"];
        demand_max = customerDatabase.GetCustomer(idx).OrderDemand["max"];
        demand = UnityEngine.Random.Range(demand_min, demand_max + 1);
        orderQty = UnityEngine.Random.Range(10, 100);
        custname = customerDatabase.GetCustomer(idx).cust_name;
        while (CustSector != ItemTip) // müsteri sektörüne göre item bulmaya zorla , sektörde tan?ml? item yoksa 0 verecek
        {
            itemid = UnityEngine.Random.Range(1, ItemDatabase.items.Count);
            Debug.Log("OLUSAN SİP ITEM ID:" + itemid);
            ItemTip = ItemDatabase.GetItem(itemid).iType;

        }
        if (itemid == 0)
        {
            Debug.Log("Order and Item not found !");
        }
        else
        {
            itemname = ItemDatabase.GetItem(itemid).name;
            sellingCost=(1+sellingCostRatio/100)* ItemDatabase.GetItem(itemid).cost; // 1-150 arası değerin 100 e bölümü +1 puan ile satış fiyatının belirlenmesi. Örn 50/100+1 = 1.5*cost=satış fiyatı
            Debug.Log(string.Format("KABUL:{0} ordered {1} in {2} days {3} type in {4} sector for {5} $", custname, itemname, demand, ItemTip, CustSector,sellingCost));

            orderid = orderDatabase.counter;//ekrandaki orderid 0 dan başlayacak
            orderDB.InsertOrder(idx, itemid, orderQty, demand, simulation.timer, ItemTip,sellingCost*orderQty); // siparişin oluşturulması
            AddToPanel(idx, itemid, orderQty, demand, custname, orderid,sellingCost);//siparişin ekran yanısması (object)
            
        }

    }
}
