using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mobius.CoreLibrary;
namespace Ediable_Repeater
{
    public class Data
    {
        public Data()
        {
        }


        public List<Names> Names
        {
            get
            {
                if (HttpContext.Current.Session["names"] == null)
                {
                    HttpContext.Current.Session["names"] = new List<Names>();
                }
                return HttpContext.Current.Session["names"] as List<Names>;
            }
            set
            {
                HttpContext.Current.Session["names"] = value;
            }
        }
        public List<MobiusServiceLibrary.Telephone> Telephone
        {
            get
            {
                if (HttpContext.Current.Session["telephone"] == null)
                {
                    HttpContext.Current.Session["telephone"] = new List<MobiusServiceLibrary.Telephone>();
                }
                return HttpContext.Current.Session["telephone"] as List<MobiusServiceLibrary.Telephone>;
            }
            set
            {
                HttpContext.Current.Session["telephone"] = value;
            }
        }
        public List<MobiusServiceLibrary.Address> Address
        {
            get
            {
                if (HttpContext.Current.Session["address"] == null)
                {
                    HttpContext.Current.Session["address"] = new List<MobiusServiceLibrary.Address>();
                }
                return HttpContext.Current.Session["address"] as List<MobiusServiceLibrary.Address>;
            }
            set
            {
                HttpContext.Current.Session["address"] = value;
            }
        }
        
    }


    public class Names
    {
        
        public int IDName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrefixName { get; set; }
        public string SuffixName { get; set; }
        public ActionType Action { get; set; }

    }
  
}