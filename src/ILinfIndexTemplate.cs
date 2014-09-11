using System;
using System.Linq;

namespace Eklam.Lift
{
    /// <summary> 
    /// Implement this interface in a partial class representing your CRUD Index Template
    /// </summary>
    public interface ILinfIndexTemplate
    {
        Type Data { get; set; }
    }
}