using Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions;

namespace Domain.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="CommentEvaluationException"></exception>
    public record CommentEvaluation
    {
        public int Value { get; private set; }


        //Cosntructor
        public CommentEvaluation(int value)
        {
            if (value > 5 || value <= 0)
            {
                throw new CommentEvaluationException(Messages.InValidCommentEvaluation);
            }
            Value = value;
        }
    }
}
