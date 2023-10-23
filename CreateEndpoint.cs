using Wolverine.Http;

namespace Wolverine.Http.Sample;

public class CreateEndpoint
{
    [WolverinePost("/issue")]
    public IssueCreated Create(CreateIssue command)
    {
        var id = Guid.NewGuid();
        var issue = new Issue
        {
            Id = id, Title = command.Title
        };

        return new IssueCreated(id);
    }
}

public record CreateIssue(string Title);

public record IssueCreated(Guid Id) : CreationResponse($"/issue/{Id}");

public class Issue
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}