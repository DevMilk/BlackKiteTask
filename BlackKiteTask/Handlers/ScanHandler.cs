using BlackKiteTask.Common;
using BlackKiteTask.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackKiteTask.Requests.Company;
using BlackKiteTask.Responses.Company;
using BlackKiteTask.Common.Infrastructure;
using static BlackKiteTask.Domain.Enums;
namespace BlackKiteTask.Handlers
{
    public class ScanHandler : IScanHandler
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<ScanHandler> _logger;
        private readonly int _retryOnExceptionCount;
        private readonly int _retryOnUnknownStatusCount;
        private readonly float _retryDelayInSeconds;
        private readonly float _pollPeriodInSeconds;
        private readonly float _overallTimeoutInSeconds;
        public ScanHandler(ICompanyService companyService, ILogger<ScanHandler> logger, IConfiguration config)
        {
            _companyService = companyService;
            _logger = logger;
            _retryOnExceptionCount = config.GetSection("PollConfig").GetValue<int?>("RetryOnExceptionCount") ?? 3;
            _retryOnUnknownStatusCount = config.GetSection("PollConfig").GetValue<int?>("RetryOnUnknownStatusCount") ?? 3;
            _retryDelayInSeconds = config.GetSection("PollConfig").GetValue<float?>("RetryDelayInSeconds") ?? 8f;
            _pollPeriodInSeconds = config.GetSection("PollConfig").GetValue<float?>("PollPeriodInSeconds") ?? 5f;
            _overallTimeoutInSeconds = config.GetSection("PollConfig").GetValue<float?>("OverallTimeoutInSeconds") ?? 0;

        }

        public async Task Scan(string companyDomain)
        {
            var startDate = DateTime.Now;
            _logger.LogInformation($"Attempting to start scan for {companyDomain} ...");
            var postCompanyResp = await _companyService.PostCompany(new PostCompaniesRequest
            {
                MainDomainValue = companyDomain,
                EcosystemId = 16420
            });
            _logger.LogInformation($"Scan Started. CompanyId: {postCompanyResp.CompanyId} ...");
            await StartPolling(postCompanyResp.CompanyId);
            _logger.LogInformation("Scan Completed. Elapsed Time: {ElapsedTime} Seconds", (DateTime.Now - startDate).TotalSeconds);


        }
        /*
         The status values starting with the "Preliminary" keyword mean, this is the first time Black Kite scans the related company. 
        This phase shouldn't really take more than 5-10 minutes. 
        
        The ones with "Extended Scan" prefix mean, this is the first time Black Kite executes a comprehensive scan on the related company. 
        This is the phase where extensive technical issues will be extracted. 
        
        The ones with "Extended Rescan" prefix mean, this is one of the many continuous scans that Black Kite executes against the target company. 
        In summary, when the status value contains both the words "Extended" and "Ready", that means the results are ready for processing.

        Beware that this property is only related to the technical rating part of the three dimensional Black Kite rating system. 
        So, the value of ScanStatus property doesn’t tell you whether the Compliance or FinancialImpact property values are calculated yet or not
        */
        private async Task StartPolling(int companyId)
        {
            int retryCounterOnException = _retryOnExceptionCount;
            int retryCounterOnUnknownStatus = _retryOnUnknownStatusCount;
            var timeoutDate = DateTime.Now.AddSeconds(1000 * (_overallTimeoutInSeconds == 0 ? int.MaxValue : _overallTimeoutInSeconds));
            bool? isSuccess = null;
            GetCompaniesResponse getCompanyResp = null;

            //Continue until getting a result or timeout
            do
            {
                try
                {
                    _logger.LogInformation("Getting scan status...");

                    getCompanyResp = await _companyService.GetCompany(new GetCompaniesRequest { Id = companyId });
                    var scanStatusAsEnum = Enum.Parse<ScanStatus>(getCompanyResp.ScanStatus.Replace(" ",""));

                    switch (scanStatusAsEnum)
                    {
                        //Failed
                        case ScanStatus.PreliminaryScanFailed:
                        case ScanStatus.ExtendedScanFailed:
                        case ScanStatus.ExtendedRescanFailed:
                            isSuccess = false;
                            break;

                        //Success
                        case ScanStatus.ExtendedResultsReady:
                        case ScanStatus.ExtendedRescanResultsReady:
                            isSuccess = true;
                            break;

                        //Unknown
                        case ScanStatus.UnknownScanStatus:
                            retryCounterOnUnknownStatus--;
                            break;

                        //Continues
                        default:
                            retryCounterOnUnknownStatus = _retryOnUnknownStatusCount;
                            break;
                    }

                    _logger.LogInformation("Current Scan Status: {ScanStatus}", getCompanyResp.ScanStatus);

                    //Wait to poll
                    await Task.Delay((int)(_pollPeriodInSeconds * 1000f));
                    //Reset retry count if there is no exception
                    retryCounterOnException = _retryOnExceptionCount; 

                } catch(HttpRequestException exc)
                {
                    
                    if (retryCounterOnException == 0)
                        throw;

                    retryCounterOnException--;
                    await Task.Delay((int)(_retryDelayInSeconds * 1000f)); //Wait to retry
                    _logger.LogErrorFormated(exc);
                    _logger.LogInformation("Failed to receive scan results, remaining retry: {retryCountOnException}", retryCounterOnException);
                }
                

            } while(!isSuccess.HasValue && DateTime.Now < timeoutDate); 
            

            if(retryCounterOnUnknownStatus == 0)
            {
                _logger.LogError("Failed after getting scan status as unknown consecutively {retryCounterOnUnknownStatus} times", _retryOnUnknownStatusCount);
                return;
            }

            if(isSuccess.HasValue && isSuccess.Value == false)
            {
                _logger.LogError("Scan Failed : {ScanStatus}", getCompanyResp.ScanStatus);
                return;
            }

            if(DateTime.Now >= timeoutDate)
            {
                _logger.LogError("Timeout Expired");
                return;
            }

            //If success (Extended + Ready) then export results to results file
            string fileName = $"results-{companyId}";
            _logger.LogInformation("Scan results received, exporting results to {fileName}.json file...", fileName);
            Utils.ExportAsJson(fileName, getCompanyResp);
            _logger.LogInformation("Scan results exported.");
        }
    }
}
