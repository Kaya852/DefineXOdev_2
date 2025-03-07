using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem
{
    public interface IPayment
    {
        public string pay(long amount);
    }
}
