using Application.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.MsSqlDatabase;

public partial class DiplomaProjectMsSqlContext : DiplomaProjectContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<DiplomaProjectMsSqlContext>() // Pobierz konfigurację z User Secrets
                .Build();

            var connectionString = configuration.GetConnectionString("DbString");
            optionsBuilder.UseSqlServer(connectionString);
        }
        else
        {
            throw new NotImplementedException("Configure UserSecrets");
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Address_pk");

            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApartmentNumber).HasMaxLength(100);
            entity.Property(e => e.BuildingNumber).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(10);

            entity.HasOne(d => d.Division).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Address_Division");

            entity.HasOne(d => d.Street).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StreetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Copy_of_Address_Street");
        });

        modelBuilder.Entity<AdministrativeDivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdministrativeDivision_pk");

            entity.ToTable("AdministrativeDivision");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.AdministrativeType).WithMany(p => p.AdministrativeDivisions)
                .HasForeignKey(d => d.AdministrativeTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdministrativeDivision_AdministrativeType");

            entity.HasOne(d => d.Country).WithMany(p => p.AdministrativeDivisions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdministrativeDivision_Country");

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

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.Branches)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Branch_Address");

            entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Branch_Company");

            entity.HasMany(d => d.Offers).WithMany(p => p.Branches)
                .UsingEntity<Dictionary<string, object>>(
                    "BranchOffer",
                    r => r.HasOne<Offer>().WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("BranchOffer_Offer"),
                    l => l.HasOne<Branch>().WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("BranchOffer_Branch"),
                    j =>
                    {
                        j.HasKey("BranchId", "OfferId").HasName("BranchOffer_pk");
                        j.ToTable("BranchOffer");
                    });
        });

        modelBuilder.Entity<BranchCharacteristicsList>(entity =>
        {
            entity.HasKey(e => new { e.QualityId, e.CharacteristicId, e.BranchId }).HasName("BranchCharacteristicsList_pk");

            entity.ToTable("BranchCharacteristicsList");

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchCharacteristicsLists)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchCharacteristicsList_Branch");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.BranchCharacteristicsLists)
                .HasForeignKey(d => d.CharacteristicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchCharacteristicsList_Characteristic");

            entity.HasOne(d => d.Quality).WithMany(p => p.BranchCharacteristicsLists)
                .HasForeignKey(d => d.QualityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BranchCharacteristicsList_Quality");
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
            entity.HasKey(e => new { e.CommentTypeId, e.InternshipId }).HasName("Comment_pk");

            entity.ToTable("Comment");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

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
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Company_pk");

            entity.ToTable("Company");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.User).WithOne(p => p.Company)
                .HasForeignKey<Company>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Company_User");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Internship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Internship_pk");

            entity.ToTable("Internship");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContractNumber).HasMaxLength(100);

            entity.HasOne(d => d.Recruitment).WithMany(p => p.Internships)
                .HasForeignKey(d => new { d.OfferId, d.PersonId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Internship_Recruitment");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Offer_pk");

            entity.ToTable("Offer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.MaxSalary).HasColumnType("money");
            entity.Property(e => e.MinSalary).HasColumnType("money");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NegotiatedSalary)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Paid)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PrivateStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PublishEnd).HasColumnType("datetime");
            entity.Property(e => e.PublishStart).HasColumnType("datetime");
            entity.Property(e => e.RemoteWork)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<OfferCharacteristicsList>(entity =>
        {
            entity.HasKey(e => new { e.CharacteristicId, e.QualityId, e.OfferId }).HasName("OfferCharacteristicsList_pk");

            entity.ToTable("OfferCharacteristicsList");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.OfferCharacteristicsLists)
                .HasForeignKey(d => d.CharacteristicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OfferCharacteristicsList_Characteristic");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferCharacteristicsLists)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OfferCharacteristicsList_Offer");

            entity.HasOne(d => d.Quality).WithMany(p => p.OfferCharacteristicsLists)
                .HasForeignKey(d => d.QualityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OfferCharacteristicsList_Quality");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Person_pk");

            entity.ToTable("Person");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.User).WithOne(p => p.Person)
                .HasForeignKey<Person>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Person_User");
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
            entity.HasKey(e => new { e.OfferId, e.PersonId }).HasName("Recruitment_pk");

            entity.ToTable("Recruitment");

            entity.Property(e => e.AcceptedRejected)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.Comment).HasColumnType("ntext");

            entity.HasOne(d => d.Offer).WithMany(p => p.Recruitments)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recruitment_Offer");

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

        modelBuilder.Entity<TypeUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeURL_pk");

            entity.ToTable("TypeURL");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => new { e.TypeUrlid, e.PublishDate, e.UserId }).HasName("URLs_pk");

            entity.ToTable("URLs");

            entity.Property(e => e.TypeUrlid).HasColumnName("TypeURLId");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");

            entity.HasOne(d => d.TypeUrl).WithMany(p => p.Urls)
                .HasForeignKey(d => d.TypeUrlid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("URLs_TypeURL");

            entity.HasOne(d => d.User).WithMany(p => p.Urls)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("URLs_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CeateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ExpiredToken).HasColumnType("datetime");
            entity.Property(e => e.Logo).HasColumnType("image");
        });

        modelBuilder.Entity<UserCharacteristicsList>(entity =>
        {
            entity.HasKey(e => new { e.QualityId, e.CharacteristicId, e.UserId }).HasName("UserCharacteristicsList_pk");

            entity.ToTable("UserCharacteristicsList");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.UserCharacteristicsLists)
                .HasForeignKey(d => d.CharacteristicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserCharacteristicsList_Characteristic");

            entity.HasOne(d => d.Quality).WithMany(p => p.UserCharacteristicsLists)
                .HasForeignKey(d => d.QualityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserCharacteristicsList_Quality");

            entity.HasOne(d => d.User).WithMany(p => p.UserCharacteristicsLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserCharacteristicsList_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
