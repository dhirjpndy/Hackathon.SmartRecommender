using ConsumerMarketplace.AdminFlexTool.Data.Factories;
using Hackathon.SmartRecommender.Domain.Models;
using Hackathon.SmartRecommender.Domain.Models.SubscriberSettings;
using Hackathon.SmartRecommender.Domain.Providers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumerMarketplace.AdminFlexTool.Reports.Data.Extensions;
using ConsumerMarketplace.AdminFlexTool.Data;

namespace Hackathon.SmartRecommender.Providers.Implementations
{
    public class DashboardProvider : IDashboardProvider
    {
        public DashboardProvider(IDataProviderFactory dataProviderFactory,
    IOptions<SubscriberConnectionStringSettings> subscriberConnectionStringSettings)
        {
            _dataProviderFactory = dataProviderFactory;
            _subscriberConnectionStringSettings = subscriberConnectionStringSettings.Value;
        }

        private readonly IDataProviderFactory _dataProviderFactory;
        private SubscriberConnectionStringSettings _subscriberConnectionStringSettings { get; set; }

        public async Task<List<BusinessDetails>> GetBusinessesDetails()
        {
            var parameters = new List<SqlParameter>();

            string sql = @" SELECT TOP(100) s.StudioID, s.StudioName
                            FROM Studios s 
                            WHERE Deleted = 0
                            ORDER BY StudioID DESC 
                          ";

            using (var dataProvider = _dataProviderFactory.CreateForConnectionStringName("SmartRecommender"))
            {
                var data = await dataProvider.ReadAsync(sql, MappingFunc, parameters);

                return data.ToList();
            }
        }

        public async Task<List<ClassSchedulingDetails>> GetClassDetails(int studioId, DateTime startDateTime, DateTime endDateTime)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(SqlParameterFactory.Create("@studioId", SqlDbType.Int, studioId));
            parameters.Add(SqlParameterFactory.Create("@startDate", SqlDbType.DateTime2, startDateTime));
            parameters.Add(SqlParameterFactory.Create("@endDate", SqlDbType.DateTime2, endDateTime));

            string sql = @" WITH CteTotalCapacity as
                            (
	                            SELECT DESCRIPTIONID, Sum(MaxCapacity) Capacity, StudioId
	                            from ClassScheduling
	                            WHERE MAXCAPACITY > 0 ANd ClassScheduling.ClassDate >= @startDate AND ClassScheduling.ClassDate <= @endDate
	                            AND StudioId = @studioId
	                            group by StudioId, DESCRIPTIONID
                            )
                            , CteTotalBookings as
                            (
                            SELECT COUNT(visitrefNo) visits, vd.DESCRIPTIONID, vd.StudioId
                            FROM Bookings vd  
                            where vd.ClassDate >= @startDate AND vd.ClassDate <= @endDate AND StudioId = @studioId
                            group by vd.DESCRIPTIONID, vd.StudioId
                            )

                            SELECT ctc.DESCRIPTIONID, ISNULL(ctb.visits, 0) visits, ctc.Capacity,
                            CASE 
                                WHEN ISNULL(ctb.visits, 0) = 0 THEN 100
                                WHEN ctb.visits = 0 THEN 100
                                WHEN ctb.visits IS NOT NULL THEN (ctb.visits * 100 / ctc.Capacity)
                            END as Score, ctc.StudioId
                            FROM CteTotalCapacity ctc
                            LEFT JOIN CteTotalBookings ctb ON ctc.DESCRIPTIONID = ctb.DESCRIPTIONID AND ctc.STUDIOID = ctb.STUDIOID 
                          ";

            using (var dataProvider = _dataProviderFactory.CreateForConnectionStringName("SmartRecommender"))
            {
                var data = await dataProvider.ReadAsync(sql, MappingScoreFunc, parameters);

                return data.ToList();
            }
        }

        public async Task<int> GetTotalScheduledClasses(int studioId, DateTime? startDateTime, DateTime? endDateTime)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(SqlParameterFactory.Create("@studioId", SqlDbType.Int, studioId));
            var dateTimeWhereClause = string.Empty;

            if (startDateTime.HasValue)
            {
                dateTimeWhereClause = $" AND tcs.ClassDate >= @startDate ";

                parameters.Add(SqlParameterFactory.Create("@startDate", SqlDbType.DateTime2, startDateTime.Value));
            }

            if (endDateTime.HasValue)
            {
                dateTimeWhereClause = $"{dateTimeWhereClause} AND tcs.ClassDate <= @endDate ";

                parameters.Add(SqlParameterFactory.Create("@endDate", SqlDbType.DateTime2, endDateTime.Value));
            }

            string sql = $@"SELECT COUNT(TotalClasses) TotalClass FROM
                            (
                             SELECT count(tcd.[Description Id]) TotalClasses, tcd.[Description Id], tcd.[Studio Id]
                             FROM serviceclassdescriptions tcd
                             INNER JOIN classscheduling tcs on tcs.DescriptionID = tcd.[Description Id] AND tcs.Studioid = tcd.[Studio Id]
                             WHERE tcd.[Studio Id] = @studioId {dateTimeWhereClause}
                             GROUP by tcd.[Description Id], tcd.[Studio Id]
                             ) as tab
                          ";

            using (var dataProvider = _dataProviderFactory.CreateForConnectionStringName("SmartRecommender"))
            {
                var data = await Task.FromResult(dataProvider.Read<int>(sql, parameters));

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Mappings the function.
        /// </summary>
        /// <returns></returns>
        private static BusinessDetails MappingFunc(IDataReader dataRow)
        {
            if (dataRow == null)
                return new BusinessDetails();

            var studioId = dataRow.GetValue<double>("StudioID");
            var studioName = dataRow.GetValue<string>("StudioName");
            var businessDescription = "";
            return new BusinessDetails { BusinessId = studioId, BusinessName = studioName, BusinessDescription = businessDescription };
        }

        /// <summary>
        /// Mappings the function.
        /// </summary>
        /// <returns></returns>
        private static ClassSchedulingDetails MappingScoreFunc(IDataReader dataRow)
        {
            if (dataRow == null)
                return new ClassSchedulingDetails();

            var studioId = dataRow.GetValue<double>("StudioID");
            var score = dataRow.GetValue<double>("Score");
            var descriptionId = dataRow.GetValue<double>("DESCRIPTIONID");
            var totalScheduling = dataRow.GetValue<double>("Capacity");
            var totalVisits = dataRow.GetValue<int>("visits");
            return new ClassSchedulingDetails { DescriptionId = descriptionId, Score = score, StudioID = studioId, TotalCapacity = totalScheduling, TotalVisits = totalVisits };
        }

    }
}
