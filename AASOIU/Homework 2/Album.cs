class Album
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Album(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Album() : this(0, "") { }

    public override string ToString() => $"[{Id}] {Name}";
}