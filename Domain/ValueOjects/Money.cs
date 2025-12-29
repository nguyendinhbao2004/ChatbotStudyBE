using Domain.Common;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueOjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } //VND, USD, EUR

        public Money()
        {
            
        }

        public Money(decimal amount, string curency)
        {
            if(amount < 0) 
                throw new DomainException("Amount must be non-negative");
            if(string.IsNullOrWhiteSpace(curency))
                throw new DomainException("Currency is required");
            Amount = amount;
            Currency = curency;
        }

        public static Money USD(decimal amount)
        {
            return new Money(amount, "USD");
        }
        public static Money VND(decimal amount)
        {
            return new Money(amount, "VND");
        }

        public static Money operator +(Money left, Money right)
        {
            if(left.Currency != right.Currency)
                throw new DomainException("Cannot add Money with different currencies");
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
