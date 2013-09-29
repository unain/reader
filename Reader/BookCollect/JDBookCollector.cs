using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookCollect
{
    class JDBookCollector : BookCollector
    {
        public JDBookCollector() : base()
        {
            BaseUri = "http://www.jd.com/booktop-1-2-0-1.html";
            XpathBookUri = "//dt[@class='p-name']/a";
            XpathISBN = "//li[@id='summary-isbn']/div[@class='dd']";
            PageEncoding = "gb2312";
        }

        
    }
}
