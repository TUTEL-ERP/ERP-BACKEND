using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly ILogger<ProvinceService> _logger;

        public ProvinceService(IProvinceRepository provinceRepository, ILogger<ProvinceService> logger)
        {
            _provinceRepository = provinceRepository;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string usrname, ProvinceRequestDto request)
        {
            return await _provinceRepository.ExecuteProcedureAsync(usrname, ProvinceAction.GRIDDATA, request);
        }

        public async Task<DataTable> InsertRecordAsync(string usrname, ProvinceRequestDto request)
        {
            return await _provinceRepository.ExecuteProcedureAsync(usrname, ProvinceAction.INSERT, request);
        }

        public async Task<DataTable> UpdateRecordAsync(string usrname, ProvinceRequestDto request)
        {
            return await _provinceRepository.ExecuteProcedureAsync(usrname, ProvinceAction.UPDATE, request);
        }

        public async Task<DataTable> DeleteRecordAsync(string usrname, ProvinceRequestDto request)
        {
            return await _provinceRepository.ExecuteProcedureAsync(usrname, ProvinceAction.DELETE, request);
        }

        public async Task<DataTable> SelectRecordAsync(string usrname, ProvinceRequestDto request)
        {
            return await _provinceRepository.ExecuteProcedureAsync(usrname, ProvinceAction.SINGELRECORD, request);
        }
    }
}