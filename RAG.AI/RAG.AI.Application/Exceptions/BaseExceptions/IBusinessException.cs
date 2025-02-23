using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Application.Exceptions.BaseExceptions
{
    public interface IBusinessException
    {
        // public ObjectResult ToHttpObjectResult()
        // {
        //     return new ObjectResult(new ProblemDetails()
        //     {
        //         Status = _statusCode,
        //         Title = "A business error occurred !",
        //         Detail = Message,
        //         Type = this.GetGenericTypeName()
        //     })
        //     {
        //         StatusCode = _statusCode
        //     };
        // }
    }
}



