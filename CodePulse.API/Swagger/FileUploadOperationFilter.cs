using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodePulse.API.Swagger
{
	// Operation filter to correctly describe multipart/form-data file uploads in Swagger
	public class FileUploadOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var hasFileParam = context.MethodInfo.GetParameters()
				.Any(p => p.ParameterType == typeof(IFormFile)
					   || p.ParameterType == typeof(IFormFile[])
					   || p.ParameterType == typeof(IEnumerable<IFormFile>));

			if (!hasFileParam) return;

			operation.Parameters?.Clear();

			var schema = new OpenApiSchema
			{
				Type = "object",
				Properties = new Dictionary<string, OpenApiSchema>()
			};

			foreach (var param in context.MethodInfo.GetParameters())
			{
				var name = param.Name;
				if (param.ParameterType == typeof(IFormFile)
				 || param.ParameterType == typeof(IFormFile[])
				 || param.ParameterType == typeof(IEnumerable<IFormFile>))
				{
					schema.Properties[name] = new OpenApiSchema { Type = "string", Format = "binary" };
				}
				else
				{
					schema.Properties[name] = new OpenApiSchema { Type = "string" };
				}
			}

			operation.RequestBody = new OpenApiRequestBody
			{
				Content =
			{
				["multipart/form-data"] = new OpenApiMediaType
				{
					Schema = schema
				}
			}
			};
		}
	}
}
