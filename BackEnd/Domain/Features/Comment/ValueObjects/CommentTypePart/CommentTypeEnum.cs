namespace Domain.Features.Comment.ValueObjects.CommentTypePart
{
    public enum CommentTypeEnum
    {
        Dzienny = 1,
        Tygodniowy = 7,
        Miesięczny = 30,
        Kwartalny = 90,
        Połroczny = 182,
        Roczny = 365,
        Końcowa = 1000,
        Dowolna = 1002,
        PozwolenieNaPublikacje = 1004
    }
}
