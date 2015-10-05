using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestTest.Classes
{
    class Period
    {
        Period(DateTime dateStart, DateTime dateEnd)
        {
            start = dateStart;
            end = dateEnd;
        }

        private DateTime start;
        private DateTime end;
    }
}
