using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.DTOs;

public class SubmitExamRequest
{
     
        public Guid ExamId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    

}

