
                using System;
                namespace PaymentSystem
                {
                    public class ApplePayment : IPayment
                    {
                        public string pay(long amount)
                        {
                        return $"{this.GetType().Name} Class automaticly generated, Payment amount: {amount}";
                        }
                    }
                }