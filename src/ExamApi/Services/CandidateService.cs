using ExamApi.Models;

namespace ExamApi.Services;

public class CandidateService : ICandidateService
{
    /// <summary>
    /// 随机产生考生列表 L0, L1, ..., Ln
    /// </summary>
    public List<Candidate> GenerateCandidates(int count)
    {
        if (count < 20)
            throw new ArgumentException("考生数量必须在 20 以上。", nameof(count));

        return Enumerable.Range(0, count)
            .Select(i => new Candidate { Id = i, Name = $"L{i}" })
            .ToList();
    }

    /// <summary>
    /// 将考生重新排列：L0, Ln, L1, Ln-1, L2, Ln-2, ...
    /// </summary>
    public List<Candidate> Reorder(List<Candidate> candidates)
    {
        if (candidates == null || candidates.Count == 0)
            return new List<Candidate>();

        var result = new List<Candidate>(candidates.Count);
        int left = 0;
        int right = candidates.Count - 1;

        while (left <= right)
        {
            result.Add(candidates[left]);
            if (left != right)
                result.Add(candidates[right]);
            left++;
            right--;
        }

        return result;
    }
}
