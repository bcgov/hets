using AutoMapper;
using HetsData.Dtos;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetsData.Repositories
{
    public interface IUserRepository
    {
        List<UserDto> GetRecords();
        UserDto GetRecord(int id, bool excludeInactiveRoles = false);
    }
    public class UserRepository : IUserRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;

        public UserRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<UserDto> GetRecords()
        {
            List<HetUser> users = _dbContext.HetUsers.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.HetUserDistricts)
                .Include(x => x.HetUserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.HetRolePermissions)
                            .ThenInclude(z => z.Permission)
                .ToList();

            foreach (HetUser user in users)
            {
                // remove inactive roles
                user.HetUserRoles = user.HetUserRoles
                    .Where(x => x.Role != null &&
                                x.EffectiveDate <= DateTime.UtcNow &&
                                (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                    .ToList();
            }

            return _mapper.Map<List<UserDto>>(users);
        }

        /// <summary>
        /// Get a User record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public UserDto GetRecord(int id, bool excludeInactiveRoles = false)
        {
            HetUser user = _dbContext.HetUsers.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.HetUserDistricts)
                .Include(x => x.HetUserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.HetRolePermissions)
                            .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.UserId == id);

            if (user != null && excludeInactiveRoles)
            {
                // remove inactive roles
                user.HetUserRoles = user.HetUserRoles
                    .Where(x => x.Role != null &&
                                x.EffectiveDate <= DateTime.UtcNow &&
                                (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                    .ToList();

            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
