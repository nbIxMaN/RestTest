using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RestTest.Tests_Method
{
    [TestFixture]
    class GetOrder
    {
        [Test]
        public void Test()
        {
            GetRequest g = new GetRequest
            {
                parameter = new Parameter[]
                {
                    new Parameter
                    {
                        name = "TargetCode",
                        valueString = "123.456.789"
                    },
                    new Parameter
                    {
                        name = "Barcode",
                        valueString ="barcode"
                    }
                }
            };
        }
    }
}
