using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace ami.rss
{
    public class rss
    {
        public GENERICRESPONSE ExecuteRSS(Dictionary<string, string> pRSS)
        {
            GENERICRESPONSE response = new GENERICRESPONSE();

            try
            {
                foreach (var a in pRSS)
                {
                    HttpWebRequest rssFeed = (HttpWebRequest)WebRequest.Create(a.Value);
                    
                    using (DataSet rssData = new DataSet())
                    {
                        rssData.ReadXml(rssFeed.GetResponse().GetResponseStream());
                        
                        foreach (DataRow dataRow in rssData.Tables["item"].Rows)
                        {
                            if (a.Key == "MEX")
                            {
                                if (dataRow[0].ToString().ToUpper() == "DOLAR")
                                {
                                    RSSCURRENCY_TYPE pRSSCURRENCY_TYPE = new RSSCURRENCY_TYPE();
                                    pRSSCURRENCY_TYPE.ADD_CURRENCY(new CURRENCY_TYPE
                                    {
                                        CaptureDate = DateTime.Now,
                                        CurrencyTypeId = 1,
                                        Value = Convert.ToDecimal(dataRow[1])
                                    });

                                    response.SUCCESSFULLY = true;
                                    response.MESSAGE = "Proceso ejecutado correctamente.";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.SUCCESSFULLY = false;
                response.MESSAGE = ex.Message;
            }

            return response;
        }
    }
}

