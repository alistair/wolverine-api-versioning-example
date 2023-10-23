using Wolverine.Http;

namespace Wolverine.Http.Sample;

public class HomeEndpoint
{
    [WolverineGet("/")]
    public IResult Index()
    {
        return Microsoft.AspNetCore.Http.Results.Redirect("/swagger");
    }
}