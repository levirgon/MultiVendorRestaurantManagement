﻿using System;
using System.Collections.Generic;
using System.Linq;
using Catalogue.Common.Invariants;

namespace Catalogue.Infrastructure.ValueObject
{
    public class MoneyValue : CustomValueObject
    {
        private MoneyValue(decimal amount, string currency)
        {
            Value = amount;
            Currency = currency;
        }

        public decimal Value { get; }

        public string Currency { get; }

        public string PriceTag => Currency + "" + Value;

        public static MoneyValue Of(decimal value)
        {
            SupportedCurrency currency;
            if (value >= 1)
                currency = SupportedCurrency.Euro;
            else
                currency = SupportedCurrency.Cent;

            return new MoneyValue(value, currency.ToDescriptionString());
        }

        public static MoneyValue Of(MoneyValue value)
        {
            return new MoneyValue(value.Value, value.Currency);
        }

        public static MoneyValue operator +(MoneyValue moneyValueLeft, MoneyValue moneyValueRight)
        {
            if (moneyValueLeft.Currency != moneyValueRight.Currency) throw new ArgumentException();

            return new MoneyValue(moneyValueLeft.Value + moneyValueRight.Value, moneyValueLeft.Currency);
        }

        public static MoneyValue operator *(int number, MoneyValue moneyValueRight)
        {
            return new MoneyValue(number * moneyValueRight.Value, moneyValueRight.Currency);
        }

        public static MoneyValue operator *(decimal number, MoneyValue moneyValueRight)
        {
            return new MoneyValue(number * moneyValueRight.Value, moneyValueRight.Currency);
        }
    }

    public static class SumExtensions
    {
        public static MoneyValue Sum<T>(this IEnumerable<T> source, Func<T, MoneyValue> selector)
        {
            return MoneyValue.Of(source.Select(selector).Aggregate((x, y) => x + y));
        }

        public static MoneyValue Sum(this IEnumerable<MoneyValue> source)
        {
            return source.Aggregate((x, y) => x + y);
        }
    }
}