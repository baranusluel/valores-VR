#!/usr/bin/python 
#my simple python summation app,  
#gets 2 integers and returns the sum 
 
import sys 
import json
import requests

response = requests.get("https://www.blackrock.com/tools/hackathon/performance", params= {'identifiers':"AAPL,GOOGL,MSFT,HON,BSPIX,IYY"}).json();
for n in range(0, 6):
	data = response["resultMap"]["RETURNS"][n]["performanceChart"]
	name = response["resultMap"]["RETURNS"][n]["uniqueId"]
	print(name);
	for i in range(0, len(data)):
	   print(str(data[i][0]) + "," + str(data[i][1]));
	if n < 5:
		print(";");