using System;
using Mobius.CoreLibrary;
using Mobius.DAL;
using Mobius.Entity;
namespace Mobius.Authorization
{
    public class Authorization
    {
        

        /// <summary>
        /// To retrieve the user information
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public Result GetUserInformation(string serialNumber, out UserInfo userInfo)
        {
            Result result = null;
            userInfo = null;
            try
            {
                result = new Result();
                userInfo = new UserInfo();                
                MobiusDAL mobiusDAL = new MobiusDAL();
                userInfo = mobiusDAL.GetUserInformation(serialNumber);
                result = mobiusDAL.Result;
            }
            catch (Exception ex)
            {
                if (result == null) result = new Result();
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException,ex.Message);
            }
            return result;
        }
    }
}
