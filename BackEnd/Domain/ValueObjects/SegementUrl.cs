using Domain.Exceptions.UserExceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record SegementUrl
    {
        public string Value { get; private set; }

        public SegementUrl(string value)
        {
            if (!IsValidSegmentUrl(value))
            {
                throw new SegementUrlException(Messages.InValidSegmentUrl);
            }
            Value = value;
        }

        private bool IsValidSegmentUrl(string segmentUrl)
        {
            return Regex.IsMatch(segmentUrl, @"^[a-zA-Z0-9\-_]+$");
        }
    }
}
