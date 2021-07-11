using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Errors
{
    /// <summary>
    /// A canonical list of errors for the application
    /// </summary>
    public enum EErrorCode
    {
        [Description("Unexpected Error")]
        UnexpectedError,

        [Description("Permission missing")]
        Forbidden,

        [Description("The request is invalid")]
        InvalidRequest,

        [Description("Resource not found")]
        NotFound,

        [Description("Required Domain Object information is missing")]
        DomainObjectInfoMissing,

        [Description("Object Already exists")]
        ObjectConflict,

        [Description("Object not found")]
        ObjectNotFound
    }
}
