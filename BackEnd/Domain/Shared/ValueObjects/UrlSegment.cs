﻿using Domain.Shared.Exceptions.AppExceptions.ValueObjectsExceptions;
using System.Text.RegularExpressions;

namespace Domain.Shared.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="UrlSegmentException"></exception>
    public record UrlSegment
    {
        public string Value { get; private set; }

        public UrlSegment(string value)
        {
            if (!IsValidSegmentUrl(value))
            {
                throw new UrlSegmentException(Messages.InValidSegmentUrl);
            }
            Value = value;
        }

        private bool IsValidSegmentUrl(string segmentUrl)
        {
            return Regex.IsMatch(segmentUrl, @"^[a-zA-Z0-9\-_]+$");
        }
    }
}