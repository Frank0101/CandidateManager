using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using System;

namespace CandidateManager.Infra.Services
{
    public class AssessmentStatusService : IAssessmentStatusService
    {
        public AssessmentStatus GetAssessmentStatus(SessionModel session)
        {
            if (session.Status == SessionStatus.Created)
            {
                return AssessmentStatus.Unavailable;
            }
            if (session.Status == SessionStatus.Published)
            {
                if (DateTime.Now < session.AvailableFrom || DateTime.Now > session.AvailableTo)
                {
                    return AssessmentStatus.OutOfRange;
                }
                return AssessmentStatus.Available;
            }
            if (session.Status == SessionStatus.Started)
            {
                if ((DateTime.Now - session.StartedAt.Value).TotalHours > session.MaxDuration)
                {
                    return AssessmentStatus.Expired;
                }
                return AssessmentStatus.Started;
            }
            return AssessmentStatus.Submitted;
        }
    }
}
