using Application.VerticalSlice.AddressPart.DTOs.DatabaseDTOs;
using Domain.Providers;
using Infrastructure;
using Infrastructure.Exceptions.AppExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using System.Text;

namespace Application.VerticalSlice.AddressPart.Interfaces
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;
        private readonly IDomainProvider _provider;
        public AddressRepository
            (
            IConfiguration configuration,
            IDomainProvider provider
            )
        {
            _provider = provider;
            _connectionString = configuration.GetSection("ConnectionStrings")["DbString"]
                ?? throw new NotImplementedException("Imposible");
        }


        public async Task<ICollection<PartialAddressDto>> GetDivisionsStreetsAsync
            (
            string administrativeDivisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var divisionStreetList = new List<AdministrativeDivisionStreetDTO>();
            var list = new List<PartialAddressDto>();

            try
            {
                await using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;
                        //Part 1 

                        com.CommandText = "SelectByStreetNameAndDivisionName";
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@DivisionName", administrativeDivisionName);
                        com.Parameters.AddWithValue("@StreetName", streetName);

                        await con.OpenAsync();
                        await using var reader = await com.ExecuteReaderAsync(cancellation);

                        while (await reader.ReadAsync(cancellation))
                        {
                            divisionStreetList.Add(new AdministrativeDivisionStreetDTO
                            {
                                AdministrativeDivisionId = (int)reader["AdministrativeDivisionId"],
                                StreetId = (int)reader["StreetId"],
                                StreetName = (string)reader["StreetName"],
                                StreetAdministrativeTypeId =
                                (int)reader["StreetAdministrativeTypeId"],
                                StreetAdministrativeTypeName =
                                (string)reader["StreetAdministrativeTypeName"],
                            });
                        }
                        reader.Close();

                        if (!divisionStreetList.Any())
                        {
                            return list;
                        }

                        com.CommandText = "SelectAdministrativeDivisionUp";
                        com.CommandType = CommandType.StoredProcedure;

                        foreach (var item in divisionStreetList)
                        {
                            com.Parameters.Clear();
                            var hierarchy = new List<AdministrativeDivisionDto>();

                            var administrativeDivisionId = item.AdministrativeDivisionId;
                            com.Parameters.AddWithValue("@DivisionId", administrativeDivisionId);

                            await using var reader2 = await com.ExecuteReaderAsync(cancellation);

                            while (await reader2.ReadAsync(cancellation))
                            {
                                hierarchy.Add(new AdministrativeDivisionDto
                                {
                                    Id = (int)reader2["Id"],
                                    AdministrativeDivisionName =
                                    (string)reader2["AdministrativeDivisionName"],
                                    ParentDivisionId =
                                    string.IsNullOrWhiteSpace(
                                        reader2["ParentDivisionId"].ToString()
                                        ) ? null : (int)reader2["ParentDivisionId"],
                                    AdministrativeTypeId = (int)reader2["AdministrativeTypeId"],
                                    AdministrativeTypeName = (string)reader2["AdministrativeTypeName"],
                                });
                            }
                            reader2.Close();

                            list.Add(new PartialAddressDto
                            {
                                Hierarchy = hierarchy,
                                StreetId = item.StreetId,
                                StreetName = item.StreetName,
                                StreetAdministrativeTypeId = item.StreetAdministrativeTypeId,
                                StreetAdministrativeTypeName = item.StreetAdministrativeTypeName,
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new SqlClientImplementationException
                    (
                    _provider.GetExceptionsMessageProvider().GenerateExceptionMessage
                    (
                    Messages.IncorrectlementationBySqlClient,
                    this.GetType(),
                    MethodBase.GetCurrentMethod(),
                    $"{administrativeDivisionName}, {streetName}",
                    PrintSqlExceptionsErrors(ex)
                    )
                    );
            }
            catch (Exception ex)
            {
                throw new SqlClientImplementationException
                    (
                    _provider.GetExceptionsMessageProvider().GenerateExceptionMessage
                    (
                    Messages.IncorrectlementationBySqlClient,
                    this.GetType(),
                    MethodBase.GetCurrentMethod(),
                    $"{administrativeDivisionName}, {streetName}",
                    ex.ToString()
                    )
                    );
            }
            return list;
        }


        private string PrintSqlExceptionsErrors(SqlException ex)
        {
            /* https://learn.microsoft.com/pl-pl/dotnet/api/system.data.sqlclient.sqlexception?view=dotnet-plat-ext-8.0
                    */
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n");
            }
            return errorMessages.ToString();
        }
    }
}
