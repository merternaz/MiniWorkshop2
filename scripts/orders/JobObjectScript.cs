using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Her İŞ EMRİ OBJESİNDE bulunan bilgileri içeriir
/// </summary>
public class JobObjectScript : MonoBehaviour
{
    public Text jID, orderName, Qty, prdQTY, orderID;
    public bool isDone = false,turnOff=false;
    public Image ordImg;
    public Image productBar;
    public Image statusImg;
    public int jid;
    public int orderid;
    public string jname;
    public float qty, prdqty;
    public Transform macListPanel;
    public Button selectableMachine;
    private float t;

    private int j_id, j_orderid;
    private void Update()
    {
        //t += Time.deltaTime;
        // productBar.fillAmount=
        prdQTY.text = prdqty.ToString("#");

        if (isDone)
        {
            statusImg.sprite = Resources.Load<Sprite>("backgrounds/OKicon");
        }
        else
        {
            statusImg.sprite = Resources.Load<Sprite>("backgrounds/crossIcon");
        }
    }



    public void RunJob()//Çalıştırmak için makinalar listesini gösterir(UYGUN MAKİNA)
    {

        j_id = this.gameObject.GetComponentInParent<JobObjectScript>().jid;
        j_orderid = this.gameObject.GetComponentInParent<JobObjectScript>().orderid;
        int itemID=orderDatabase.GetCollectionOrderId(j_orderid).itemid;
        ItemType itype = ItemDatabase.GetItem(itemID).iType;

       
            if (!macListPanel.gameObject.activeSelf) // açksa kapat/kapalysa aç
            {
                macListPanel.gameObject.SetActive(true);
            }
            else
            {
                macListPanel.gameObject.SetActive(false);


            }
        
       


        /* if (macListPanel.childCount > 0)
         {
             for (int i = 0; i < macListPanel.childCount; i++)
             {
                 Destroy(macListPanel.gameObject.GetComponentInChildren<Button>().transform.GetChild(i));
             }

         }*/

        if (macListPanel.childCount == 0) // içinde obje yok ise 
        {
            for (int i = 0; i < simulation.machinesUsebleIDs.Count; i++) // kullanılabilir makina listesindeki sayı kadar
            {
                if (machineDatabase.GetMachine(simulation.machinesUsebleIDs[i]).machineType == (int)itype) // item ile makina tipi uyumlu ise seçme şansı verir
                {
                    Button selectMachine = Instantiate(selectableMachine, macListPanel); // buton oluştur
                    selectMachine.GetComponentInChildren<Text>().text = machineDatabase.GetMachine(simulation.machinesUsebleIDs[i]).machine_name;
                    selectMachine.transform.GetChild(1).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("machines/mac_icons/" + machineDatabase.GetMachine(simulation.machinesUsebleIDs[i]).machine_name);
                    i++; // buton içinde text ogesinin verisini kullanılabilir makinalar listesinden al
                }
                
            }
        }



    }

    /// <summary>
    /// hangi makinanın butonuna basıldıysa , o makinayı sahnede bulup ANIMATORUNU başlatmalı ve üretim simulasyonu başlamalı
    /// hangi iş no ve sip no çalışıyorsa o kayıtları güncellemeli
    /// </summary>
   /* public void AttachToMachine()
    {
        string machineName;
        machineName=this.GetComponentInChildren<Text>().text;
        GameObject.FindGameObjectWithTag(machineName).GetComponent<machineInfo>().GetAnimatorStart();
    }*/
}
