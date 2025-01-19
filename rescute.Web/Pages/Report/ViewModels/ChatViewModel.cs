using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;

namespace rescute.Web.Pages.Report.ViewModels;

public class ChatViewModel
{
    private readonly Id<TimelineItem> parentTimelineItem;
    private IList<Comment> comments;

    public ChatViewModel(Id<TimelineItem> parentTimelineItem)
    {
        this.parentTimelineItem = parentTimelineItem;
    }

    public IList<string> GetComments()
    {
        return comments.Select(comment => comment.CommentText).ToList();
    }

    public void Post(string commentText, string username)
    {
    }

    public void UpdateComments(IList<Comment> comments)
    {
        this.comments = comments;
    }
}