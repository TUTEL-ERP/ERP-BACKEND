using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryService> _logger;

        public CountryService(ICountryRepository countryRepository, ILogger<CountryService> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        // ✅ Removed: GetAllAsync, GetByIdAsync, GetActiveAsync, CreateAsync, UpdateAsync, DeleteAsync, CheckExistsAsync

        // ✅ ONLY THESE REMAIN:
        public async Task<DataTable> GetGridDataAsync(string usrname, CountryRequestDto request)
        {
            return await _countryRepository.ExecuteProcedureAsync(usrname, CountryAction.GRIDDATA, request);
        }

        public async Task<DataTable> InsertRecordAsync(string usrname, CountryRequestDto request)
        {
            return await _countryRepository.ExecuteProcedureAsync(usrname, CountryAction.INSERT, request);
        }

        public async Task<DataTable> UpdateRecordAsync(string usrname, CountryRequestDto request)
        {
            return await _countryRepository.ExecuteProcedureAsync(usrname, CountryAction.UPDATE, request);
        }

        public async Task<DataTable> DeleteRecordAsync(string usrname, CountryRequestDto request)
        {
            return await _countryRepository.ExecuteProcedureAsync(usrname, CountryAction.DELETE, request);
        }

        public async Task<DataTable> SelectRecordAsync(string usrname, CountryRequestDto request)
        {
            return await _countryRepository.ExecuteProcedureAsync(usrname, CountryAction.SINGELRECORD, request);
        }
    }
}