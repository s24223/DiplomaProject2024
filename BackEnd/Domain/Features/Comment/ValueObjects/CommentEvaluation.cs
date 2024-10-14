using Domain.Features.Comment.Exceptions.ValueObjects;

namespace Domain.Features.Comment.ValueObjects
{
    public record CommentEvaluation
    {
        //Value
        public int Value { get; private set; }


        //Cosntructor
        public CommentEvaluation(int value)
        {
            if (value > 5 || value <= 0)
            {
                throw new CommentEvaluationException(Messages.CommentEvaluation_Value_Invalid);
            }
            Value = value;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
