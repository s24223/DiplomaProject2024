﻿using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record UserId : GuidId
    {
        public UserId(Guid? value) : base(value)
        {
        }
    }
}