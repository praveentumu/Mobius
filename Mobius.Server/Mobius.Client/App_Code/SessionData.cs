
namespace FirstGenesis.UI
{
    using System.Diagnostics;
    using System;
    using System.Data;
    using System.Collections;
    using System.Web;


    /// <summary>
    /// This is the class where Functions defined for working with Session Variables
    /// </summary>
    public class SessionData
    {
        #region "Private Variable"
        private HttpContext m_objContext;
        #endregion

        public SessionData()
        {
            m_objContext = HttpContext.Current;
        }
        #region "Public Methods"

        /// <summary>
        /// This is defined for retreiving the Session value of the defined key
        /// </summary>
        /// <param name="sKey">string (defined in Common Class)</param>
        /// <returns>Returns a Object</returns>
        public object getItem(string SessionKey)
        {
            if (m_objContext.Session != null)
            {
                if (m_objContext.Session[SessionKey] == null)
                {
                    return null;
                }
                else
                {
                    return m_objContext.Session[SessionKey];
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        ///  This is defined for modifing the Session value of the defined key
        /// </summary>
        /// <param name="sKey"> string (defined in Common Class)</param>
        /// <param name="Value">value for Session variable</param>
        public void SetItem(string SessionKey, string Value)
        {
            if (m_objContext.Session[SessionKey] != null)
            {
                m_objContext.Session[SessionKey] = Value;
            }
        }

        /// <summary>
        /// This is for creating new session variable
        /// </summary>
        /// <param name="sKey">string (defined in Common Class)</param>
        /// <param name="oValue">value for Session variable</param>
        public void Add(string SessionKey, object oValue)
        {
            if (m_objContext.Session[SessionKey] == null)
            {
                m_objContext.Session.Add(SessionKey, oValue);
            }
            else
            {
                m_objContext.Session[SessionKey] = oValue;
            }
        }

        /// <summary>
        /// This is defined for removing the Session value of the given key
        /// </summary>
        /// <param name="sKey">string (defined in Common Class)</param>
        public void Remove(string SessionKey)
        {
            if (m_objContext.Session[SessionKey] != null)
            {
                m_objContext.Session.Remove(SessionKey);
            }
        }

        /// <summary>
        /// This is defined for removing all the Session Variables.
        /// </summary>
        public void Clear()
        {
            if (!System.Convert.ToBoolean(m_objContext.Session == null))
            {
                m_objContext.Session.Clear();
            }
        }

        /// <summary>
        /// This is to set the Session Time Out Value
        /// </summary>
        /// <param name="intTimeOut"></param>
        public void SessionTimeOut(Int32 intTimeOut)
        {
            HttpContext.Current.Session.Timeout = intTimeOut;
        }
        #endregion
    }
}


