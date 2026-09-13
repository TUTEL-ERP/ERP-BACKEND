using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<CityService> _logger;

        public CityService(
            ICityRepository cityRepository,
            ILogger<CityService> logger)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<List<CityDto>> GetGridDataAsync(
            string usrname,
            CityRequestDto request)
        {
            return await _cityRepository.ExecuteProcedureAsync(
                usrname,
                CityAction.GRIDDATA,
                request);
        }

        public async Task<List<CityDto>> InsertRecordAsync(
            string usrname,
            CityRequestDto request)
        {
            return await _cityRepository.ExecuteProcedureAsync(
                usrname,
                CityAction.INSERT,
                request);
        }

        public async Task<List<CityDto>> UpdateRecordAsync(
            string usrname,
            CityRequestDto request)
        {
            return await _cityRepository.ExecuteProcedureAsync(
                usrname,
                CityAction.UPDATE,
                request);
        }

        public async Task<List<CityDto>> DeleteRecordAsync(
            string usrname,
            CityRequestDto request)
        {
            return await _cityRepository.ExecuteProcedureAsync(
                usrname,
                CityAction.DELETE,
                request);
        }

        public async Task<List<CityDto>> SelectRecordAsync(
            string usrname,
            CityRequestDto request)
        {
            return await _cityRepository.ExecuteProcedureAsync(
                usrname,
                CityAction.SINGELRECORD,
                request);
        }
    }
}