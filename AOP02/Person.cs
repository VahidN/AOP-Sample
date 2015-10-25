using PropertyChanged;

namespace AOP02
{
    [ImplementPropertyChanged]
    public class Person
    {
        public string Id { set; get; }
        public string Name { set; get; }
    }
}

