using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data;
using System.Diagnostics;

namespace OokNET
{
    public class ExcelExport
    {
        //OpenXML kullanarak DataSeti Excel (.xlsx) dosyasına çevirip indirmek
        // REFERANSLAR
        // ***************************************
        // DocumentFormat.OpenXml
        // WindowsBase
        //Bu iki referans projeye bir kez eklenir
        // ***************************************
        public static bool ExcelOlusturIndir(DataSet ds, string dosyaadi, System.Web.HttpResponse Response)
        {
            try
            {
                //"HttpCacheability does not exist" hatası alırsanız projeye System.Web referansını ekleyin
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                using (SpreadsheetDocument dosya = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {
                    DataSetExceleYaz(ds, dosya);
                }
                stream.Flush();
                stream.Position = 0;

                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";

                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment; filename=" + dosyaadi);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                byte[] data1 = new byte[stream.Length];
                stream.Read(data1, 0, data1.Length);
                stream.Close();
                Response.BinaryWrite(data1);
                Response.Flush();
                Response.End();

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Hata oluştu: " + ex.Message);
                return false;
            }
        }


        private static void DataSetExceleYaz(DataSet ds, SpreadsheetDocument calismaSayfasi)
        {
            // Excel dosyasının içeriği oluşturuluyor. 
            calismaSayfasi.AddWorkbookPart();
            calismaSayfasi.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
            calismaSayfasi.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            // "WorkbookStylesPart", OLEDB bağlantı hatasını önlüyor
            WorkbookStylesPart kitapStil = calismaSayfasi.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            Stylesheet stilSayfasi = new Stylesheet();
            kitapStil.Stylesheet = stilSayfasi;

            //  DataSet içindeki tablolara ulaşıp her biri için ayrı çalışma sayfası oluşturulur.
            uint calismaSayfaNo = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  Çalışma sayfaları özelleştirilebilir
                string calismaSayfaID = "rId" + calismaSayfaNo.ToString();
                string calismaSayfaIsim = dt.TableName;

                WorksheetPart yeniCalismaSayfasiBolumu = calismaSayfasi.WorkbookPart.AddNewPart<WorksheetPart>();
                yeniCalismaSayfasiBolumu.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

                // sayfa verileri oluştur
                yeniCalismaSayfasiBolumu.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

                // Çalışma Sayfasını Kaydet
                DataTableExceleYaz(dt, yeniCalismaSayfasiBolumu);
                yeniCalismaSayfasiBolumu.Worksheet.Save();

                // create the worksheet to workbook relation
                if (calismaSayfaNo == 1)
                    calismaSayfasi.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                calismaSayfasi.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = calismaSayfasi.WorkbookPart.GetIdOfPart(yeniCalismaSayfasiBolumu),
                    SheetId = (uint)calismaSayfaNo,
                    Name = dt.TableName
                });

                calismaSayfaNo++;
            }

            calismaSayfasi.WorkbookPart.Workbook.Save();
        }


        private static void DataTableExceleYaz(DataTable dt, WorksheetPart calismaSayfasiParca)
        {
            var calismaSayfasi = calismaSayfasiParca.Worksheet;
            var calismaVerisi = calismaSayfasi.GetFirstChild<SheetData>();

            string hucreVerisi = "";

            // Tablodan gelen her başlık excelde sütunlara başlık olarak atanıyor
            // Ayrıca, her veri sütununun (Metin veya Sayısal) ne tür olduğunu gösteren bir dizi oluşturacağız, bu nedenle gerçek veri hücrelerini yazmaya başladığımızda, Metin değerleri mi yoksa Sayısal hücre değerleri mi yazacağımızı bileceğiz.

            int sutunSayisi = dt.Columns.Count;
            bool[] sayisalKolonMu = new bool[sutunSayisi];

            string[] excelKolonIsimleri = new string[sutunSayisi];
            for (int n = 0; n < sutunSayisi; n++)
                excelKolonIsimleri[n] = ExcelKolonIsmiVer(n);

            //  Çalışma sayfasına başlık satırı oluşturuluyor
            uint satirIndex = 1;

            var baslikSatiri = new Row { RowIndex = satirIndex };  // satır çalışma sayfasının en başına ekleniyor
            calismaVerisi.Append(baslikSatiri);

            for (int kolonIndex = 0; kolonIndex < sutunSayisi; kolonIndex++)
            {
                DataColumn kolon = dt.Columns[kolonIndex];
                metinHucresiEkle(excelKolonIsimleri[kolonIndex] + "1", kolon.ColumnName, baslikSatiri);
                sayisalKolonMu[kolonIndex] = (kolon.DataType.FullName == "System.Decimal") || (kolon.DataType.FullName == "System.Int32");
            }

            //  Tablodaki her bir satıra gidiyoruz
            double hucreSayisalDeger = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // yeni satır oluşturup tablodan gelen satırları buraya ekliyoruz..
                ++satirIndex;
                var yeniExcelSatir = new Row { RowIndex = satirIndex };  // Çalışma sayfasının en üstüne satır ekle
                calismaVerisi.Append(yeniExcelSatir);

                for (int kolonIndex = 0; kolonIndex < sutunSayisi; kolonIndex++)
                {
                    hucreVerisi = dr.ItemArray[kolonIndex].ToString();

                    // Veriden hücre oluştur
                    if (sayisalKolonMu[kolonIndex])
                    {
                        //  Sayısal hücreler için, girdi verilerimizin bir sayı olduğundan emin olun, ardından bunu Excel dosyasına yazın.
                        //  Bu sayısal değer NULL ise, Excel dosyasına hiçbir şey yazmayın.
                        hucreSayisalDeger = 0;
                        if (double.TryParse(hucreVerisi, out hucreSayisalDeger))
                        {
                            hucreVerisi = hucreSayisalDeger.ToString();
                            sayisalHucresiEkle(excelKolonIsimleri[kolonIndex] + satirIndex.ToString(), hucreVerisi, yeniExcelSatir);
                        }
                    }
                    else
                    {
                        //  Metinsel hücre değerleri için, giriş verilerini doğrudan Excel dosyasına yazar
                        metinHucresiEkle(excelKolonIsimleri[kolonIndex] + satirIndex.ToString(), hucreVerisi, yeniExcelSatir);
                    }
                }
            }
        }


        private static void metinHucresiEkle(string hucreReferans, string hucreMetinDegeri, Row excelSatir)
        {
            //  Satıra yeni bir metinsel hücre ekle
            Cell hucre = new Cell() { CellReference = hucreReferans, DataType = CellValues.String };
            CellValue hucreDegeri = new CellValue();
            hucreDegeri.Text = hucreMetinDegeri;
            hucre.Append(hucreDegeri);
            excelSatir.Append(hucre);
        }

        private static void sayisalHucresiEkle(string hucreReferans, string hucreSayiDegeri, Row excelSatir)
        {
            // Satır içerisine yeni bir sayısal hücre ekle 
            Cell hucre = new Cell() { CellReference = hucreReferans };
            CellValue hucreDegeri = new CellValue();
            hucreDegeri.Text = hucreSayiDegeri;
            hucre.Append(hucreDegeri);
            excelSatir.Append(hucre);
        }

        private static string ExcelKolonIsmiVer(int sutunIndex)
        {
            //Excel kolon isimlerini döner
            if (sutunIndex < 26)
                return ((char)('A' + sutunIndex)).ToString();

            char ilkKarakter = (char)('A' + (sutunIndex / 26) - 1);
            char ikinciKarakter = (char)('A' + (sutunIndex % 26));

            return string.Format("{0}{1}", ilkKarakter, ikinciKarakter);
        }


    }
}