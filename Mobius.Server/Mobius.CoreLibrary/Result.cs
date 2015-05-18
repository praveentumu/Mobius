using System;

namespace Mobius.CoreLibrary
{
    [Serializable]
    public class Result
    {
        private ErrorCode _errorCode;
        private string _errorMessage;
        private bool _IsSuccess = false;

        /// <summary>
        /// 
        /// </summary>
        public ErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        public void SetError(ErrorCode errorCode)
        {
            this._errorCode = errorCode;
            this._errorMessage = Helper.GetErrorMessage(errorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public void SetError(ErrorCode errorCode, string errorMessage)
        {
            this._errorMessage = errorMessage;
            this._errorCode = errorCode;
        }
    }
}
