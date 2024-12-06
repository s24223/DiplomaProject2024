using Application.Databases.Relational.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Databases.Relational;

public partial class DiplomaProjectContext : DbContext
{
    public DiplomaProjectContext()
    {
    }

    public DiplomaProjectContext(DbContextOptions<DiplomaProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AdministrativeDivision> AdministrativeDivisions { get; set; }

    public virtual DbSet<AdministrativeType> AdministrativeTypes { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BranchOffer> BranchOffers { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<CharacteristicType> CharacteristicTypes { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentType> CommentTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Models.Exception> Exceptions { get; set; }

    public virtual DbSet<Internship> Internships { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationSender> NotificationSenders { get; set; }

    public virtual DbSet<NotificationStatus> NotificationStatuses { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferCharacteristic> OfferCharacteristics { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonCharacteristic> PersonCharacteristics { get; set; }

    public virtual DbSet<Quality> Qualities { get; set; }

    public virtual DbSet<Recruitment> Recruitments { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<UrlType> UrlTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }
}
