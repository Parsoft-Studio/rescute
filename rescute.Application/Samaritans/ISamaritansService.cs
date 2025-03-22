using rescute.Domain.Aggregates;

namespace rescute.Application.Samaritans;

public interface ISamaritansService
{
    Task<Samaritan> GetOneSamaritan();
}