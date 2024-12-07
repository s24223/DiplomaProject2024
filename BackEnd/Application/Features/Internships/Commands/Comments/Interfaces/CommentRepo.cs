using Application.Databases.Relational;
using Application.Features.Internships.Mappers;

namespace Application.Features.Internships.Commands.Comments.Interfaces
{
    public class CommentRepo : ICommentRepo
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IInternshipMapper _mapper;

        //Constructor
        public CommentRepo(
            DiplomaProjectContext context,
            IInternshipMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods


        /*
         public Guid InternshipId { get; set; }

            public int CommentTypeId { get; set; }

            public DateTime Created { get; set; }

            public string Description { get; set; } = null!;

            public int? Evaluation { get; set; }

         */
    }
}
