using System.Threading.Tasks;
using GraphQL;
using GraphQL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

static class QueryExecutor
{
    public static async Task<object> ExecuteQuery(string queryString, ServiceCollection services, DbContext dataContext, Inputs inputs, GlobalFilters filters)
    {
        queryString = queryString.Replace("'", "\"");
        EfGraphQLConventions<MyDataContext>.RegisterInContainer(services, dataContext.Model, filters);
        using (var provider = services.BuildServiceProvider())
        using (var schema = new Schema(new FuncDependencyResolver(provider.GetRequiredService)))
        {
            var documentExecuter = new EfDocumentExecuter();

            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = queryString,
                UserContext = dataContext,
                Inputs = inputs
            };

            var executionResult = await documentExecuter.ExecuteWithErrorCheck(executionOptions);
            return executionResult.Data;
        }
    }
}