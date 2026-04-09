using ExamApi.Models;

namespace ExamApi.Services;

public interface ICandidateService
{
    List<Candidate> GenerateCandidates(int count);
    List<Candidate> Reorder(List<Candidate> candidates);
}
