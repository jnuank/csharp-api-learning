using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Api.Domain.Base;

[CollectionBuilder(typeof(SequenceEquatableList), "Create")]
public readonly struct SequenceEquatableList<T>(ImmutableList<T> items) : IEnumerable<T> where T : IEquatable<T>
{
	private ImmutableList<T> Items { get; } = items ?? [];

	public bool Equals(SequenceEquatableList<T> other) => Items.SequenceEqual(other.Items);

	public override bool Equals(object? obj) => obj is SequenceEquatableList<T> other && Items.SequenceEqual(other.Items);

	public override int GetHashCode()
	{
		var hash = new HashCode();
		foreach (var item in Items)
		{
			hash.Add(item.GetHashCode());
		}
		return hash.ToHashCode();
	}

	public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public static implicit operator SequenceEquatableList<T>(ImmutableList<T> list) => new(list);

	public override string ToString() => $"[{string.Join(", ", Items)}]";
	public static bool operator ==(SequenceEquatableList<T> left, SequenceEquatableList<T> right)
	 => left.Equals(right);

	public static bool operator !=(SequenceEquatableList<T> left, SequenceEquatableList<T> right)
	 => !(left == right);

}

public static class SequenceEquatableList
{
	public static SequenceEquatableList<T> Create<T>(ReadOnlySpan<T> items) where T : IEquatable<T> => new(ImmutableList.Create(items));

}