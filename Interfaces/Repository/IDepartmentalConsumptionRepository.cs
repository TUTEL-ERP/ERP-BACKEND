using server.Dto;
using server.Enums;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IDepartmentalConsumptionRepository
    {
        Task<DataSet> ExecuteProcedureAsync(
            string usrname,
            DepartmentalConsumptionAction action,
            DepartmentalConsumptionDto request);
    }
}
