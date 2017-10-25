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

public class Finance : MonoBehaviour
{
    public List<Stock> stocks = new List<Stock>();
    public List<StockYahoo> stocksY = new List<StockYahoo>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.Debug.Log("Start");
        // full path of python interpreter  

        //string bash = @"C:\Windows\System32\bash.exe";
        string python = @"C:\Users\baran\AppData\Local\Programs\Python\Python36-32\python.exe";

        // python app to call  
        string myPythonApp = @"C:\Users\baran\Documents\BlackRockAPI\Assets\sum.py";

        // Create new process start info 
        //ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(bash);
        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

        // make sure we can read the output from stdout 
        myProcessStartInfo.UseShellExecute = false;
        myProcessStartInfo.RedirectStandardOutput = true;

        // start python app with 3 arguments  
        // 1st arguments is pointer to itself,  
        // 2nd and 3rd are actual arguments we want to send 
        //myProcessStartInfo.Arguments = "-c 'python Assets/sum.py'";
        myProcessStartInfo.Arguments = myPythonApp;

        Process myProcess = new Process();
        // assign start information to the process 
        myProcess.StartInfo = myProcessStartInfo;

        // start the process 
        myProcess.Start();

        // Read the standard output of the app we called.  
        // in order to avoid deadlock we will read output first 
        // and then wait for process terminate: 
        StreamReader myStreamReader = myProcess.StandardOutput;
        //string myString = myStreamReader.ReadLine();

        //if you need to read multiple lines, you might use: 
        string myString = myStreamReader.ReadToEnd();

        // wait exit signal from the app we called and then close it. 
        myProcess.WaitForExit();
        myProcess.Close();

        // write the output we got from python app 
        //UnityEngine.Debug.Log("Value received from script: " + myString);

        string[] companies = myString.Split(';');
        UnityEngine.Debug.Log(myString);
        UnityEngine.Debug.Log("there are " + companies.Length + " companies");
        foreach (string company in companies)
        {
            UnityEngine.Debug.Log("a company");
            Stock stock = new Stock();
            string[] lines = company.Split('\n');
            foreach (string line in lines)
            {
                if (line == null || line == "" || line.Contains(";"))
                    continue;
                if (Char.IsLetter(line[0]))
                {
                    stock.name = line;
                    continue;
                }
                if (!line.Contains(","))
                    continue;
                string[] parts = line.Split(',');
                //UnityEngine.Debug.Log(line);
                Int64 time = Int64.Parse(parts[0]);
                DateTime date = FromUnixTime(time);
                stock.dates.Add(date);
                double val = double.Parse(parts[1]);
                stock.vals.Add(val);
            }
            stocks.Add(stock);
        }


        string argumentString = "-c \"from yahoo_finance import Share;";
        string[] comps = new string[] { "AAPL", "GOOGL", "MSFT", "HON", "BLK", "TSLA", "^GSPC", "^IXIC", "GC=F"};
        foreach (string comp in comps)
        {
            argumentString += "x = Share('" + comp + "'); print(x.get_prev_close() + ',' + x.get_price());";
        }

        myProcessStartInfo.Arguments = argumentString;

        myProcess = new Process();
        // assign start information to the process 
        myProcess.StartInfo = myProcessStartInfo;

        // start the process 
        myProcess.Start();

        // Read the standard output of the app we called.  
        // in order to avoid deadlock we will read output first 
        // and then wait for process terminate: 
        myStreamReader = myProcess.StandardOutput;
        //string myString = myStreamReader.ReadLine();

        //if you need to read multiple lines, you might use: 
        myString = myStreamReader.ReadToEnd();

        // wait exit signal from the app we called and then close it. 
        myProcess.WaitForExit();
        myProcess.Close();

        String[] clines = myString.Split('\n');
        for (int i = 0; i < 9; i++)
        {
            String line = clines[i];
            String[] parts = line.Split(',');
            double opening = double.Parse(parts[0]);
            double price = double.Parse(parts[1]);
            double percent = Math.Round(100 * (price - opening) / opening, 2);
            StockYahoo stock = new StockYahoo();
            stock.price = price; stock.percent = percent; stock.name = comps[i];
            stocksY.Add(stock);
        }
    }

    public static DateTime FromUnixTime(Int64 unixTime)
    {
        return epoch.AddMilliseconds(unixTime);
    }
    private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Stock
{
    public String name = "";
    public List<DateTime> dates = new List<DateTime>();
    public List<double> vals = new List<double>();
}

public class StockYahoo
{
    public String name = "";
    public double percent = 0;
    public double price = 0;
}