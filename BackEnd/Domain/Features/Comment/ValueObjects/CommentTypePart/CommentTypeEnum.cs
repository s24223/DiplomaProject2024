namespace Domain.Features.Comment.ValueObjects.CommentTypePart
{
    public enum CommentTypeEnum
    {
        Dzienny = 1,
        Tygodniowy = 7,
        Miesieczny = 30,
        Kwartalny = 90,
        PołRoczny = 182,
        Roczny = 365,
        Koncowa = 1000,
        Dowolna = 1002,
        PozwolenieNaPublikacje = 1004
    }
}
