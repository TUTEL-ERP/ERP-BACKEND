// Repository/CountryRepository.cs
using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Enity;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

namespace server.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
        {
            return await _dbSet
                .Select(c => new CountryDto
                {
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    CountryCode = c.CountryCode,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .OrderBy(c => c.CountryName)
                .ToListAsync();
        }

        public async Task<CountryDto?> GetCountryByIdAsync(int countryId)
        {
            return await _dbSet
                .Where(c => c.CountryId == countryId)
                .Select(c => new CountryDto
                {
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    CountryCode = c.CountryCode,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CountryDto>> GetActiveCountriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .Select(c => new CountryDto
                {
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    CountryCode = c.CountryCode,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .OrderBy(c => c.CountryName)
                .ToListAsync();
        }



        // Repository/CountryRepository.cs
        public async Task<List<dynamic>> GetGridDataAsync(string formName)
        {
            var dummyRequest = new CountryRequestDto { CountryId = 0 };

            // ✅ Call the SP with ActionType.Select (1)
            var result = await ExecuteSpAsync(ActionType.Select, dummyRequest, formName);

            return result as List<dynamic> ?? new List<dynamic>();
        }

        public async Task<dynamic> GetRecordByIdAsync(string formName, int id)
        {
            var request = new CountryRequestDto { CountryId = id };

            // ✅ Call the SP with ActionType.SelectById (2)
            var result = await ExecuteSpAsync(ActionType.SelectById, request, formName);

            // Result will be a List<dynamic> with one item, so return the first item
            var list = result as List<dynamic>;
            return list?.FirstOrDefault();
        }



        public async Task<bool> IsCountryExistsAsync(string countryName, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.CountryName == countryName);
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.CountryId != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task<CountryDto> CreateCountryAsync(CountryRequestDto countryDto)
        {
            var country = new Country
            {
                CountryName = countryDto.CountryName,
                CountryCode = countryDto.CountryCode,
                IsActive = countryDto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            await AddAsync(country);
            await SaveChangesAsync();

            return new CountryDto
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
                CountryCode = country.CountryCode,
                IsActive = country.IsActive,
                CreatedDate = country.CreatedDate
            };
        }

        public async Task<CountryDto?> UpdateCountryAsync(CountryRequestDto countryDto)
        {
            var country = await _dbSet.FindAsync(countryDto.CountryId);
            if (country == null) return null;

            country.CountryName = countryDto.CountryName;
            country.CountryCode = countryDto.CountryCode;
            country.IsActive = countryDto.IsActive;

            await UpdateAsync(country);
            await SaveChangesAsync();

            return await GetCountryByIdAsync(country.CountryId);
        }

        public async Task<bool> DeleteCountryAsync(int countryId)
        {
            return await DeleteAsync(countryId);
        }



        public async Task<dynamic> ExecuteSpAsync(ActionType action, CountryRequestDto request, string formName = "Country")
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_DYNAMIC_GRID", connection); 
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FormName", formName);

            cmd.Parameters.AddWithValue("@ActionType", (int)action);

            cmd.Parameters.AddWithValue("@RecordId", request.CountryId);

            cmd.Parameters.AddWithValue("@CountryId", request.CountryId);
            cmd.Parameters.AddWithValue("@CountryName", (object?)request.CountryName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryCode", (object?)request.CountryCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@ExcludeId", (object?)request.ExcludeId ?? DBNull.Value);

            // Output parameters (For Insert/Update scenarios)
            var outputParam = new SqlParameter("@OutputData", SqlDbType.NVarChar, -1)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            var statusParam = new SqlParameter("@Status", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusParam);

            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            await connection.OpenAsync();

            switch (action)
            {
                case ActionType.Select:
                case ActionType.SelectActive:
                case ActionType.SelectById:
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var result = new List<dynamic>();
                        while (await reader.ReadAsync())
                        {
                            // ✅ Generic dynamic object creation from the SP result
                            var row = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));
                            }
                            result.Add(row);
                        }
                        return result;
                    }

                case ActionType.CheckExists:
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToBoolean(outputParam.Value);

                default:
                    await cmd.ExecuteNonQueryAsync();
                    return new CountryResponseDto
                    {
                        CountryId = Convert.ToInt32(outputParam.Value),
                        Status = statusParam.Value?.ToString(),
                        Message = messageParam.Value?.ToString()
                    };
            }
        }
    }
}