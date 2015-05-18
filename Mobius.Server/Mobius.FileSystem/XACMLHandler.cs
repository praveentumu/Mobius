using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using System.IO;
using System.Xml.Linq;

namespace Mobius.FileSystem
{
  public class XACMLHandler
  {
      public XACMLHandler()
      {

      }
      /// <summary>
      /// This method will parse the XML to fetch and return the purpose from the XACML data.
      /// </summary>
      /// <param name="xdom"></param>
      /// <returns></returns>
      public string GetPurpose(XmlDocument xdom)
      {
          string str = string.Empty;
          try
          {
              XmlNodeList xmlnodeList = xdom.SelectNodes("//Actions");

              foreach (XmlNode node in xmlnodeList)
              {
                  if (node != null)
                  {
                      str = node.InnerText;
                  }
              }
          }
          catch (Exception)
          {
              //TO DO
          }
          return str;
      }

      /// <summary>
      /// This method will return the XACML document metadata.
      /// </summary>
      /// <param name="XACMLByteData"></param>
      /// <returns></returns>
      public XACMLClass GetXACMLDocumentDetail(byte[] XACMLByteData)
      {
          XACMLClass objXACMLClass = new XACMLClass();
          //XmlDocument xmldoc = new XmlDocument();
          //xmldoc.Load(ms);
          try
          {
              MemoryStream ms = new MemoryStream(XACMLByteData);

              var xdoc = XDocument.Load(ms);


              IEnumerable<XElement> subjectElement = xdoc.Root.Descendants("Subjects").Elements("Subject");
              objXACMLClass.Subject = new List<string>();
              foreach (XElement node in subjectElement)
              {
                  if (node != null)
                  {
                      objXACMLClass.Subject.Add(node.Value);
                  }
              }


              //To fetch the Purpose from Policy/XACML document.
              IEnumerable<XElement> actionElement = xdoc.Root.Descendants("Actions");
              foreach (XElement node in actionElement)
              {
                  if (node != null)
                  {
                      objXACMLClass.PurposeofUse = node.Value;
                  }
              }

              var nodes = from x in xdoc.Root.Descendants("Condition").Elements("Apply")
                          select new
                          {
                              RuleDate = x.Value
                          };

              foreach (var node in nodes)
              {
                  if (!String.IsNullOrEmpty(objXACMLClass.RuleStartDate))
                      objXACMLClass.RuleEndDate = node.RuleDate;
                  if (String.IsNullOrEmpty(objXACMLClass.RuleStartDate))
                      objXACMLClass.RuleStartDate = node.RuleDate;
              }
             
          }
          catch (Exception)
          {

              // TO DO
          }

          return objXACMLClass;
      }

  }
}