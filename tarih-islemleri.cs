using System.Globalization

void Main()
{
	DateTime datetime = DateTime.Now;//new DateTime(2020,12,30);
	//Console.WriteLine(datetime);
	int year = datetime.Year;
	//Console.WriteLine(year);
	DateTime dtBaslangicGun = baslangicGunuBul(datetime, DayOfWeek.Monday);
	Console.WriteLine(dtBaslangicGun.ToString("dd.MM.yyyy"));
	DateTime dtBitisGunu = dtBaslangicGun.AddDays(6);
	Console.WriteLine(dtBitisGunu.ToString("dd.MM.yyyy"));
	
	var kultur = CultureInfo.CurrentCulture;
	
	int haftaNo = kultur.Calendar.GetWeekOfYear(datetime,
												kultur.DateTimeFormat.CalendarWeekRule,
												kultur.DateTimeFormat.FirstDayOfWeek);
	Console.WriteLine($"Hafta No: {haftaNo}");
	
	//Ay numarası
	int ayNo = kultur.Calendar.GetMonth(datetime);
	Console.WriteLine($"Ay No: {ayNo}");
	
	//Yılın kaçıncı günü
	int gunNo = kultur.Calendar.GetDayOfYear(datetime);
	Console.WriteLine($"Gün No: {gunNo}");
	
}
public DateTime baslangicGunuBul(DateTime dt, DayOfWeek haftaninIlkGunu)
{
	int fark = dt.DayOfWeek - haftaninIlkGunu;
	return dt.AddDays(-1 * fark).Date;
}

