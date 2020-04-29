﻿using MultiVendorRestaurantManagement.Domain.Base;

namespace MultiVendorRestaurantManagement.Domain.Rules
{
    public class OpeningAndClosingHoursAreValid : IBusinessRule
    {
        private readonly int _openingHour;
        private readonly int _closingHour;

        public OpeningAndClosingHoursAreValid(int openingHour, int closingHour)
        {
            _openingHour = openingHour;
            _closingHour = closingHour;
        }

        public bool IsBroken()
        {
            return _openingHour < 0 || _openingHour > 24 || _closingHour < 0 || _closingHour > 24 ||
                   _openingHour < _closingHour;
        }

        public string Message => "opening and closing hours must be valid";
    }
}