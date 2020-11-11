//parse && tryparse
void Main()
{
	string str1 = "1000";
	string str2 = null;
	string str3 = "1000.99999";
	string str4 = "9999999999999999999999999999999999";
	string str5 = "blabla";
	
	int sonuc;
	
	try
	{
		//sonuc = int.Parse(str1); 		//1000
		//sonuc = int.Parse(str2); 		//ArgumentNullException
		//sonuc = int.Parse(str3);   	//FormatException
		//sonuc = int.Parse(str4);		//OverflowException
		sonuc = int.Parse(str5);		//FormatException
		Console.WriteLine(sonuc);
	}
	catch (Exception ex)
	{
		Console.WriteLine("Hata: " + ex.GetType());
	}
	
	
	//TryParse
	bool parseDurum;
	
	parseDurum = int.TryParse(str1, out sonuc); //true
	parseDurum = int.TryParse(str2, out sonuc); //false
	parseDurum = int.TryParse(str3, out sonuc);	//false
	parseDurum = int.TryParse(str4, out sonuc);	//false
	parseDurum = int.TryParse(str5, out sonuc);	//false
	Console.WriteLine(parseDurum);
	
	//kullanım
	int deger = -1;
	if(int.TryParse(str2, out sonuc))
	{
		deger = sonuc;
	}
	
	Console.WriteLine(deger);
	
	
	string str6 = "5.6666";
	
	
	NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
	{
		NumberDecimalSeparator =".",
	};


	double deger2 = double.Parse(str6,numberFormatInfo);
	Console.WriteLine(deger2);

}
