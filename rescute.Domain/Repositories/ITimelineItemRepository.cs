using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Domain.Repositories;

public interface ITimelineItemRepository : IRepository<TimelineItem>
{
    Task<IReadOnlyList<StatusReport>> GetStatusReportsAsync(StatusReportFilter filter, int pageSize, int pageIndex);

    /// <summary>
    /// POJO encapsulating a filter to query the repository for <see cref="StatusReport"/> items. 
    /// </summary>
    public record StatusReportFilter
    {
        /// <summary>
        /// Encapsulates a filter to query the repository for <see cref="StatusReport"/> items. 
        /// </summary>
        private StatusReportFilter(bool UsersReports,
            bool UsersContributions,
            bool HasTransportRequest,
            bool HasFundraiserRequest,
            Samaritan CurrentUser)
        {
            this.UsersReports = UsersReports;
            this.UsersContributions = UsersContributions;
            this.HasTransportRequest = HasTransportRequest;
            this.HasFundraiserRequest = HasFundraiserRequest;
            this.CurrentUser = CurrentUser;
        }

        /// <param name="usersReports">If the result should only contain reports created by the current logged-in user</param>
        /// <param name="usersContributions">If the result should only contain reports the current logged-in user has participated in</param>
        /// <param name="hasTransportRequest">If the result should only contain reports with transport request in them</param>
        /// <param name="hasFundraiserRequest">If the result should only contain reports with fundraising request in them</param>
        /// <param name="currentUser">Currently logged-in user</param>
        /// <returns></returns>
        public static StatusReportFilter CreateFilter(bool usersReports, bool usersContributions,
            bool hasTransportRequest,
            bool hasFundraiserRequest, Samaritan currentUser)
        {
            return new StatusReportFilter(usersReports, usersContributions, hasTransportRequest, hasFundraiserRequest,
                currentUser);
        }

        /// <summary>
        /// Returns a filtered <see cref="IQueryable{StatusReport}"/>.
        /// </summary>
        /// <param name="timelineItems">The <see cref="IQueryable{TimelineItem}"/> to perform the filtering on.</param>
        /// <param name="comments"></param>
        public IQueryable<StatusReport> Filter(IQueryable<TimelineItem> timelineItems, IQueryable<Comment> comments)
        {
            IQueryable<StatusReport> query = timelineItems.OfType<StatusReport>();
            query = UsersReports ? query.Where(item => item.CreatedBy == CurrentUser.Id) : query;
            query = UsersContributions ? query.Where(HasUserContributed(timelineItems, comments)) : query;
            query = HasTransportRequest ? query.Where(WithTransportRequest(timelineItems)) : query;
            query = HasFundraiserRequest ? query.Where(WithFundraiserRequest(timelineItems)) : query;
            return query;
        }

        private static Expression<Func<StatusReport, bool>> WithFundraiserRequest(IQueryable<TimelineItem> timelineItems)
        {
            return statusReport =>
                timelineItems.OfType<Bill>().Any(bill =>
                    bill.AnimalId == statusReport.AnimalId && bill.ContributionRequest != null);
        }

        private static Expression<Func<StatusReport, bool>> WithTransportRequest(IQueryable<TimelineItem> timelineItems)
        {
            return statusReport =>
                timelineItems.OfType<TransportRequest>().Any(item => item.AnimalId == statusReport.AnimalId);
        }

        private Expression<Func<StatusReport, bool>> HasUserContributed(
            IQueryable<TimelineItem> timelineItems,
            IQueryable<Comment> comments)
        {
            return statusReport =>
                // if there are timeline items created by the current user for the same report timeline
                timelineItems.Any(item => item.AnimalId == statusReport.AnimalId &&
                                          item.CreatedBy == CurrentUser.Id) ||
                // if there are comments by the current user on the report timeline
                timelineItems.OfType<Bill>().Any(bill => bill.AnimalId == statusReport.AnimalId &&
                                                         bill.ContributionRequest.Contributions.Any(contribution =>
                                                             contribution.ContributorId == CurrentUser.Id)) ||
                // if there are bills with a contribution from the current user
                comments.Any(comment => comment.CreatedBy == CurrentUser.Id &&
                                        timelineItems.Where(item => item.AnimalId == statusReport.AnimalId)
                                            .Select(item => item.Id)
                                            .Contains(comment.RepliesTo));
        }

        /// <summary>If the result should only contain reports created by the current logged-in user</summary>
        public bool UsersReports { get; init; }

        /// <summary>If the result should only contain reports the current logged-in user has participated in</summary>
        public bool UsersContributions { get; init; }

        /// <summary>If the result should only contain reports with transport request in them</summary>
        public bool HasTransportRequest { get; init; }

        /// <summary>If the result should only contain reports with fundraising request in them</summary>
        public bool HasFundraiserRequest { get; init; }

        /// <summary>Currently logged-in user</summary>
        public Samaritan CurrentUser { get; init; }
    }
}