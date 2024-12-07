namespace Domain.Features.Comment.ValueObjects.CommentTypePart
{
    public record DomainCommentType
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }


        //Cosntructors
        public DomainCommentType(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
