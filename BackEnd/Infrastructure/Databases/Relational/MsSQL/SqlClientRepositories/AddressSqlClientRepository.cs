using Application.Databases.Relational.Models;
using Application.Features.Addresses.Interfaces;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Address.ValueObjects.Identificators;
using Infrastructure.Exceptions.AppExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Databases.Relational.MsSQL.SqlClientRepositories
{
    public class AddressSqlClientRepository : IAddressSqlClientRepository
    {
        //Values
        private readonly string _connectionString;
        private readonly IExceptionsRepository _exceptionsRepository;


        //Constructor
        public AddressSqlClientRepository
            (
            IConfiguration configuration,
            IExceptionsRepository exceptionsRepository
            )
        {
            _exceptionsRepository = exceptionsRepository;
            _connectionString = configuration.GetSection("ConnectionStrings")["DbString"] ??
                throw new InfrastructureLayerException(Messages.NotConfiguredConnectionString);
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
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
                throw _exceptionsRepository.ConvertSqlClientDbException
                    (
                    ex,
                    $"DivisionName: {divisionName}, StreetName: {streetName}"
                    );
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
                throw _exceptionsRepository.ConvertSqlClientDbException
                    (
                    ex,
                    $"DivisionId: {divisionId}"
                    );
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
    }
}
