using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetsData.Repositories
{
    public interface IRentalRequestRepository
    {
        RentalRequestLite ToLiteModel(HetRentalRequest request);
    }

    public class RentalRequestRepository : IRentalRequestRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;

        public RentalRequestRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Convert to Rental Request Lite Model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RentalRequestLite ToLiteModel(HetRentalRequest request)
        {
            RentalRequestLite requestLite = new RentalRequestLite();

            if (request != null)
            {
                requestLite.YesCount = RentalRequestHelper.CalculateYesCount(request);

                if (request.DistrictEquipmentType != null)
                {
                    requestLite.EquipmentTypeName = request.DistrictEquipmentType.EquipmentType.Name;
                    requestLite.DistrictEquipmentName = request.DistrictEquipmentType.DistrictEquipmentName;
                }

                requestLite.Id = request.RentalRequestId;
                requestLite.LocalArea = _mapper.Map<LocalAreaDto>(request.LocalArea);

                if (request.Project != null)
                {
                    requestLite.PrimaryContact = request.Project.PrimaryContact;
                    requestLite.ProjectName = request.Project.Name;
                    requestLite.ProjectId = request.Project.ProjectId;
                }
                else
                {
                    requestLite.ProjectName = "Request - View Only";
                }

                requestLite.Status = request.RentalRequestStatusType.Description;
                requestLite.EquipmentCount = request.EquipmentCount;
                requestLite.ExpectedEndDate = request.ExpectedEndDate;
                requestLite.ExpectedStartDate = request.ExpectedStartDate;
            }

            return requestLite;
        }
    }
}
