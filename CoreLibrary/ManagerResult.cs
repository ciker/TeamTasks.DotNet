using System.Collections.Generic;

namespace CoreLibrary
{
    public class ManagerResult
    {
        public bool Success { get; private set; }

        public IEnumerable<string> Errors { get; private set; }

        public ManagerResult()
        {
            Success = true;
        }
        
        public ManagerResult(params string[] errors)
        {
            if (errors == null)
            {
                Success = true;
            }
            else
            {
                Errors = errors;
                Success = false;
            }
        }
    }

}
