using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using BceidService;
using Microsoft.Extensions.Logging;

namespace HetsBceid
{
    public interface IBceidApi
    {
        Task<(string error, BceidAccount account)> GetBceidAccountCachedAsync(Guid? userGuid, string username, string userType, Guid requestorGuid, string requestorType);
    }

    public class BceidApi : IBceidApi
    {
        private readonly BCeIDServiceSoapClient _client;
        private readonly ILogger<BceidApi> _logger;
        private readonly Dictionary<string, BceidAccount> _accountCache; //no need for ConcurrentDictionary
        private readonly System.Timers.Timer _timer;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public BceidApi(BCeIDServiceSoapClient client, ILogger<BceidApi> logger)
        {
            _client = client;
            _logger = logger;
            _accountCache = new Dictionary<string, BceidAccount>();
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(RefreshCache);
            _timer.Interval = TimeSpan.FromMinutes(_client.CacheLifespan).TotalMilliseconds;
            _timer.Enabled = true;
        }

        private void RefreshCache(object source, ElapsedEventArgs e)
        {
            _logger.LogInformation($"BCeID Cache clean up: {_accountCache.Keys.Count} entries.");
            _accountCache.Clear();
        }

        public async Task<(string error, BceidAccount account)> GetBceidAccountCachedAsync(Guid? userGuid, string username, string userType, Guid requestorGuid, string requestorType)
        {
            //to minimize the BCeID web service calls - may have a performance issue when multiple fresh users log in at the same time.            
            await _semaphore.WaitAsync();

            try
            {
                var key = username + "||" + userType;
                if (_accountCache.ContainsKey(key))
                {
                    _logger.LogInformation($"BCeID cache hit: {key}");
                    return ("", _accountCache[key]);
                }

                var (error, account) = await GetBceidAccountAsync(userGuid, username, userType, requestorGuid.ToString("N"), requestorType);

                if (account != null)
                {
                    _logger.LogInformation($"BCeID new key: {key}");
                    _accountCache[key] = account;
                }

                if (account != null && string.IsNullOrEmpty(account.Username)) account.Username = username;

                return (error, account);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<(string error, BceidAccount account)> GetBceidAccountAsync(Guid? userGuid, string username, string userType, string requestorGuid, string requestorType)
        {
            var targetTypeCode = userType.IsIdirUser() ? BCeIDAccountTypeCode.Internal : BCeIDAccountTypeCode.Business;
            var requesterTypeCode = requestorType.IsIdirUser() ? BCeIDAccountTypeCode.Internal : BCeIDAccountTypeCode.Business;

            var request = new AccountDetailRequest();
            request.requesterAccountTypeCode = requesterTypeCode;
            request.requesterUserGuid = requestorGuid;
            request.accountTypeCode = targetTypeCode;

            //ISA - for IDIR, only IDIR search is allowed
            if (userType.IsIdirUser())
            {
                request.userId = username;
            }
            else if (userGuid != null)
            {
                request.userGuid = userGuid?.ToString("N");
            }
            else
            {
                request.userId = username;
            }

            request.onlineServiceId = _client.Osid;

            var response = await _client.getAccountDetailAsync(request);

            if (response.code != ResponseCode.Success)
            {
                return (response.message, null);
            }
            else if (response.failureCode == FailureCode.NoResults)
            {
                return ("", null);
            }

            var account = new BceidAccount();

            account.Username = response.account.userId.value;
            account.UserGuid = userGuid ?? new Guid(response.account.guid.value);
            account.UserType = userType;

            if (account.UserType.IsBusinessUser())
            {
                account.BusinessGuid = new Guid(response.account.business.guid.value);
                account.BusinessLegalName = response.account.business.legalName.value;

                var doingBusinessAs = response.account.business.doingBusinessAs.value;
                account.DoingBusinessAs = doingBusinessAs.IsEmpty() ? account.BusinessLegalName : doingBusinessAs;

                var businessNumber = response.account.business.businessNumber.value;
                account.BusinessNumber = businessNumber.IsEmpty() ? 0 : Convert.ToDecimal(businessNumber);
            }

            account.DisplayName = response.account.displayName.value;
            account.FirstName = response.account.individualIdentity.name.firstname.value;
            account.LastName = response.account.individualIdentity.name.surname.value;
            account.Email = response.account.contact.email.value;

            return ("", account);
        }

    }
}
