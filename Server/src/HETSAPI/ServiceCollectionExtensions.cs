using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using HETSAPI.Controllers;
using HETSAPI.Services;
using HETSAPI.Services.Impl;

namespace HETSAPI
{
    /// <summary>
    /// Utility extension added to aspnet core to facilitate registration of application-specific services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application-specific services to the dependency injection container in aspnet core.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IAttachmentService, AttachmentService>();
            services.AddTransient<IAttachmentUploadService, AttachmentUploadService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IConditionTypeService, ConditionTypeService>();
            services.AddTransient<IContactService, ContactService>();            
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IDistrictEquipmentTypeService, DistrictEquipmentTypeService>();
            services.AddTransient<IDistrictService, DistrictService>();            
            services.AddTransient<IDumpTruckService, DumpTruckService>();                        
            services.AddTransient<IEquipmentAttachmentService, EquipmentAttachmentService>();
            services.AddTransient<IEquipmentService, EquipmentService>();
            services.AddTransient<IEquipmentTypeService, EquipmentTypeService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<ILocalAreaService, LocalAreaService>();
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IOwnerService, OwnerService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProvincialRateTypeService, ProvincialRateTypeService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<IRentalAgreementConditionService, RentalAgreementConditionService>();
            services.AddTransient<IRentalAgreementRateService, RentalAgreementRateService>();
            services.AddTransient<IRentalAgreementService, RentalAgreementService>();
            services.AddTransient<IRentalRequestAttachmentService, RentalRequestAttachmentService>();
            services.AddTransient<IRentalRequestRotationListService, RentalRequestRotationListService>();
            services.AddTransient<IRentalRequestService, RentalRequestService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IServiceAreaService, ServiceAreaService>();
            services.AddTransient<ISeniorityAuditService, SeniorityAuditService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ITimeRecordService, TimeRecordService>();
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
