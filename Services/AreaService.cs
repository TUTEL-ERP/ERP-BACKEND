using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using System.Data;

namespace server.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly ILogger<AreaService> _logger;

        public AreaService(IAreaRepository areaRepository, ILogger<AreaService> logger)
        {
            _areaRepository = areaRepository;
            _logger = logger;
        }

        public async Task<DataTable> GetGridDataAsync(string usrname, AreaRequestDto request)
        {
            return await _areaRepository.ExecuteProcedureAsync(usrname, AreaAction.GRIDDATA, request);
        }

        public async Task<DataTable> InsertRecordAsync(string usrname, AreaRequestDto request)
        {
            return await _areaRepository.ExecuteProcedureAsync(usrname, AreaAction.INSERT, request);
        }

        public async Task<DataTable> UpdateRecordAsync(string usrname, AreaRequestDto request)
        {
            return await _areaRepository.ExecuteProcedureAsync(usrname, AreaAction.UPDATE, request);
        }

        public async Task<DataTable> DeleteRecordAsync(string usrname, AreaRequestDto request)
        {
            return await _areaRepository.ExecuteProcedureAsync(usrname, AreaAction.DELETE, request);
        }

        public async Task<DataTable> SelectRecordAsync(string usrname, AreaRequestDto request)
        {
            return await _areaRepository.ExecuteProcedureAsync(usrname, AreaAction.SINGELRECORD, request);
        }
    }
}