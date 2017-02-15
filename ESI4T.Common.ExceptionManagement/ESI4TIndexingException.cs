using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.ExceptionManagement
{
    public class ESI4TIndexingException: ApplicationException
    {
        public ESI4TIndexingException() : base()
        {
        }
        public string Code { get; set; }

        public ESI4TIndexingException(string code, string errorMessage)
			: base(errorMessage)
		{
            Code = code;
        }

        public ESI4TIndexingException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        public ESI4TIndexingException(string msg)
            : base(msg)
        {
        }
    }
}
