using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class OrderResponse
    {
        public const string resourсeType = "OrderResponse";
        public Identifier identifier; // Идентификатор заказа в ЛИС
        public Link request; // Cсылка на Order
        public DateTime date; //Дата-время результата
        public Link who; // Ссылка на Organisation
        public string orderStatus; // (1.2.643.2.69.1.1.1.45)
        public string description; // Комментарий к результату
        public Link[] fulfillment; // Ссылка на DiagnosticReport
    }
}
