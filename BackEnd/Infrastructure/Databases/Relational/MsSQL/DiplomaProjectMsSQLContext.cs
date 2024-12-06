using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Infrastructure.Exceptions.AppExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases.Relational.MsSQL;

public partial class DiplomaProjectMsSQLContext : DiplomaProjectContext
{
    //Values
    private readonly IConfiguration _configuration;


    //Cosntructor
    public DiplomaProjectMsSQLContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    //======================================================================================================
    //======================================================================================================
    //======================================================================================================s
    //Methods
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetSection("ConnectionStrings")["DbString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InfrastructureLayerException(Messages.NotConfiguredConnectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(connectionString);
            //.LogTo(Console.WriteLine);
            //.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }); // Logowanie tylko SQL
        }
        else
        {
            throw new InfrastructureLayerException(Messages.NotConfiguredUserSecrets);
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Address_pk");

            entity.ToTable("Address");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ApartmentNumber).HasMaxLength(100);
            entity.Property(e => e.BuildingNumber).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(10);

            entity.HasOne(d => d.Division).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Address_Division");

            entity.HasOne(d => d.Street).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("Address_Street");
        });

        modelBuilder.Entity<AdministrativeDivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdministrativeDivision_pk");

            entity.ToTable("AdministrativeDivision");

            entity.HasIndex(e => e.ParentDivisionId, "IDX_AdministrativeDivision_ParentDivisionId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.AdministrativeType).WithMany(p => p.AdministrativeDivisions)
                .HasForeignKey(d => d.AdministrativeTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdministrativeDivision_AdministrativeType");

            entity.HasOne(d => d.ParentDivision).WithMany(p => p.InverseParentDivision)
                .HasForeignKey(d => d.ParentDivisionId)
                .HasConstraintName("Division_Division");

            entity.HasMany(d => d.Streets).WithMany(p => p.Divisions)
                .UsingEntity<Dictionary<string, object>>(
                    "DivisionStreet",
                    r => r.HasOne<Street>().WithMany()
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DivisionStreet_Street"),
                    l => l.HasOne<AdministrativeDivision>().WithMany()
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DivisionStreet_AdministrativeDivision"),
                    j =>
                    {
                        j.HasKey("DivisionId", "StreetId").HasName("DivisionStreet_pk");
                        j.ToTable("DivisionStreet");
                    });
        });

        modelBuilder.Entity<AdministrativeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdministrativeType_pk");

            entity.ToTable("AdministrativeType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Branch_pk");

            entity.ToTable("Branch");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UrlSegment)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.Branches)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("Branch_Address");

            entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Branch_Company");
        });

        modelBuilder.Entity<BranchOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BranchOffer_pk");

            entity.ToTable("BranchOffer");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PublishEnd)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PublishStart)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchOffers)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchOffer_Branch");

            entity.HasOne(d => d.Offer).WithMany(p => p.BranchOffers)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchOffer_Offer");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Characteristic_pk");

            entity.ToTable("Characteristic");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CharacteristicType).WithMany(p => p.Characteristics)
                .HasForeignKey(d => d.CharacteristicTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Characteristic_CharacteristicType");

            entity.HasMany(d => d.ChildCharacteristics).WithMany(p => p.ParentCharacteristics)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacteristicColocation",
                    r => r.HasOne<Characteristic>().WithMany()
                        .HasForeignKey("ChildCharacteristicId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CharacteristicColocation_CharacteristicP"),
                    l => l.HasOne<Characteristic>().WithMany()
                        .HasForeignKey("ParentCharacteristicId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CharacteristicColocation_Characteristic"),
                    j =>
                    {
                        j.HasKey("ParentCharacteristicId", "ChildCharacteristicId").HasName("CharacteristicColocation_pk");
                        j.ToTable("CharacteristicColocation");
                    });

            entity.HasMany(d => d.ParentCharacteristics).WithMany(p => p.ChildCharacteristics)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacteristicColocation",
                    r => r.HasOne<Characteristic>().WithMany()
                        .HasForeignKey("ParentCharacteristicId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CharacteristicColocation_Characteristic"),
                    l => l.HasOne<Characteristic>().WithMany()
                        .HasForeignKey("ChildCharacteristicId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CharacteristicColocation_CharacteristicP"),
                    j =>
                    {
                        j.HasKey("ParentCharacteristicId", "ChildCharacteristicId").HasName("CharacteristicColocation_pk");
                        j.ToTable("CharacteristicColocation");
                    });
        });

        modelBuilder.Entity<CharacteristicType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CharacteristicType_pk");

            entity.ToTable("CharacteristicType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.CommentTypeId, e.Created, e.InternshipId }).HasName("Comment_pk");

            entity.ToTable("Comment");

            entity.Property(e => e.Created).HasColumnType("datetime");

            entity.HasOne(d => d.CommentType).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CommentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comment_CommentType");

            entity.HasOne(d => d.Internship).WithMany(p => p.Comments)
                .HasForeignKey(d => d.InternshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comment_Internship");
        });

        modelBuilder.Entity<CommentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CommentType_pk");

            entity.ToTable("CommentType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Company_pk");

            entity.ToTable("Company");

            entity.HasIndex(e => e.ContactEmail, "Company_UNIQUE_ContactEmail").IsUnique();

            entity.HasIndex(e => e.Name, "Company_UNIQUE_Name").IsUnique();

            entity.HasIndex(e => e.Regon, "Company_UNIQUE_Regon").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.Created).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Regon)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.UrlSegment)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Company)
                .HasForeignKey<Company>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Company_User");
        });

        modelBuilder.Entity<Application.Databases.Relational.Models.Exception>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Exception_pk");

            entity.ToTable("Exception");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExceptionType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("C")
                .IsFixedLength();
        });

        modelBuilder.Entity<Internship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Internship_pk");

            entity.ToTable("Internship", tb => tb.HasTrigger("Internship_UNIQUE_ContractNumber"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContractNumber).HasMaxLength(100);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Recruitment).WithOne(p => p.Internship)
                .HasForeignKey<Internship>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Internship_Recruitment");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notification_pk");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Completed).HasColumnType("datetime");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsReadedByUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();

            entity.HasOne(d => d.NotificationSender).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notification_NotificationSender");

            entity.HasOne(d => d.NotificationStatus).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notification_NotificationStatus");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Notification_User");
        });

        modelBuilder.Entity<NotificationSender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NotificationSender_pk");

            entity.ToTable("NotificationSender");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<NotificationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NotificationStatus_pk");

            entity.ToTable("NotificationStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Offer_pk");

            entity.ToTable("Offer");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsForStudents)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.IsNegotiatedSalary)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaxSalary).HasColumnType("money");
            entity.Property(e => e.MinSalary).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<OfferCharacteristic>(entity =>
        {
            entity.HasKey(e => new { e.OfferId, e.CharacteristicId }).HasName("OfferCharacteristic_pk");

            entity.ToTable("OfferCharacteristic");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.OfferCharacteristics)
                .HasForeignKey(d => d.CharacteristicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OfferCharacteristicsList_Characteristic");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferCharacteristics)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OfferCharacteristicsList_Offer");

            entity.HasOne(d => d.Quality).WithMany(p => p.OfferCharacteristics)
                .HasForeignKey(d => d.QualityId)
                .HasConstraintName("OfferCharacteristicsList_Quality");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Person_pk");

            entity.ToTable("Person");

            entity.HasIndex(e => e.ContactEmail, "Person_UNIQUE_ContactEmail").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.ContactPhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Created).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.CvUrl).HasMaxLength(100);
            entity.Property(e => e.IsPublicProfile)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.IsStudent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);
            entity.Property(e => e.UrlSegment)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.People)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("Person_Address");

            entity.HasOne(d => d.User).WithOne(p => p.Person)
                .HasForeignKey<Person>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Person_User");
        });

        modelBuilder.Entity<PersonCharacteristic>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.CharacteristicId }).HasName("PersonCharacteristic_pk");

            entity.ToTable("PersonCharacteristic");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.PersonCharacteristics)
                .HasForeignKey(d => d.CharacteristicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PersonCharacteristicsList_Characteristic");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonCharacteristics)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PersonCharacteristicsList_Person");

            entity.HasOne(d => d.Quality).WithMany(p => p.PersonCharacteristics)
                .HasForeignKey(d => d.QualityId)
                .HasConstraintName("PersonCharacteristicsList_Quality");
        });

        modelBuilder.Entity<Quality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Quality_pk");

            entity.ToTable("Quality");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CharacteristicType).WithMany(p => p.Qualities)
                .HasForeignKey(d => d.CharacteristicTypeId)
                .HasConstraintName("Quality_CharacteristicType");
        });

        modelBuilder.Entity<Recruitment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Recruitment_pk");

            entity.ToTable("Recruitment", tb => tb.HasTrigger("Recruitment_Invalid_BranchOffer"));

            entity.HasIndex(e => new { e.PersonId, e.BranchOfferId }, "Recruitment_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CvUrl).HasMaxLength(100);
            entity.Property(e => e.IsAccepted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.BranchOffer).WithMany(p => p.Recruitments)
                .HasForeignKey(d => d.BranchOfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recruitment_BranchOffer");

            entity.HasOne(d => d.Person).WithMany(p => p.Recruitments)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recruitment_Person");
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Street_pk");

            entity.ToTable("Street");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.AdministrativeType).WithMany(p => p.Streets)
                .HasForeignKey(d => d.AdministrativeTypeId)
                .HasConstraintName("Street_AdministrativeType");
        });

        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => new { e.Created, e.UrlTypeId, e.UserId }).HasName("Url_pk");

            entity.ToTable("Url");

            entity.HasIndex(e => new { e.Path, e.UserId }, "Url_UNIQUE_Path").IsUnique();

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Path).HasMaxLength(800);

            entity.HasOne(d => d.UrlType).WithMany(p => p.Urls)
                .HasForeignKey(d => d.UrlTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Url_UrlType");

            entity.HasOne(d => d.User).WithMany(p => p.Urls)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Url_User");
        });

        modelBuilder.Entity<UrlType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UrlType_pk");

            entity.ToTable("UrlType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pk");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "User_UNIQUE_Login").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExpiredToken).HasColumnType("datetime");
            entity.Property(e => e.IsHideProfile)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.LastLoginIn).HasColumnType("datetime");
            entity.Property(e => e.LastPasswordUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.ResetPasswordInitiated).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
