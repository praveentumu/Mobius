using System;


namespace FirstGenesis.UI
{    
    /// <summary>
    /// This is the Global Class used for defining the sessions
    /// </summary>
    public class GlobalSessions
    {

        #region "Public Methods"

        /// <summary>
        /// This is defined for retreiving the Session value of the defined key
        /// </summary>
        /// <param name="sKey">enumSessionItem (defined in Common Class)</param>
        /// <returns>string value for Session Item</returns>
        public static object SessionItem(SessionItem sKey)
        {            
            SessionData oSession = new SessionData();
            object sValue = oSession.getItem(sKey.ToString());

            oSession = null;
            return sValue;
        } 
        

        /// <summary>
        /// to Add the Session values
        /// </summary>
        /// <param name="sKey">enumSessionItem (defined in Common Class), value for session</param>
        /// <param name="oValue">object as Session Value</param>
        public static void SessionAdd(SessionItem sKey, object oValue)
        {
            SessionData oSession = new SessionData();
            oSession.Add(sKey.ToString(), oValue);
            oSession = null;
        }

        /// <summary>
        /// to remove the Session values for the given key
        /// </summary>
        /// <param name="sKey">enumSessionItem (defined in Common Class)</param>
        public static void SessionRemove(SessionItem sKey)
        {
            SessionData oSession = new SessionData();
            oSession.Remove(sKey.ToString());
            oSession = null;
        }

        /// <summary>
        /// to remove all the Session Variables
        /// </summary>
        public static void SessionRemoveAll()
        {
            SessionData oSession = new SessionData();
            oSession.Clear();
            oSession = null;
        }              
        #endregion        
    }
}
