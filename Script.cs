Console.WriteLine("Hello, World!");


// int[] number = [1, 2, 3, 4, 5];

// Console.WriteLine(string.Join(", ", number[^3..]));

// A a = null;

// a.B = "C";

// Console.WriteLine(a?.B);

var score1 = new Score { Value = 10 };
var score2 = new Score { Value = 20 };

var result = score1.CompareTo(score2);
Console.WriteLine(result);

IComparable<Score> comparable = score1;
IComparable<Score> comparable2 = score1;

Console.WriteLine(score1.IsGreaterThan(score2));
Console.WriteLine(score1.IsLessThan(score2));
Console.WriteLine(score1.IsEqualTo(score2));


class A
{
	public string B { get; set; } = "B";
}

// trait 

public interface IComparable<T>
{
	int CompareTo(T other);

	// これは一度、IComparable<T>にキャストしてからじゃないと使えない
	// bool IsGreaterThan(T other) => CompareTo(other) > 0;
	// bool IsLessThan(T other) => CompareTo(other) < 0;
	// bool IsEqualTo(T other) => CompareTo(other) == 0;
}

// static class IComparableExtensions
// {
// 	extension<T>(IComparable<T> source)
// 	{
// 		public bool IsGreaterThan(T other) => source.CompareTo(other) > 0;
// 		public bool IsLessThan(T other) => source.CompareTo(other) < 0;
// 		public bool IsEqualTo(T other) => source.CompareTo(other) == 0;
// 	}

// }

public class Score : IComparable<Score>
{
	public int Value { get; set; }

	public int CompareTo(Score other)
	{
		return Value.CompareTo(other.Value);
	}
}

public static class ScoreExtensions
{
	public static bool IsGreaterThan<T>(this IComparable<T> score, T other)
		=> score.CompareTo(other) > 0;
	public static bool IsLessThan<T>(this IComparable<T> score, T other)
		=> score.CompareTo(other) < 0;
	public static bool IsEqualTo<T>(this IComparable<T> score, T other)
		=> score.CompareTo(other) == 0;
}
