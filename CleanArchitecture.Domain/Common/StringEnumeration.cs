namespace SafeFleet.MediaManagement.Domain.SharedKernel;

using System.Reflection;

/// <summary>
/// Enumeration class
/// <inheritdoc>
///   <see>
///     <cref>https://docs.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types</cref>
///   </see>
/// </inheritdoc>
/// </summary>
public class StringEnumeration : IComparable<StringEnumeration>
{
  protected StringEnumeration(string id, string name)
  {
    Id = id;
    Name = name;
  }

  public string Name { get; }

  public string Id { get; }

  public int CompareTo(StringEnumeration other)
  {
    return other == null ? 1 : string.Compare(Name, other.Name, StringComparison.InvariantCulture);
  }

  public override string ToString()
  {
    return Name;
  }

  private static IEnumerable<T> GetAll<T>() where T : StringEnumeration
  {
    var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

    return fields.Select(f => f.GetValue(null)).Cast<T>();
  }

  public static bool operator ==(StringEnumeration x, StringEnumeration y)
  {
    return EqualOperator(x, y);
  }

  public static bool operator !=(StringEnumeration x, StringEnumeration y)
  {
    return !(x == y);
  }

  private static bool EqualOperator(StringEnumeration left, StringEnumeration right)
  {
    if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
    {
      return false;
    }

    return ReferenceEquals(left, null) || left.Equals(right);
  }

  public override bool Equals(object obj)
  {
    var otherValue = obj as StringEnumeration;

    if (otherValue == null)
    {
      return false;
    }

    var typeMatches = GetType().Equals(obj.GetType());
    var valueMatches = Id.Equals(otherValue.Id, StringComparison.InvariantCulture);

    return typeMatches && valueMatches;
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode();
  }

  public static T FromValue<T>(string value) where T : StringEnumeration
  {
    var matchingItem = Parse<T, string>(value, "value", item => item.Id == value);
    return matchingItem;
  }

  public static T FromDisplayName<T>(string displayName) where T : StringEnumeration
  {
    var matchingItem = Parse<T, string>(displayName, "display name",
      item => item.Name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
    return matchingItem;
  }

  private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : StringEnumeration
  {
    var matchingItem = GetAll<T>().FirstOrDefault(predicate);

    if (matchingItem == null)
    {
      throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
    }

    return matchingItem;
  }

  private static T TryParse<T>(Func<T, bool> predicate, T defaultValue) where T : StringEnumeration
  {
    var matchingItem = GetAll<T>().FirstOrDefault(predicate);
    return matchingItem ?? defaultValue;
  }

  public static T ToEnumerator<T>(string displayName, T defaultValue) where T : StringEnumeration
  {
    var matchingItem = TryParse(
      item => item.Name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase), defaultValue);
    return matchingItem;
  }

  public static bool operator <(StringEnumeration left, StringEnumeration right)
  {
    return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
  }

  public static bool operator <=(StringEnumeration left, StringEnumeration right)
  {
    return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
  }

  public static bool operator >(StringEnumeration left, StringEnumeration right)
  {
    return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
  }

  public static bool operator >=(StringEnumeration left, StringEnumeration right)
  {
    return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
  }
}
