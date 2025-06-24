using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.ExamService;

public class ExamConfigResult
{
    public ExamConfigFetchStatus Status { get; set; }
    public ExamConfigDto? Config { get; set; }
}