using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.ExamService
{

    public class ExamConfigDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset OpenTimeUtc { get; set; }
        public DateTimeOffset CloseTimeUtc { get; set; }
        public List<PublicQuestionDto> Questions { get; set; } = new();
    }
}
