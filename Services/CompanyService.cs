using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ICompanyRepository companyRepository, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string usrname, CompanyRequestDto request)
            => await _companyRepository.ExecuteProcedureAsync(usrname, CompanyAction.GRIDDATA, request);

        public async Task<DataTable> InsertRecordAsync(string usrname, CompanyRequestDto request)
            => await _companyRepository.ExecuteProcedureAsync(usrname, CompanyAction.INSERT, request);

        public async Task<DataTable> UpdateRecordAsync(string usrname, CompanyRequestDto request)
            => await _companyRepository.ExecuteProcedureAsync(usrname, CompanyAction.UPDATE, request);

        public async Task<DataTable> DeleteRecordAsync(string usrname, CompanyRequestDto request)
            => await _companyRepository.ExecuteProcedureAsync(usrname, CompanyAction.DELETE, request);

        public async Task<DataTable> SelectRecordAsync(string usrname, CompanyRequestDto request)
            => await _companyRepository.ExecuteProcedureAsync(usrname, CompanyAction.SINGELRECORD, request);
    }
}