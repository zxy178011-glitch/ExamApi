using ExamApi.Services;

namespace ExamApi.Tests;

public class CandidateServiceTests
{
    private readonly CandidateService _service = new();

    [Fact]
    public void GenerateCandidates_WithValidCount_ReturnsCorrectNumber()
    {
        var result = _service.GenerateCandidates(25);

        Assert.Equal(25, result.Count);
        Assert.Equal("L0", result[0].Name);
        Assert.Equal("L24", result[24].Name);
    }

    [Fact]
    public void GenerateCandidates_WithCountLessThan20_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _service.GenerateCandidates(10));
    }

    [Fact]
    public void GenerateCandidates_With20_Succeeds()
    {
        var result = _service.GenerateCandidates(20);
        Assert.Equal(20, result.Count);
    }

    [Fact]
    public void Reorder_WithFourElements_ReturnsCorrectOrder()
    {
        // 题目示例: {1,2,3,4} → {1,4,2,3}
        var candidates = new List<ExamApi.Models.Candidate>
        {
            new() { Id = 1, Name = "L0" },
            new() { Id = 2, Name = "L1" },
            new() { Id = 3, Name = "L2" },
            new() { Id = 4, Name = "L3" },
        };

        var result = _service.Reorder(candidates);

        Assert.Equal(4, result.Count);
        Assert.Equal(1, result[0].Id); // L0
        Assert.Equal(4, result[1].Id); // Ln
        Assert.Equal(2, result[2].Id); // L1
        Assert.Equal(3, result[3].Id); // Ln-1
    }

    [Fact]
    public void Reorder_WithFiveElements_ReturnsCorrectOrder()
    {
        // {1,2,3,4,5} → {1,5,2,4,3}
        var candidates = Enumerable.Range(1, 5)
            .Select(i => new ExamApi.Models.Candidate { Id = i, Name = $"L{i - 1}" })
            .ToList();

        var result = _service.Reorder(candidates);

        Assert.Equal(5, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(5, result[1].Id);
        Assert.Equal(2, result[2].Id);
        Assert.Equal(4, result[3].Id);
        Assert.Equal(3, result[4].Id);
    }

    [Fact]
    public void Reorder_WithEmptyList_ReturnsEmptyList()
    {
        var result = _service.Reorder(new List<ExamApi.Models.Candidate>());
        Assert.Empty(result);
    }

    [Fact]
    public void Reorder_WithSingleElement_ReturnsSameElement()
    {
        var candidates = new List<ExamApi.Models.Candidate>
        {
            new() { Id = 1, Name = "L0" }
        };

        var result = _service.Reorder(candidates);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void Reorder_PreservesAllElements()
    {
        var candidates = _service.GenerateCandidates(25);
        var result = _service.Reorder(candidates);

        Assert.Equal(candidates.Count, result.Count);
        Assert.Equal(
            candidates.OrderBy(c => c.Id).Select(c => c.Id),
            result.OrderBy(c => c.Id).Select(c => c.Id));
    }
}
