using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;
using System;

public class stockMenu : MonoBehaviour
{
    public Transform stockPanelContent;
    public GameObject stockObj;
    public List<GameObject> objList = new List<GameObject>();
    public DatabaseReference userRefer;
    public float usingCap;
    public static float warehouseUsingCapacity;
    public Text usingCapText;
    public static List<stockItems> currentStock = new List<stockItems>();
    // Start is called before the first frame update
    void Awake()
    {
        userRefer = auth.userReferance.Child(auth.USER_ID).Child("warehouse");

        CreateStockList();
    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void stockItem(int id, string name, float qty)
    {
        GameObject stkObj = Instantiate(stockObj, stockPanelContent);
        stockPropertys stkProp = stkObj.GetComponent<stockPropertys>();
        stkProp.id = id;
        stkProp.name.text = name;
        stkProp.qty = qty;
        stkProp.quantity.text = qty.ToString("#.#") + " pcs";
        stkProp.icon.sprite = Resources.Load<Sprite>("items/" + name);
        stkProp.volume = ItemDatabase.GetItem(id).stats["volume"];
        stkProp.vol.text = (ItemDatabase.GetItem(id).stats["volume"]*qty).ToString("#.#");
        stkObj.transform.SetParent(stockPanelContent);
        objList.Add(stkObj);

        

    }

    public static stockItems GetStockItemInfo(int id) // stok listesinden itemlere ulaş
    {
        return currentStock.Find(x => x.itemID == id);
    }

    public void CreateStockList()
    {
          if (objList.Count > 0)
          {
              foreach (var x in objList)
              {
                  Destroy(x);
              }
              objList.Clear();
          }

         //userRefer = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("warehouse");
         userRefer.ValueChanged += DepoControl;

       // DataGetir();

    }

 

    private void DepoControl(object sender, ValueChangedEventArgs sss)
    {
        usingCap = 0f;
        float vol = 0f;
        currentStock = new List<stockItems>();

        if (sss.DatabaseError != null)
        {
            Debug.LogError(sss.DatabaseError.Message);
            return;
        }

         var whKeys = sss.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
         int ID;
         float qt;

        foreach (var wh_property in sss.Snapshot.Children) // katman içindeki tip ve değerler
        {

            var values = wh_property.Value as Dictionary<string, object>;
            ID = Convert.ToInt32(values["item_id"]);
            qt = (float)Convert.ToDouble(values["quantity"]);
            stockItem(ID, values["item_name"].ToString(), qt);
            vol = ItemDatabase.GetItem(ID).stats["volume"]*qt;

            currentStock.Add(new stockItems(ID, qt));//stok listesi oluştur direk ulaşım için

            usingCap += vol;
        }

        warehouseUsingCapacity = usingCap;
        usingCapText.text = (100*(usingCap / auth.stockCAP)).ToString("#.#") + "%";
        Debug.Log("usingCap CAP:" + usingCap);


        userRefer.ValueChanged -= DepoControl;
    }

    IEnumerator ReadyStock()
    {
        var getTask = userRefer.GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        Debug.Log("READY STOKC");
    }

    private void DataGetir()
    {
        StartCoroutine(ReadyStock());
        Debug.Log("DATA GETİR");
        userRefer.GetValueAsync().ContinueWith(gettask =>
        {
            if (gettask.IsCompleted)
            {
                DataSnapshot whKeys = gettask.Result; // özel unique ID katmanı
                int ID;
                float qt;

                foreach (var wh_property in whKeys.Children) // katman içindeki tip ve değerler
                {

                    var values = wh_property.Value as Dictionary<string, object>;
                    ID = Convert.ToInt32(values["item_id"]); // itemID
                    qt = (float)Convert.ToDouble(values["quantity"]);//itemQuantity
                    stockItem(ID, values["item_name"].ToString(), qt);


                }
                Debug.Log("DEPO OK");
            }
            else
            {
                Debug.LogError("DEPO HATA");
            }
        });
    }
}
