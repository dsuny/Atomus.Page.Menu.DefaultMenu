using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomus.Page.Menu
{
    public class MenuItem
    {
        public decimal MenuID { get; set; }
        public decimal AssemblyID { get; set; }
        public bool VisibleOne { get; set; }
        public string Title { get; set; }
        public Xamarin.Forms.Page Page { get; set; }
    }
}