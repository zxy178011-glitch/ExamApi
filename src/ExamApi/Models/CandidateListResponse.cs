namespace ExamApi.Models;

public class CandidateListResponse
{
    public List<Candidate> Original { get; set; } = new();
    public List<Candidate> Reordered { get; set; } = new();
}
