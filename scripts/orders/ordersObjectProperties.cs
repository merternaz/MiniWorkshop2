using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class ordersObjectProperties : MonoBehaviour
{
    public Image icon, statusImg;
    JobDatabase jobDB = new JobDatabase();
    public string custname, itemname, quantity;
    public int orderid,itemid;
    public float qty, remTime,sellingPrice;
    public Text dealTotalCost,IDTEXT;
    public Text message;
    public Image timerBar;
    public bool delayed = false;
    private float currentTime, timer;
    public ItemType itype;
    public Button acceptButton, rejectButton,sendButton;
    public GameObject jobObj,exportObj;
    private Transform exportPanelContent;
    private bool isAccepted = false;
    public bool isCompleted = false;
    private bool isSent = false;
    private int sendObjCounter = 0;
    private void Start()
    {
        currentTime = simulation.timer;
        exportPanelContent = GameObject.FindGameObjectWithTag("ExportScreen").GetComponent<Transform>();
    }
     void Update()
    {
        

        if (!isCompleted)
        {
            timer += Time.deltaTime; // geçen zaman

            if (timer > remTime + currentTime) // zaman , termin+mevcut zamandan büyükse GECİKMİS sipariş
            {
                timerBar.fillAmount = 0;
                delayed = true;
            }
            else
            {
                timerBar.fillAmount = ((remTime + currentTime) - timer) / (remTime + currentTime);
            }
        }
        

        
        if (isAccepted)
        {
            //Sipariş kabul edildiğinde SARI RENK görünür
            string stat1 = "status1";
            this.gameObject.GetComponentInParent<ordersObjectProperties>().statusImg.sprite = Resources.Load<Sprite>("scripts/customers/custIcon/status1");

            if (isCompleted)
            {
                this.gameObject.GetComponentInParent<ordersObjectProperties>().statusImg.sprite = Resources.Load<Sprite>("scripts/customers/custIcon/status2");
                
                if (isSent)
                {
                    sendButton.gameObject.SetActive(false);
                }
                else
                {
                    sendButton.gameObject.SetActive(true);
                }
            }


        }
    }

    public void RejectOrder()
    {
        int orderid = this.gameObject.GetComponentInParent<ordersObjectProperties>().orderid;
        int index = 0;

        for(int i = 0; i < customerOrder.ordersList.Count; i++)// otomatik eklenen orderList içinden iptal edilen OrderId yi listeden kaldıracak indexi bul
        {
            if(orderid== customerOrder.ordersList[i].GetComponent<ordersObjectProperties>().orderid)
            {

                index = i;
            }
        }


        Destroy(this.transform.gameObject);//ekrandaki listeden sil
        customerOrder.ordersList.RemoveAt(index);//orderList içerisinen sil(ekrandaki sayaç kontrolü)
        orderDatabase.DeleteOrderID(orderid);//siparişlerin Database içerisinden sildi

    }

    public void AcceptOrder()
    {
        int orderid = this.gameObject.GetComponentInParent<ordersObjectProperties>().orderid;

        ItemType tip = ItemDatabase.GetItem(itemid).iType;
        float or_qty = this.gameObject.GetComponentInParent<ordersObjectProperties>().qty;

   
       
        jobDB.AddJob(orderid, tip, or_qty, 0f,jobObj);

        Destroy(acceptButton.gameObject);
        Destroy(rejectButton.gameObject);
        

        isAccepted = true;
    }

    /// <summary>
    /// Sipariş tamamlanmış. Sipariş miktarı kadar depodan sevk et. Anlaşma fiyatını kasaya ekle (kazanç)
    /// </summary>
    public void SendOrder()
    {
        database db = new database();
        int orderid = this.gameObject.GetComponentInParent<ordersObjectProperties>().orderid;
        int itemid = this.gameObject.GetComponentInParent<ordersObjectProperties>().itemid;
        float earnings = this.gameObject.GetComponentInParent<ordersObjectProperties>().sellingPrice;
        float or_qty = this.gameObject.GetComponentInParent<ordersObjectProperties>().qty;
        int sendListID = sendObjCounter;
        // TEST İÇİN PASİFTE---- AÇILMALI-----------
      /*  float currentCash = infobar.cash + earnings; // kazancı kasaya ekle
        float currentStockQty = (db.GetItemQuantity(auth.USER_ID, itemid)) - or_qty; // mevcut stoktan sevk olacak miktarı çıkar

        StartCoroutine(db.UpdateBusinessCash(auth.USER_ID, currentCash));//kasayı güncelle
        StartCoroutine(db.UpdateItemQuantity(auth.USER_ID, itemid, currentStockQty));// depoyu güncelle*/

        AddExportList(sendListID,orderid, itemid, or_qty, earnings, remTime);
        //Destroy(sendButton.gameObject);
        isSent = true;
        //sendButton.gameObject.SetActive(false);
        
        sendObjCounter++;
    }

    /// <summary>
    /// Sevk edilebilir Obje oluşumu (Export Panel üzerinde)
    /// </summary>
    /// <param name="sendID"></param>
    /// <param name="orderID"></param>
    /// <param name="itemID"></param>
    /// <param name="qty"></param>
    /// <param name="totEarnings"></param>
    /// <param name="remainingTime"></param>
    void AddExportList(int sendID,int orderID,int itemID,float qty,float totEarnings,float remainingTime) //EXPORT PANEL de OBJE OLUŞTURACAK FONKS
    {
        GameObject _exportObj = Instantiate(exportObj, exportPanelContent);
        ExportObjectProperty eop = _exportObj.GetComponent<ExportObjectProperty>();
        eop.orderId = orderID;
        eop.itemId = itemID;
        eop.OrderQty = qty;
        eop.unitPerEarnings = totEarnings / qty;
        eop.demandTime = remainingTime;
        eop.itemImg.sprite= Resources.Load<Sprite>("items/" + ItemDatabase.GetItem(itemID).name);
        eop.sendListID = sendID;//export panelindeki ID
        ExportingOrderDB.ExportableObjectList.Add(_exportObj);
    }

    public IEnumerator UpdateCash(string userid, float money) // Güncel 
    {

        DatabaseReference waultReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = waultReferance.Child(userid).Child("cash").SetValueAsync(money);
        //item_id = Convert.ToInt32(DBTask.get);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Wallet Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Wallet Güncellendi");
        }
    }
}
