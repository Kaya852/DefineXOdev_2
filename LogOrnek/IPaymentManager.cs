using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem
{
    public interface IPaymentManager
    {
        public bool CheckConfigFile();
        public Dictionary<string, string> GetPaymentTypes();
        public bool SetPaymentType(string LoggerType);
        public string pay(long amount);

    }
}
