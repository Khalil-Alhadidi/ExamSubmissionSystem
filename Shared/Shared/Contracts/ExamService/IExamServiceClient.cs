using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.ExamService;

public interface IExamServiceClient
{
    Task<ExamConfigDto?> GetExamConfigAsync(Guid examId);

}
