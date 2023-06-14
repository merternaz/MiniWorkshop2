using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobDatabase : MonoBehaviour
{
    public static List<JobScript> jobList = new List<JobScript>();
    public static List<JobScript> jobCollection = new List<JobScript>();
    public static int jobid;
    public Transform jobPanel;
    public static List<GameObject> jobObjectList = new List<GameObject>();
    //public GameObject jobObject; 

    public void AddJob(int orderid, ItemType type, float ordqty, float prdqty, GameObject ob)
    {
        /* jobList = new List<JobScript>()        
         {
             new JobScript(jobid,orderid,type,ordqty),

         };*/

        jobCollection.Add(new JobScript(jobid, orderid, type, ordqty));

        Transform jobPanel = GameObject.FindGameObjectWithTag("jobcontent").transform;
        GameObject obj = Instantiate(ob, jobPanel);
        JobObjectScript jos = obj.GetComponent<JobObjectScript>();
        jos.jID.text = jobid.ToString();
        jos.jid = jobid;
        jos.orderid = orderid;
        jos.orderID.text = orderid.ToString();
        jos.jname = orderDatabase.GetCollectionOrderId(orderid).ItemName;
        jos.orderName.text = orderDatabase.GetCollectionOrderId(orderid).ItemName;
        jos.Qty.text = ordqty.ToString();
        jos.qty = ordqty;
        jos.isDone = false;
        jos.ordImg.sprite = Resources.Load<Sprite>("items/" + jos.jname);
        jobObjectList.Add(obj);

        jobid++;
    }

   

    public static JobScript GetJob(int job_id)
    {
        return jobCollection.Find(x => x.jobID == job_id);
    }

    public void RefreshJobList(GameObject ob)
    {
        Transform jobPanel = GameObject.FindGameObjectWithTag("jobcontent").transform;
        GameObject gObj = Resources.Load("script/orders/JobObject") as GameObject;

        Debug.Log("TRANSFORM ADI=" + ob.GetComponent<JobObjectScript>().jname);
        //Debug.Log("GO ADI=" + jobObject.name);


        for (int i = 0; i < jobCollection.Count; i++)  // job objesi içindeki özellikleri , oluşan jobscript listesinden güncel olarak çekecek
        {
            GameObject obj = Instantiate(gObj, jobPanel);
            JobObjectScript jos = obj.GetComponent<JobObjectScript>();
            jos.jID.text = jobCollection[i].jobID.ToString();
            jos.orderName.text = orderDatabase.GetCollectionOrderId(jobCollection[i].orderID).ItemName;
            jos.Qty.text = jobCollection[i].orderQty.ToString();
            jos.isDone = jobCollection[i].isDone;
            jos.ordImg.sprite = orderDatabase.GetCollectionOrderId(jobCollection[i].orderID).ItemImg;

            Debug.Log("İŞ ID=" + jobList[i].orderID);
        }

    }

}
