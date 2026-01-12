using System.Collections.Immutable;

namespace Api.Domain.Base;

public readonly struct SequenceEquatableList<T> where T : IEquatable<T>
{
	public ImmutableList<T> Items { get; }

	public SequenceEquatableList(ImmutableList<T> items) => Items = items ?? [];

	public bool Equals(SequenceEquatableList<T> other) => Items.SequenceEqual(other.Items);

	public override bool Equals(object? obj) => obj is SequenceEquatableList<T> other && Equals(other);

	public override int GetHashCode()
	{
		var hash = new HashCode();
		foreach (var item in Items)
		{
			hash.Add(item.GetHashCode());
		}
		return hash.ToHashCode();
	}

	public static implicit operator SequenceEquatableList<T>(ImmutableList<T> list) => new(list);
}