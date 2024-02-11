using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.Domain.Enums
{
    public enum StatusCode
    {
        //Users
        USER_NOT_FOUND = 1,
        USER_ALREADY_EXISTS = 2,
        USER_BLOCKED = 3,

        //General
        OK=200,
        SERVER_ERROR=500,
    }
}
