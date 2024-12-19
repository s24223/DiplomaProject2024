using Application.Shared.DTOs.Features.Internships;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Intership.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Infrastructure.Exceptions.AppExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace Infrastructure.Databases.Relational.MsSQL.SqlClientRepositories
{
    public class SqlClientRepo : ISqlClientRepo
    {
        //Values
        private readonly string _connectionString;


        //Constructor
        public SqlClientRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings")["DbString"] ??
                throw new InfrastructureLayerException(Messages.NotConfiguredConnectionString);
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        public async Task<InternshipDetailsResp> GetInternshipDetailsAsync
            (
            RecrutmentId recrutmentId,
            UserId userId,
            CancellationToken cancellation
            )
        {
            try
            {
                await using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync(cancellation);

                    await using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;

                        com.CommandText = "GET_PARAMITERS_BY_INTERSHIP";
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@IntershipId", recrutmentId.Value);
                        com.Parameters.AddWithValue("@UserId", userId.Value);

                        await using var reader = await com.ExecuteReaderAsync(cancellation);
                        int totalCount = 0;
                        bool comapanyAllowedToPublish = false;
                        bool personAllowedToPublish = false;
                        int? comapanyEndEvaluation = -1;
                        int? personEndEvaluation = -1;
                        double? comapanyAvgInTimeEvaluation = -1;
                        double? personAvgInTimeEvaluation = -1;

                        while (await reader.ReadAsync(cancellation))
                        {
                            totalCount = (int)reader["TOTAL_COUNT"];
                            comapanyAllowedToPublish = (int)reader["COMPANY_PUBLISH_ALLOWED_COUNT"] > 0;
                            personAllowedToPublish = (int)reader["PERSON_PUBLISH_ALLOWED_COUNT"] > 0;
                            comapanyEndEvaluation = (int)reader["COMPANY_END"];
                            personEndEvaluation = (int)reader["PERSON_END"];
                            comapanyAvgInTimeEvaluation = (double)reader["AVG_COMPANY_IN"];
                            personAvgInTimeEvaluation = (double)reader["AVG_PERSON_IN"];

                        }
                        reader.Close();
                        if (comapanyAvgInTimeEvaluation.HasValue && comapanyAvgInTimeEvaluation < 0)
                        {
                            comapanyAvgInTimeEvaluation = null;
                        }
                        return new InternshipDetailsResp
                        {
                            Count = totalCount,
                            CompanyPermissionForPublication = comapanyAllowedToPublish,
                            PersonPermissionForPublication = personAllowedToPublish,
                            CompanyEndEvaluation = comapanyEndEvaluation,
                            PersonEndEvaluation = personEndEvaluation,
                            CompanyAvgEvaluationInTime = comapanyAvgInTimeEvaluation,
                            PersonAvgEvaluationInTime = personAvgInTimeEvaluation
                        };
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, $"RecrutmentId: {recrutmentId.Value},\r\nUserId: {userId.Value}");
            }
        }

        public async Task<(int DivisionId, int? StreetId)> GetDivisionIdStreetIdAsync
            (
            string wojewodztwo,
            string? powiat,
            string? gmina,
            string city,
            string? dzielnica,
            string? street,
            CancellationToken cancellation
            )
        {
            try
            {
                await using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync(cancellation);

                    await using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;

                        com.CommandText = "FIND_AdministrativeDivision_Street";
                        com.CommandType = CommandType.StoredProcedure;

                        var dbPowiat = AdaptString(powiat);
                        var dbGmina = AdaptString(gmina);
                        var dbDzielnica = AdaptString(dzielnica);
                        var dbStreet = AdaptString(street);

                        com.Parameters.AddWithValue("@WOJ_NAME",
                            wojewodztwo);
                        com.Parameters.AddWithValue("@POW_NAME",
                            (powiat == null ? DBNull.Value : powiat));
                        com.Parameters.AddWithValue("@GMI_NAME",
                            (gmina == null ? DBNull.Value : gmina));
                        com.Parameters.AddWithValue("@CITY_NAME",
                            city);
                        com.Parameters.AddWithValue("@DZIELNICA_NAME",
                            (dzielnica == null ? DBNull.Value : dzielnica));
                        com.Parameters.AddWithValue("@STREET_NAME",
                            (street == null ? DBNull.Value : street));

                        await using var reader = await com.ExecuteReaderAsync(cancellation);
                        int divisionId = -1;
                        int? streetId = -1;

                        while (await reader.ReadAsync(cancellation))
                        {
                            divisionId = (int)reader["DIVISON_ID"];
                            streetId = reader["STREET_ID"] == DBNull.Value ?
                                (int?)null : (int)reader["STREET_ID"];
                        }
                        reader.Close();
                        return (divisionId, streetId);
                    }
                }
            }
            catch (System.Exception ex)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"wojewodztwo: {wojewodztwo}");
                builder.AppendLine($"powiat: {powiat}");
                builder.AppendLine($"gmina: {gmina}");
                builder.AppendLine($"city: {city}");
                builder.AppendLine($"dzielnica: {dzielnica}");
                builder.AppendLine($"street: {street}");

                throw HandleException(ex, builder.ToString());
            }
        }

        public async Task<(int? WojId, IEnumerable<int> DivisionIds)> GetChildDivisionIdsAsync(
            string wojewodztwo,
            string? divisionName,
            CancellationToken cancellation)
        {
            try
            {
                await using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync(cancellation);

                    await using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;

                        com.CommandText = "DIVISION_IDS_SELECTOR";
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@WOJ_NAME", wojewodztwo);
                        com.Parameters.AddWithValue("@DIVISION_NAME",
                            (string.IsNullOrWhiteSpace(divisionName) ? DBNull.Value : divisionName));

                        await using var reader = await com.ExecuteReaderAsync(cancellation);
                        int? wojewodztwoId = null;
                        int? divisionId = null;
                        var ids = new List<int>();

                        while (await reader.ReadAsync(cancellation))
                        {
                            if (!wojewodztwoId.HasValue)
                            {
                                wojewodztwoId = reader["WOJ_ID"] == DBNull.Value ? null : (int)reader["WOJ_ID"];
                            }

                            divisionId = reader["DIV_ID"] == DBNull.Value ? null : (int)reader["DIV_ID"];
                            if (divisionId.HasValue)
                            {
                                ids.Add(divisionId.Value);
                            }
                        }
                        reader.Close();
                        return (wojewodztwoId, ids);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, $"WojewodztwoId: {wojewodztwo},\r\nDivisionName: {divisionName}");
            }
        }
        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods

        private System.Exception HandleException(System.Exception ex, string? inputData = null)
        {
            if (ex is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                switch (number)
                {
                    case 50009:
                        throw new IntershipException(
                            $"{Messages.Intership_Query_Paramiter_NotFound}: {inputData}",
                            Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
                    case 50010:
                        throw new AddressException(
                            $"{Messages.Procedure_FIND_Division_Street}: {message}\n {inputData}",
                            Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
                };
            }

            return new SqlClientException($"{ex.Message}: {inputData}");
        }

        private string? AdaptString(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;



        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Past Methods

        /*
                public async Task<IEnumerable<(int DivisionId, Street Street)>> GetCollocationsAsync
                    (
                    string divisionName,
                    string streetName,
                    CancellationToken cancellation
                    )
                {
                    try
                    {
                        await using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await using (SqlCommand com = new SqlCommand())
                            {
                                var collocation = new List<(int DivisionId, Street Street)>();

                                com.Connection = con;

                                com.CommandText = "SelectByStreetNameAndDivisionName";
                                com.CommandType = CommandType.StoredProcedure;

                                com.Parameters.AddWithValue("@DivisionName", divisionName);
                                com.Parameters.AddWithValue("@StreetName", streetName);

                                await con.OpenAsync();
                                await using var reader = await com.ExecuteReaderAsync(cancellation);

                                while (await reader.ReadAsync(cancellation))
                                {
                                    if (string.IsNullOrWhiteSpace(reader["StreetAdministrativeTypeId"].ToString()))
                                    {
                                        collocation.Add((
                                            (int)reader["AdministrativeDivisionId"],
                                            new Street
                                            {
                                                Id = (int)reader["StreetId"],
                                                Name = (string)reader["StreetName"],
                                                AdministrativeTypeId = null,
                                            }
                                        ));
                                    }
                                    else
                                    {
                                        collocation.Add((
                                            (int)reader["AdministrativeDivisionId"],
                                            new Street
                                            {
                                                Id = (int)reader["StreetId"],
                                                Name = (string)reader["StreetName"],
                                                AdministrativeTypeId = (int)reader["StreetAdministrativeTypeId"],
                                                AdministrativeType = new AdministrativeType
                                                {
                                                    Id = (int)reader["StreetAdministrativeTypeId"],
                                                    Name = (string)reader["StreetAdministrativeTypeName"],
                                                },
                                            }
                                        ));
                                    }
                                }
                                reader.Close();
                                return collocation;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw HandleException(ex, $"DivisionName: {divisionName}, StreetName: {streetName}");
                    }
                }

                public async Task<Dictionary<DivisionId, AdministrativeDivision>> GetDivisionsHierachyUpAsync
                    (
                    int divisionId,
                    CancellationToken cancellation
                    )
                {
                    try
                    {
                        await using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await con.OpenAsync(cancellation);

                            await using (SqlCommand com = new SqlCommand())
                            {
                                var dictionary = new Dictionary<DivisionId, AdministrativeDivision>();

                                com.Connection = con;

                                com.CommandText = "SelectAdministrativeDivisionUp";
                                com.CommandType = CommandType.StoredProcedure;

                                com.Parameters.AddWithValue("@DivisionId", divisionId);

                                await using var reader = await com.ExecuteReaderAsync(cancellation);

                                while (await reader.ReadAsync(cancellation))
                                {
                                    var databaseDivision = new AdministrativeDivision
                                    {
                                        Id = (int)reader["Id"],
                                        Name = (string)reader["AdministrativeDivisionName"],
                                        ParentDivisionId = string.IsNullOrWhiteSpace(reader["ParentDivisionId"].ToString()) ?
                                            null : (int)reader["ParentDivisionId"],
                                        AdministrativeTypeId = (int)reader["AdministrativeTypeId"],
                                        AdministrativeType = new AdministrativeType
                                        {
                                            Id = (int)reader["AdministrativeTypeId"],
                                            Name = (string)reader["AdministrativeTypeName"],
                                        }
                                    };
                                    var id = new DivisionId(databaseDivision.Id);
                                    dictionary[id] = databaseDivision;
                                }
                                reader.Close();
                                return dictionary;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw HandleException(ex, $"DivisionId: {divisionId}");
                    }
                }

                public async Task<(int TotalCount, IEnumerable<Guid> Ids)> GetBranchIdsSorted
                    (
                    Guid companyId,
                    int? divisionId,
                    int? streetId,
                    int maxItems,
                    int page,
                    bool ascending,
                    CancellationToken cancellation
                    )
                {
                    divisionId = divisionId.HasValue ? divisionId.Value : -1;
                    streetId = streetId.HasValue ? streetId.Value : -1;

                    var list = new List<Guid>();
                    var TotalCount = -1;
                    try
                    {
                        await using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await con.OpenAsync(cancellation);

                            await using (SqlCommand com = new SqlCommand())
                            {
                                com.Connection = con;

                                com.CommandText = "BranchesSorted";
                                com.CommandType = CommandType.StoredProcedure;

                                com.Parameters.AddWithValue("@CompanyId", companyId.ToString());
                                com.Parameters.AddWithValue("@DivisionId", divisionId);
                                com.Parameters.AddWithValue("@StreetId", streetId);
                                com.Parameters.AddWithValue("@MaxItems", maxItems);
                                com.Parameters.AddWithValue("@Page", page);
                                com.Parameters.AddWithValue("@Ascending", (ascending ? 1 : 0));

                                await using var reader = await com.ExecuteReaderAsync(cancellation);
                                Guid id;

                                while (await reader.ReadAsync(cancellation))
                                {
                                    id = (Guid)reader["BranchId"];
                                    if (TotalCount < 0)
                                    {
                                        TotalCount = (int)reader["TotalRecords"];
                                    }
                                    list.Add(id);
                                }
                                reader.Close();
                                return (TotalCount, list);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw HandleException(ex, $"CompanyId: {companyId}");
                    }
                }


                public async Task<IEnumerable<int>> GetDivisionIdsDownAsync
                    (
                    int divisionId,
                    CancellationToken cancellation
                    )
                {
                    var list = new List<int>();
                    try
                    {
                        await using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await con.OpenAsync(cancellation);

                            await using (SqlCommand com = new SqlCommand())
                            {
                                com.Connection = con;

                                com.CommandText = "SelectDivisionIdsWithStreets";
                                com.CommandType = CommandType.StoredProcedure;

                                com.Parameters.AddWithValue("@DivisionId", divisionId);

                                await using var reader = await com.ExecuteReaderAsync(cancellation);
                                int id;

                                while (await reader.ReadAsync(cancellation))
                                {
                                    id = (int)reader["AdministrativeDivisionId"];
                                    list.Add(id);
                                }
                                reader.Close();
                                return (list);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw HandleException(ex, $"DivisionId: {divisionId}");
                    }
                }*/
    }
}
