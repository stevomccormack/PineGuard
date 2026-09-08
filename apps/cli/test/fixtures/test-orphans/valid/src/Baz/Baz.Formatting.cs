namespace Sample.Baz;

// The other half of the same partial type — its own declaration of
// "partial class Baz" is a second, independent hit for the same name; the
// rule only needs one match across the whole project, from either file.
public partial class Baz
{
    public override string ToString() => Name;
}
