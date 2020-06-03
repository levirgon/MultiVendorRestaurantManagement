﻿namespace BasketManagement.Domain.Base
{
    public class ConditionMustBeTrueRule : IBusinessRule
    {
        private readonly bool _conditionToBeValid;

        public ConditionMustBeTrueRule(bool conditionToBeValid, string errorErrorMessage)
        {
            _conditionToBeValid = conditionToBeValid;
            ErrorMessage = errorErrorMessage;
        }

        public bool IsBroken()
        {
            return !_conditionToBeValid;
        }

        public string ErrorMessage { get; }
    }
}