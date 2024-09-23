﻿namespace Domain.Templates.ValueObjects.EntityIdentificators
{
    public record IntId
    {
        public int Value { get; private set; }

        public IntId(int value)
        {
            Value = value;
        }
    }
}
