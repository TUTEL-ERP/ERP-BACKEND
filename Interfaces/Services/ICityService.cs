using server.Dto;

namespace server.Interfaces.Services
{
    public interface ICityService
    {
        Task<List<CityDto>> GetGridDataAsync(
            string usrname,
            CityRequestDto request);

        Task<List<CityDto>> InsertRecordAsync(
            string usrname,
            CityRequestDto request);

        Task<List<CityDto>> UpdateRecordAsync(
            string usrname,
            CityRequestDto request);

        Task<List<CityDto>> DeleteRecordAsync(
            string usrname,
            CityRequestDto request);

        Task<List<CityDto>> SelectRecordAsync(
            string usrname,
            CityRequestDto request);
    }
}