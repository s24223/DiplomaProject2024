using Domain.Shared.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Shared.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="UrlSegmentException"></exception>
    public record UrlSegment
    {
        //Values
        public string Value { get; private set; }


        //Cosntructor
        public UrlSegment(string value)
        {
            if (!IsValidSegmentUrl(value))
            {
                throw new UrlSegmentException(Messages.SegmentUrl_Value_Invalid);
            }
            Value = value;
        }


        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        //Public Methods
        public static implicit operator UrlSegment?(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            else
                return new UrlSegment(value);
        }

        public static implicit operator string?(UrlSegment? value)
        {
            return value switch
            {
                null => null,
                _ => value.Value,
            };
        }

        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        //Private Methods
        private bool IsValidSegmentUrl(string segmentUrl)
        {
            return Regex.IsMatch(segmentUrl, @"^[a-zA-Z0-9\-_]+$");
        }
    }
}
