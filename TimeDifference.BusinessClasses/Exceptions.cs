using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class UnAuthorize : Exception
    {
        public UnAuthorize()
        {
        }

        public UnAuthorize(string message)
            : base(message)
        {
        }

        public UnAuthorize(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class EntryNotFound : Exception
    {
        public EntryNotFound()
        {
        }

        public EntryNotFound(string message)
            : base(message)
        {
        }

        public EntryNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
