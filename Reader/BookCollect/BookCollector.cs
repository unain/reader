using HTMLFetchAndParse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookCollect
{
    class BookCollector
    {
        public HTMLParser Parser { set; get; }
        public HTMLFetcher Fetcher { set; get; }
        public String BaseUri { set; get; }
        public String XpathISBN { set; get; }
        public String XpathBookUri { set; get; }
        public String PageEncoding { set; get; }
        public String Domain { set; get; }

        public BookCollector()
        {
            Parser = new HTMLParser();
            Fetcher = new HTMLFetcher();
            BaseUri = "http://book.douban.com/chart?subcat=I";
            XpathISBN = "//div[@id=\"info\"]/span[last()]/following-sibling::text()[1]";
            XpathBookUri = "//a[@class=\"fleft\"]";
            PageEncoding = "utf-8";
            Domain = new Uri(BaseUri).Host;
        }
        public virtual Books GetBookFromPage(String PageContent, String ISBN)
        {
            Books book = new Books();
            book.ISBN = ISBN;
            book.Title = Parser.ParseHtmlSingleNodeContent(PageContent, "//span[@property=\"v:itemreviewed\"]", false, true);
            book.Author = Parser.ParseHtmlSingleNodeContent(PageContent, "//div[@id=\"info\"]/span/span[contains(.,'作者')]/following-sibling::a", false, true);
            var lang = Parser.ParseHtmlSingleNodeContent(PageContent, "//div[@id=\"info\"]/span/span[contains(.,'译者')]", false, true);
            if (lang == null) book.Lang = "CH"; else book.Lang = "XX";
            book.Score = decimal.Parse(Parser.ParseHtmlSingleNodeContent(PageContent, "//strong[@class='ll rating_num ']", false, true)) * 10;
            book.Votes = int.Parse(Parser.ParseHtmlSingleNodeContent(PageContent, "//span[@property='v:votes']", false, true));
            book.OnBoard = true;
            book.DataSource = "DB";
            book.EntryTime = DateTime.Now;
            book.ReadMark = false;
            book.NewMark = true;
           
            return book;
        }
        public void Run()
        {
            UpdateBook();
            BaseUri = "http://book.douban.com/chart?subcat=F";
            UpdateBook();
        }
        public virtual void UpdateBook()
        {
            var content = Fetcher.LoadPage(BaseUri, PageEncoding);
            var booklist = Parser.ParseHtmlColloctionNodeAttribute(content, "//a[@class=\"fleft\"]", "href", false, true);
            foreach (var uri in booklist)
            {
                content = Fetcher.LoadPage(uri, PageEncoding);
                var ISBN = Parser.ParseHtmlSingleNodeContent(content, "//div[@id=\"info\"]/span[last()]/following-sibling::text()[1]", false, true).Trim();
                using (var context = new IBookEntities())
                {
                    if (!context.Books.Any<Books>(b => b.ISBN == ISBN))
                    {
                        context.Books.AddObject(GetBookFromPage(content, ISBN));
                        context.SaveChanges();
                    }
                }
            }
        }

        public virtual IList<String> GetISBNList()
        {
            List<String> lists = new List<string>();

            var content = Fetcher.LoadPage(BaseUri, PageEncoding);
            var booklist = Parser.ParseHtmlColloctionNodeAttribute(content,XpathBookUri, "href" , false, true);
            foreach (var uri in booklist)
            {
                content = Fetcher.LoadPage(uri, PageEncoding);
                var ISBN = Parser.ParseHtmlSingleNodeContent(content, XpathISBN, false, true).Trim();
                lists.Add(ISBN);
            }
            return lists;
        }

    }
}
