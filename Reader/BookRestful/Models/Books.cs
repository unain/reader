//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookRestful.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Books
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Lang { get; set; }
        public decimal Score { get; set; }
        public int Votes { get; set; }
        public bool OnBoard { get; set; }
        public string DataSource { get; set; }
        public System.DateTime EntryTime { get; set; }
        public bool ReadMark { get; set; }
        public bool NewMark { get; set; }
    }
}
