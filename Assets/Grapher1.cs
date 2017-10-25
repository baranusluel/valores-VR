using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using System.Diagnostics;

public class Grapher1 : MonoBehaviour {

    public ParticleSystem.Particle[] pointsAll;
    public int stockid = 0;

    void Start()
    {
        stockid = 0;
        GameObject thePlayer = GameObject.Find("FinanceObject");
        Finance playerScript = thePlayer.GetComponent<Finance>();
        List<Stock> stocks = playerScript.stocks;
        /*for (int i = 0; i < 5; i++)
        {
            Stock stock1 = stocks[i];
            UnityEngine.Debug.Log(stock1.vals[stock1.vals.Count - 1]);
        }*/

        //UnityEngine.Debug.Log("there are " + stocks.Count + " stocks");
        UnityEngine.Debug.Log("id is " + stockid);
        Stock stock = stocks[stockid];
        UnityEngine.Debug.Log("graphing " + stock.name);
        UnityEngine.Debug.Log(stock.dates[0].ToShortDateString());
        List<double> vals = stock.vals;
        UnityEngine.Debug.Log("ending price: " + stock.vals[stock.vals.Count - 1]);
        UnityEngine.Debug.Log("ending date: " + stock.dates[stock.vals.Count - 1]);
        UnityEngine.Debug.Log("before ending date: " + stock.dates[stock.vals.Count - 2]);
        pointsAll = new ParticleSystem.Particle[vals.Count];
        float increment = 75f / vals.Count;
        for (int i = 0; i < vals.Count; i++)
        {
            float x = i * increment - 40;
            pointsAll[i].position = new Vector3(x, (float)vals[i] / 1.9f - 30, -10);
            //pointsAll[i].position = new Vector3(x, 0, 0f);
            pointsAll[i].color = new Color(1f, 0f, 0f);
            pointsAll[i].size = 1f;
        }
    }

    void Update()
    {
        //foreach (ParticleSystem.Particle[] points in pointsAll)
        //{
            GetComponent<ParticleSystem>().SetParticles(pointsAll, pointsAll.Length);
        //}
    }
}
