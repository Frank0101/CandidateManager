using CandidateManager.Core.Models;

namespace CandidateManager.Core.Services
{
    public interface IAssessmentStatusService
    {
        AssessmentStatus GetAssessmentStatus(SessionModel session);
    }
}
