using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Shared;

namespace rescute.Web.Pages.Report.ViewModel;

public class ChatViewModel
{
    private readonly Id<TimelineItem> parentTimelineItem;
    private IList<Comment> comments;

    public ChatViewModel(Id<TimelineItem> parentTimelineItem)
    {
        this.parentTimelineItem = parentTimelineItem;
    }

    public IList<String> GetComments()
    {
        return comments.Select(comment => comment.CommentText).ToList();
    }

    public void Post(String commentText, String username)
    {
    }

    public void UpdateComments(IList<Comment> comments)
    {
        this.comments = comments;
    }
}