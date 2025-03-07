
                using System;
                namespace PaymentSystem
                {
                    public class PayfixPayment : IPayment
                    {
                        public string pay(long amount)
                        {
                        return $"{this.GetType().Name} Class automaticly generated, Payment amount: {amount}";
                        }
                    }
                }