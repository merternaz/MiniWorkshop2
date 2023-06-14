using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slidingpage : MonoBehaviour
{
    public Transform mainPanel, materialPanel, machinesPanel, stockPanel, orderPanel, jobsPanel, ImportExportPanel,reportPanel,personelPanel;
    float slide_speed = 1000f, t;
    Vector2 mainLoc, matPanelLoc, stockPanelLoc, orderPanelLoc, macPanelLoc, jobPanelLoc,ImpExpPanelLoc,reportPanelLoc,personelPanelLoc;
    public static bool callMaterial = false, sendMaterial = false, callOrder = false, closeOrder = false,
        callStock = false, sendStock = false, callMachine = false, closeMachine = false,
        callJobs = false, closeJobs = false, callImportExport = false, closeImportExport = false,
        callReportPanel=false,sendReportPanel=false, callPersonelPanel = false, sendPersonelPanel = false;
    // Start is called before the first frame update
    void Start()
    {
        mainLoc = mainPanel.transform.localPosition;
        matPanelLoc = materialPanel.transform.position;// 960,-1080
        stockPanelLoc = stockPanel.transform.position;// 960,-1080
        orderPanelLoc = orderPanel.transform.position;
        macPanelLoc = machinesPanel.transform.position;
        jobPanelLoc = jobsPanel.transform.position;
        ImpExpPanelLoc = ImportExportPanel.transform.position;
        reportPanelLoc = reportPanel.transform.position;
        personelPanelLoc = personelPanel.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (callMaterial)
        {
            t += Time.deltaTime;
            float y = matPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                materialPanel.transform.position = new Vector2(matPanelLoc.x, y);
            }
            else
            {
                callMaterial = false;
                materialPanel.transform.position = new Vector2(matPanelLoc.x, 0);

                t = 0;
            }
        }

        if (sendMaterial)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > matPanelLoc.y)
            {

                materialPanel.transform.position = new Vector2(matPanelLoc.x, y);
            }
            else
            {

                materialPanel.transform.position = new Vector2(matPanelLoc.x, matPanelLoc.y);
                sendMaterial = false;
                t = 0;
            }
        }

        if (callStock)
        {
            t += Time.deltaTime;
            float y = stockPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                stockPanel.transform.position = new Vector2(stockPanelLoc.x, y);
            }
            else
            {
                callStock = false;
                stockPanel.transform.position = new Vector2(stockPanelLoc.x, 0);

                t = 0;
            }
        }

        if (sendStock)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > stockPanelLoc.y)
            {

                stockPanel.transform.position = new Vector2(stockPanelLoc.x, y);
            }
            else
            {

                stockPanel.transform.position = new Vector2(stockPanelLoc.x, stockPanelLoc.y);
                sendStock = false;
                t = 0;
            }
        }

        if (callOrder)
        {
            t += Time.deltaTime;
            float y = orderPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                orderPanel.transform.position = new Vector2(orderPanelLoc.x, y);
            }
            else
            {
                callOrder = false;
                orderPanel.transform.position = new Vector2(orderPanelLoc.x, 0);

                t = 0;
            }
        }

        if (closeOrder)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > orderPanelLoc.y)
            {

                orderPanel.transform.position = new Vector2(orderPanelLoc.x, y);
            }
            else
            {

                orderPanel.transform.position = new Vector2(orderPanelLoc.x, orderPanelLoc.y);
                closeOrder = false;
                t = 0;
            }
        }

        if (callMachine)
        {
            t += Time.deltaTime;
            float y = macPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                machinesPanel.transform.position = new Vector2(macPanelLoc.x, y);
            }
            else
            {
                callMachine = false;
                machinesPanel.transform.position = new Vector2(macPanelLoc.x, 0);

                t = 0;
            }
        }

        if (closeMachine)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > macPanelLoc.y)
            {

                machinesPanel.transform.position = new Vector2(macPanelLoc.x, y);
            }
            else
            {

                machinesPanel.transform.position = new Vector2(macPanelLoc.x, macPanelLoc.y);
                closeMachine = false;
                t = 0;
            }
        }

        if (callJobs)
        {
            t += Time.deltaTime;
            float y = jobPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                jobsPanel.transform.position = new Vector2(jobPanelLoc.x, y);
            }
            else
            {
                callJobs = false;
                jobsPanel.transform.position = new Vector2(jobPanelLoc.x, 0);

                t = 0;
            }
        }

        if (closeJobs)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > jobPanelLoc.y)
            {

                jobsPanel.transform.position = new Vector2(jobPanelLoc.x, y);
            }
            else
            {

                jobsPanel.transform.position = new Vector2(jobPanelLoc.x, jobPanelLoc.y);
                closeJobs = false;
                t = 0;
            }
        }

        if (callImportExport)
        {
            t += Time.deltaTime;
            float y = ImpExpPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                ImportExportPanel.transform.position = new Vector2(ImpExpPanelLoc.x, y);
            }
            else
            {
                callImportExport = false;
                ImportExportPanel.transform.position = new Vector2(ImpExpPanelLoc.x, 0);

                t = 0;
            }
        }

        if (closeImportExport)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > ImpExpPanelLoc.y)
            {

                ImportExportPanel.transform.position = new Vector2(ImpExpPanelLoc.x, y);
            }
            else
            {

                ImportExportPanel.transform.position = new Vector2(ImpExpPanelLoc.x, ImpExpPanelLoc.y);
                closeImportExport = false;
                t = 0;
            }
        }

        if (callReportPanel)
        {
            t += Time.deltaTime;
            float y = reportPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                reportPanel.transform.position = new Vector2(reportPanelLoc.x, y);
            }
            else
            {
                callReportPanel = false;
                reportPanel.transform.position = new Vector2(reportPanelLoc.x, 0);

                t = 0;
            }
        }

        if (sendReportPanel)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > reportPanelLoc.y)
            {

                reportPanel.transform.position = new Vector2(reportPanelLoc.x, y);
            }
            else
            {

                reportPanel.transform.position = new Vector2(reportPanelLoc.x, reportPanelLoc.y);
                sendReportPanel = false;
                t = 0;
            }
        }

        if (callPersonelPanel)
        {
            t += Time.deltaTime;
            float y = personelPanelLoc.y + (t * slide_speed);

            if (y < 0)
            {

                personelPanel.transform.position = new Vector2(personelPanelLoc.x, y);
            }
            else
            {
                callPersonelPanel = false;
                personelPanel.transform.position = new Vector2(personelPanelLoc.x, 0);

                t = 0;
            }
        }

        if (sendPersonelPanel)
        {
            t += Time.deltaTime;
            float y = 0 - (t * slide_speed);
            if (y > personelPanelLoc.y)
            {

                personelPanel.transform.position = new Vector2(personelPanelLoc.x, y);
            }
            else
            {

                personelPanel.transform.position = new Vector2(personelPanelLoc.x, personelPanelLoc.y);
                sendPersonelPanel = false;
                t = 0;
            }
        }
    }

    public void OpenMatPanel()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callMaterial = true;
        sendMaterial = false;

    }

    public void CloseMatPanel()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callMaterial = false;
        sendMaterial = true;

    }

    public void OpenStock()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callStock = true;
        sendStock = false;

    }
    public void CloseStock()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callStock = false;
        sendStock = true;

    }

    public void OpenOrder()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callOrder = true;
        closeOrder = false;

    }
    public void CloseOrder()
    {
        // matPanelLoc = materialPanel.transform.position;// 960,-1080

        callOrder = false;
        closeOrder = true;

    }

    public void OpenMachine()
    {
        callMachine = true;
        closeMachine = false;
    }
    public void CloseMachine()
    {
        callMachine = false;
        closeMachine = true;
    }
    public void OpenJobs()
    {
        callJobs = true;
        closeJobs = false;
    }
    public void CloseJobs()
    {
        callJobs = false;
        closeJobs = true;
    }

    public void OpenImpExp()
    {
        callImportExport = true;
        closeImportExport = false;
    }
    public void CloseImpExp()
    {
        callImportExport = false;
        closeImportExport = true;
    }

    public void OpenReport()
    {
        callReportPanel = true;
        sendReportPanel = false;
    }
    public void CloseReport()
    {
        callReportPanel = false;
        sendReportPanel = true;
    }

    public void OpenPersonel()
    {
        callPersonelPanel = true;
        sendPersonelPanel = false;
    }
    public void ClosePersonel()
    {
        callPersonelPanel = false;
        sendPersonelPanel = true;
    }



}

