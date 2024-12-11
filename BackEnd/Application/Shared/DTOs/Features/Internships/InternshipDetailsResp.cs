namespace Application.Shared.DTOs.Features.Internships
{
    public class InternshipDetailsResp
    {
        public int Count { get; init; } = 0;
        public bool CompanyPermissionForPublication { get; init; } = false;
        public bool PersonPermissionForPublication { get; init; } = false;
        public int? CompanyEndEvaluation { get; init; } = null;
        public int? PersonEndEvaluation { get; init; } = null;
        public double? CompanyAvgEvaluationInTime { get; init; } = null;
        public double? PersonAvgEvaluationInTime { get; init; } = null;
    }
}
