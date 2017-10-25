using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stockpopulator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject thePlayer = GameObject.Find("FinanceObject");
        Finance playerScript = thePlayer.GetComponent<Finance>();
        List<StockYahoo> stocks = playerScript.stocksY;
        Text text = GetComponent<Text>();
        int i = int.Parse(text.text);
        if (i <= 5)
        {
            text.text = "" + stocks[i].price;
            if (stocks[i].percent > 0)
            {
                text.color = new Color(0f, 1f, 0f);
            } else if (stocks[i].percent < 0){
                text.color = new Color(1f, 0f, 0f);
            }
        }
        else
        {
            text.text = "" + stocks[i].percent + "%";
            if (stocks[i].percent > 0)
            {
                text.color = new Color(0f, 1f, 0f);
            }
            else if (stocks[i].percent < 0)
            {
                text.color = new Color(1f, 0f, 0f);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
