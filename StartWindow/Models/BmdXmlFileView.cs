namespace StartWindow.Models
{
    public class BmdXmlFileView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Passing { get; set; } = true;

        public BmdXmlFileView(int id, string name, string value, bool passing)
        {
            Id = id;
            Name = name;
            Value = value;
            Passing = passing;
        }
    }
}
