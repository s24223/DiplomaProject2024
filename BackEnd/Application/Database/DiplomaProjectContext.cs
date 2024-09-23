﻿using Application.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace Application.Database;

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

    public virtual DbSet<BranchCharacteristicsList> BranchCharacteristicsLists { get; set; }

    public virtual DbSet<BranchOffer> BranchOffers { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<CharacteristicType> CharacteristicTypes { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentType> CommentTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Application.Database.Models.Exception> Exceptions { get; set; }

    public virtual DbSet<Internship> Internships { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonCharacteristicsList> PersonCharacteristicsLists { get; set; }

    public virtual DbSet<Quality> Qualities { get; set; }

    public virtual DbSet<Recruitment> Recruitments { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<UrlType> UrlTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProblem> UserProblems { get; set; }
}
