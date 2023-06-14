using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Bu fonksiyon iş emri üzerinden stok kontrolü sağlar. Üretilecek malzeme depoda varsa üretime gerek yok bilgisi gelir. Üretip
/// üretmeme kullanıcı yetkisindedir. Yok ise zaten stokta olmadığı bilgisi verir.
/// </summary>
public class stockCheck : MonoBehaviour,IPointerClickHandler
{
    int itemid, orderid;
    float stockQty = 0f, orderQty;
    public Transform relationPanel;
    public Button relationBtn;
    private DatabaseReference stockRef;

    void Start()
    {
        stockRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("warehouse");
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponentInParent<JobObjectScript>().turnOff) // İŞ EMRİNİ STOKTA KONTROL ET: eğer İŞ EMRİ BİTMEMİŞ İSE KONTROL YAP , BİTENLER GİZLENECEK
        {
            //auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged += WarehouseControl1; // item ID var/yok kontrol
            stockRef.ValueChanged += WarehouseControl1;

        }

        //relationBtn.onClick.AddListener(delegate { WarehouseControl1( });

    }

    private void WarehouseControl1(object sender, ValueChangedEventArgs vvv)
    {
        var whKeys = vvv.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
        orderid = this.gameObject.GetComponentInParent<JobObjectScript>().orderid;
        itemid = orderDatabase.GetCollectionOrderId(orderid).itemid;
        orderQty = this.gameObject.GetComponentInParent<JobObjectScript>().qty;
        

        foreach (var wh_property in vvv.Snapshot.Children) // katman içindeki tip ve değerler
        {


            if (wh_property.Key.Equals(itemid.ToString()))
            {

                //   this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse = true; // itemi işaretle varsa 

                var values = wh_property.Value as Dictionary<string, object>;

                stockQty = (float)Convert.ToDouble(values["quantity"].ToString()); // AMBAR MİKTARI
                break;
            }
        }

        if (orderQty <= stockQty) // stoktaki ürün siparişe yetecek kadar var ise üretime gerek yok
        {
            Debug.Log("STOK VAR.URETME");
            this.gameObject.GetComponentInParent<JobObjectScript>().isDone = true;
            this.gameObject.GetComponentInParent<JobObjectScript>().turnOff = true;
            UpdateOrderObject(orderid);
            RequirementMaterials(itemid, orderQty);
        }
        else // depoda yok iise üretim gerekli. Üretimin hammaddesini kontrol et
        {
            RequirementMaterials(itemid, orderQty);
        }

        // auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged -= WarehouseControl1; // item ID var/yok kontrol
        stockRef.ValueChanged -= WarehouseControl1;
    }

    /// <summary>
    /// Üretilecek malzememenin hammadde kontrolünü sağlar. Hangi üründen ne kadar gerekli veya direkt ürünün kendisi sipariş olabilr
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="qty"></param>
    public void RequirementMaterials(int itemID,float qty)
    {
        float totalCost=0f, quantity;
        if (!relationPanel.gameObject.activeSelf)
        {
            foreach (KeyValuePair<int, float> components in ItemDatabase.GetItem(itemID).comp) // Material ID ye göre anahtar değerleri alacak component içerisinden
            {
                int component_itemid = components.Key; // bileşen itemID
                float percent = components.Value;//bileşen item oranı

                if (component_itemid != 0) // bileşen_id 0 değilse 1 den fazla bileşeni vardır
                {
                    float cost = ItemDatabase.GetItem(component_itemid).cost * qty * percent; // bileşenler içinden id ye ait cost bul ve miktarla çarp
                    totalCost += cost;
                }
                else // eğer 0 ise malzeme kendisidir . Ana malzemedir
                {
                    float cost = ItemDatabase.GetItem(itemID).cost * qty;
                    totalCost = cost;
                }

                relationPanel.gameObject.SetActive(true);// ilişkisel ürün ağacı gösterimini aç

                if (relationPanel.childCount > 0)// önceden obje var ise temizle
                {
                    for(int i=0;i< relationPanel.childCount; i++)
                    {
                        Destroy(relationPanel.GetChild(i).gameObject);
                    }

                }
                else
                {
                    Button obj = Instantiate(relationBtn, relationPanel);//buton görünümden obje oluştur
                    obj.GetComponent<relationMaterialInfo>().id = component_itemid;// ilişkili item id
                    obj.GetComponent<relationMaterialInfo>().percent.text = (100 * percent).ToString() + "%";// item kullanım yüzdesi
                    obj.GetComponent<relationMaterialInfo>().itemname = ItemDatabase.GetItem(component_itemid).name;// ilişkili item adı
                    obj.GetComponent<relationMaterialInfo>().item_icon.sprite = Resources.Load<Sprite>("items/" + ItemDatabase.GetItem(component_itemid).name);//ilişkili ürünün resmi

                }


                Debug.LogWarning("BileşenlerID=" + component_itemid + "/" + percent);
            }

        }
    }

    public void UpdateOrderObject(int orderid) // ORDER_OBJECT ögesininin bilgilerini günceller
    {

        for (int i = 0; i < customerOrder.ordersList.Count; i++)
        {
            Debug.Log("ORDER SAYISI:" + customerOrder.ordersList.Count+"/"+orderid);
            if (orderid == customerOrder.ordersList[i].GetComponent<ordersObjectProperties>().orderid)
            {
                customerOrder.ordersList[i].GetComponent<ordersObjectProperties>().isCompleted = true;
                return;
            }

        }

    }
}
